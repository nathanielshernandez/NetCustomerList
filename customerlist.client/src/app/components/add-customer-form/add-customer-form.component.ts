import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { Customer } from '../../models/customer';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomerListService, CustomerModificationType } from '../../customer-list-service';

@Component({
  selector: 'app-add-customer-form',
  templateUrl: './add-customer-form.component.html',
})
export class AddCustomerFormComponent {

  // Define a FormGroup to manage the form controls
  public customerForm: FormGroup = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    companyName: new FormControl('', Validators.required),
    street: new FormControl(''),
    city: new FormControl(''),
    state: new FormControl(''),
    zip: new FormControl()
  });

  // Checks if customer form is valid
  public customerFormIsValid() {
    const firstNameIsValid = this.customerForm.get('firstName')?.valid;
    const lastNameIsValid = this.customerForm.get('lastName')?.valid;
    const companyNameIsValid = this.customerForm.get('companyName')?.valid;

    return firstNameIsValid && lastNameIsValid && companyNameIsValid;
  }

  constructor(
    private http: HttpClient,
    private activeModal: NgbActiveModal,
    private customerService: CustomerListService
  ) { }

  /**
   * Closes the add-customer-form modal
   */
  public closeAddCustomerModal(): void {
    this.activeModal.close();
  }

  /**
   * Send a POST request to add the new customer
   */
  public submitAddCustomerForm() {
    const newCustomer: Customer = {
      "customerID": -1,
      "firstName": this.customerForm.get('firstName')?.value ?? "",
      "lastName": this.customerForm.get('lastName')?.value ?? "",
      "companyName": this.customerForm.get('companyName')?.value ?? "",
      "address": {
        "addressID": -1,
        "street": this.customerForm.get('street')?.value ?? "",
        "city": this.customerForm.get('city')?.value ?? "",
        "state": this.customerForm.get('state')?.value ?? "",
        "zip": Number(this.customerForm.get('zip')?.value) ?? -1,
      }
    };

    this.http.post("http://localhost:5291/api/CustomerList", newCustomer)
      .subscribe({
        next: response => {
          if (response !== null) {
            const generatedCustomer = response as Customer;
            // Get the generated CustomerID and AddressID
            const generatedCustomerId: number = generatedCustomer.customerID;
            const generatedAddressId: number = generatedCustomer.address.addressID;
            // Update the IDs of the newCustomer object
            // This ensures that when the object is added to the in-memory list of customers,
            // it has the correct IDs
            newCustomer.customerID = generatedCustomerId;
            newCustomer.address.addressID = generatedAddressId;
            this.customerService.emitCustomerListModified(newCustomer, CustomerModificationType.Added);
            this.closeAddCustomerModal();
          }
        }
      });
    this.customerForm.reset();
  }
}
