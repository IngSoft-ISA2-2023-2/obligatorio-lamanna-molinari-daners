export interface StockRequest {
    id: number;
    status: string;
    requestDate: string;
    details: StockRequestDetail[]
}

export interface StockRequestDetail{
    quantity: number;
    drug: StockRequestDetailDrug;
}

export interface StockRequestDetailDrug{
    code: string;
    name: string;
    stock: number;
}

export class RequestDetail {
    details: RequestDetailHeader[];
    constructor(details: RequestDetailHeader[]){
        this.details = details;
    }
}

export class RequestDetailHeader {
    drug: any;
    code: string;
    quantity: number;
    constructor(code: string, quantity: number){
        this.code = code;
        this.quantity = quantity;
        this.drug = new RequestDetailDrug(this.code)
    }
}

export class RequestDetailDrug {
    code: string = "";
    constructor(code: string){
        this.code = code;
    }
}