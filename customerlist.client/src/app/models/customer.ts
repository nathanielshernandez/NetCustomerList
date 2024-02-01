import { Address } from "./address";

export interface Customer {
    customerID: number;
    firstName: string;
    lastName: string;
    companyName: string;
    address: Address;
}
