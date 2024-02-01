import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Customer } from '../../models/customer';
import { CustomerListService, CustomerModificationType } from '../../customer-list-service';

@Component({
  selector: 'app-edit-customer-form',
  templateUrl: './edit-customer-form.component.html',
})
export class EditCustomerFormComponent implements OnInit {
  // Input property to receive customer data from parent component
  @Input() customer?: Customer;

  // FormGroup to manage the form controls
  public customerForm = new FormGroup({
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

  ngOnInit(): void {
    // Populate form controls with customer data if available
    if (this.customer) {
      this.customerForm.patchValue({
        firstName: this.customer.firstName,
        lastName: this.customer.lastName,
        companyName: this.customer.companyName,
        street: this.customer.address.street,
        city: this.customer.address.city,
        state: this.customer.address.state,
        zip: this.customer.address.zip
      });
    }
  }

  /**
   * Close the edit-customer-form modal
   */
  public closeEditCustomerModal() {
    this.activeModal.close();
  }

  /**
   * Send a DELETE request to remove the customer
   */
  public deleteCustomer() {
    this.http.delete("http://localhost:5291/api/CustomerList/" + this.customer?.customerID)
      .subscribe({
        next: response => {
          console.log(response);
          if (this.customer) {
            console.log("Emitting customer list modified");
            this.customerService.emitCustomerListModified(this.customer, CustomerModificationType.Deleted);
          }
          this.closeEditCustomerModal();
        }
      });
  }

  /**
   * Send a PUT request to update the customer
   */
  public submitEditCustomerForm() {
    const newCustomer: Customer = {
      "customerID": this.customer?.customerID as unknown as number,
      "firstName": this.customerForm.get('firstName')?.value ?? "",
      "lastName": this.customerForm.get('lastName')?.value ?? "",
      "companyName": this.customerForm.get('companyName')?.value ?? "",
      "address": {
        "addressID": this.customer?.address?.addressID as unknown as number,
        "street": this.customerForm.get('street')?.value ?? "",
        "city": this.customerForm.get('city')?.value ?? "",
        "state": this.customerForm.get('state')?.value ?? "",
        "zip": this.customerForm.get('zip')?.value ?? -1,
      }
    };

    this.http.put("http://localhost:5291/api/customerlist", newCustomer)
      .subscribe({
        next: response => {
          this.customerService.emitCustomerListModified(response as Customer, CustomerModificationType.Edited);
          this.closeEditCustomerModal();
        }
      });
  }
}
