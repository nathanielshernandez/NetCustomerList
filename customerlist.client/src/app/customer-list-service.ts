import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { Customer } from "./models/customer";

@Injectable({
  providedIn: "root"
})
export class CustomerListService {
  private customerListModified = new Subject<{ customer: Customer; modificationType: CustomerModificationType }>();

  public customerListModified$ = this.customerListModified.asObservable();

  emitCustomerListModified(customer: Customer, modificationType: CustomerModificationType) {
    this.customerListModified.next({ customer, modificationType });
  }
}

export enum CustomerModificationType {
  Added = "ADDED",
  Edited = "EDITED",
  Deleted = "DELETED"
}