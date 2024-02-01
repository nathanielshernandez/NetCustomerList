import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EditCustomerFormComponent } from './components/edit-customer-form/edit-customer-form.component';
import { CustomerListService, CustomerModificationType } from './customer-list-service';
import { AddCustomerFormComponent } from './components/add-customer-form/add-customer-form.component';
import { Customer } from './models/customer';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  public title: string = 'customerlist.client';
  public customers: Customer[] = [];

  // Subscription to handle observables
  private subscription: Subscription = new Subscription();

  constructor(
    private http: HttpClient,
    private modalService: NgbModal,
    private customerListService: CustomerListService
  ) { }

  ngOnInit(): void {
    // Subscribe to the customerListModified$ observable in the customerListService
    this.subscription.add(
      this.customerListService.customerListModified$.subscribe({
        next: data => {
          this.handleCustomerModification(data.customer, data.modificationType);
        }
      })
    )

    // Fetch the initial list of customers from the API
    this.getCustomers().subscribe({
      next: (data) => {
        console.log(data);
        this.customers = data;
      },
    });
  }

  /**
   * Opens the edit-customer-form modal 
   * @param customer Customer model that populates the edit modal field
   */
  public openEditCustomerModal(customer: Customer): void {
    const modalRef = this.modalService.open(EditCustomerFormComponent);
    modalRef.componentInstance.customer = customer;
  }

  /**
   * Opens the add-customer-form modal
   */
  public openAddCustomerModal(): void {
    this.modalService.open(AddCustomerFormComponent);
  }

  /**
   * Fetches a list of customers from the API
   * @returns a list of Customers
   */
  public getCustomers(): Observable<Customer[]> { 
    return this.http.get<Customer[]>("http://localhost:5291/api/CustomerList");
  }

  /**
   * Determines the action taken on the in-memory customer list based on what modification has occurred
   * @param customer the customer that has been modified
   * @param modificationType the modification type
   * @returns
   */
  private handleCustomerModification(customer: Customer, modificationType: CustomerModificationType): void {
    if (modificationType === CustomerModificationType.Added) {
      this.customers.push(customer);
      return;
    }
    if (modificationType === CustomerModificationType.Edited) {
      const index = this.customers.findIndex(c => c.customerID === customer.customerID);
      this.customers[index] = customer;
      return;
    }
    if (modificationType === CustomerModificationType.Deleted) {
      const newCustomerArr: Customer[] = this.customers.filter(c => c.customerID !== customer.customerID);
      this.customers = newCustomerArr;
      return;
    }
  }
}
