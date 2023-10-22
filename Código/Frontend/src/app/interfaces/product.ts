export interface Product {
    id: number;
    code: string;
    name: string;
    price: number;
    description: string;
    pharmacy: {
        id: number;
        name: string;  
      };
  }

  export class ProductRequest {
    code: string;
    name: string;
    price: number;
    description: string;
    pharmacyName: string = "";

    constructor(code: string, name: string, price: number, description: string, pharmacyName: string){
      this.code = code;
      this.name = name;
      this.price = price;
      this.description = description;
      this.pharmacyName = pharmacyName;
    }
  }
  
  export class UpdateProductRequest {
    code: string;
    name: string;
    price: number;
    description: string;
  

    constructor(code: string, name: string, price: number, description: string){
      this.code = code;
      this.name = name;
      this.price = price;
      this.description = description;
    }
  }
