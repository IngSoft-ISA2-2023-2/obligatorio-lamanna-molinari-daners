export interface Pharmacy {
    id: number;
    name: string;
    address: string;
  }
  export class PharmacyRequest {
    name: string;
    address: string;

    constructor(
        pharmacyName: string,
        pharmacyAddress: string){
            this.name = pharmacyName;
            this.address = pharmacyAddress;
        }
  }