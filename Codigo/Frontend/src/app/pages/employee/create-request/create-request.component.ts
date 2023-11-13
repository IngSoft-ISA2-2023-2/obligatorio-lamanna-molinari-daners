import { Component, OnInit } from '@angular/core';
import { cilPlus, cilX, cilSend } from '@coreui/icons';
import { DrugService } from '../../../services/drug.service';
import { Drug } from '../../../interfaces/drug';
import { CommonService } from '../../../services/CommonService';
import { RequestDetail, RequestDetailHeader  } from 'src/app/interfaces/stock-request';
import { StockRequestService } from 'src/app/services/stock-request.service';

@Component({
  selector: 'app-create-request',
  templateUrl: './create-request.component.html',
  styleUrls: ['./create-request.component.css'],
})
export class CreateRequestComponent implements OnInit {

  quantity: number = 1;
  code: string = "";
  drugs: Drug[] = [];
  selectDrug: string = "";
  requests: RequestDetailHeader[] = [];
  icons = { cilPlus, cilX, cilSend};

  constructor(
    private commonService: CommonService,
    private stockRequestService: StockRequestService,
    private drugService: DrugService,
  ) {}

  ngOnInit(): void {
    this.getDrugsByUser();
  }

  onChange(e: any) {
    this.code = e.target.value;
  } 

  getDrugsByUser(){
    this.drugService.getDrugsByUser()
    .subscribe((d: Drug[]) => 
      { 
        this.drugs = d;
        this.code = this.drugs.length > 0 ? this.drugs[0].code : "";
      });
  }

  addRequest(): void {
    let exist: boolean = false;
    for(let item of this.requests){
      if (item.code === this.code){
        item.quantity += this.quantity;
        exist = true;
        break;
      }
    }
    if (!exist){
      let req = new RequestDetailHeader(this.code, this.quantity);
      this.requests.push(req);
    }
  }

  deleteRequest(index: number): void {
    this.requests.splice(index, 1);
  }

  createRequest(): void {
    this.stockRequestService.createStockRequest(this.requests)
    .subscribe((d: any) => {
      if (d && d.created){
        this.requests = [];
        this.commonService.updateToastData(
          `Success creating Stock Request`,
          'success',
          'Stock Request created.'
        );
     }
    });
  }

}
