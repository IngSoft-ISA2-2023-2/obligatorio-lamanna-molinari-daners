import { Component, OnInit } from '@angular/core';
import { cilX, cilCheckAlt } from '@coreui/icons';
import { StockRequest } from 'src/app/interfaces/stock-request';
import { CommonService } from 'src/app/services/CommonService';
import { StockRequestService } from 'src/app/services/stock-request.service';

@Component({
  selector: 'app-stock-request-owner',
  templateUrl: './stock-request-owner.component.html',
  styleUrls: ['./stock-request-owner.component.css']
})
export class StockRequestOwnerComponent implements OnInit {

  icons = { cilX, cilCheckAlt };
  requests: StockRequest[] = [];

  constructor(private stockRequestService: StockRequestService,
    private commonService: CommonService) { }

  ngOnInit(): void {
    this.getStockRequests();
  }

  getStockRequests(){
    this.stockRequestService.getStockRequestsByOwner().subscribe(s => this.requests = s);
  }

  getColor(status: string) {
    if (status === 'Approved'){
      return "green";
    }else if (status === 'Rejected'){
      return "red";
    }
    return "orange";
  }

  approve(requestId: number): void{
    this.stockRequestService.approveStockRequest(requestId).subscribe((p: any) => {
      if (p) {
        for (let request of this.requests) {
          if (request.id === requestId) {
            request.status = "Approved";
          }
        }
        this.commonService.updateToastData(
          `Success approving request with Id ${requestId}`,
          'success',
          'Success Approving.'
        );
      }
    });
  }


  reject(requestId: number): void{
    this.stockRequestService.rejectStockRequest(requestId).subscribe((p: any) => {
      if (p) {
        for (let request of this.requests) {
          if (request.id === requestId) {
            request.status = "Rejected";
          }
        }
        this.commonService.updateToastData(
          `Success rejecting request with Id ${requestId}`,
          'success',
          'Success Rejecting.'
        );
      }
    });
  }
}
