export class PurchaseRequest {
    buyerEmail: string = "";
    purchaseDate: string = "";
    details: PurchaseRequestDetail[] = [];

    constructor(buyerEmail: string, 
                    purchaseDate: string, 
                    details: PurchaseRequestDetail[]){
        this.buyerEmail = buyerEmail;
        this.purchaseDate = purchaseDate;
        this.details = details;
    }
}

export class PurchaseRequestDetail {
  code: string = "";
  quantity: number = 1;
  pharmacyId: number = 1;

  constructor(code: string, 
                        quantity: number, 
                        pharmacyId: number){
      this.code = code;
      this.quantity = quantity;
      this.pharmacyId = pharmacyId;
  }
}

export interface PurchaseResponse {
  id: number;
  buyerEmail: string;
  purchaseDate: string;
  trackingCode: string;
  totalAmount: number;
  details: PurchaseDetailModelResponse[]
}

export interface PurchaseDetailModelResponse {
  id: number;
  code: string;
  name: string;
  quantity: number;
  price: number;
  pharmacyId: number;
  pharmacyName: string;
  status: string;
}

export interface PurchaseModelResponseStatus {
  purchaseId: number;
  purchaseDetailId: number;
  drugCode: string;
  drugName: string;
  quantity: number;
  price: number;
  pharmacyId: number;
  pharmacyName: string;
  status: string;
}