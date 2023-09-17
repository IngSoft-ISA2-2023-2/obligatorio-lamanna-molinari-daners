import { Component, OnInit, ViewChild } from '@angular/core';
import { cilCheckAlt, cilX, cilSearch, cilCalendar, cilDelete  } from '@coreui/icons';
import { BsDatepickerConfig, BsDaterangepickerDirective } from 'ngx-bootstrap/datepicker';
import { StockRequest } from 'src/app/interfaces/stock-request';
import { StockRequestService } from 'src/app/services/stock-request.service';

@Component({
  selector: 'app-stock-request',
  templateUrl: './stock-request.component.html',
  styleUrls: ['./stock-request.component.css'],
})
export class StockRequestComponent implements OnInit {

  requests: StockRequest[] = [];
  icons = { cilCheckAlt, cilX, cilSearch, cilCalendar, cilDelete };
  searchValue: string = "";
  statusValue: string = "";
  statuses: string[]=["All","Approved","Pending", "Rejected"];

  @ViewChild(BsDaterangepickerDirective, { static: false }) rangepicker: 
    BsDaterangepickerDirective | undefined;
    
    customBsConfig: Partial<BsDatepickerConfig> = {
      adaptivePosition: true,
      clearButtonLabel: 'Clear',
      showClearButton: true,
      rangeInputFormat: 'DD/MM/YYYY',
      showTodayButton: true,
      containerClass: 'theme-dark-blue',
      showWeekNumbers: false,
      isAnimated: true,
    };
    dateRange: Date[] = [];
    range = {
        start: new Date(),
        end: new Date()
    };

  constructor(
    private stockRequestService: StockRequestService) {}

  ngOnInit(): void {
    this.statusValue = "";
    this.searchValue = "";
    this.getStockRequestsByEmployee("", "", undefined, undefined);
  }

  getStockRequestsByEmployee(status: string, code: string, start?: Date, end?: Date){
    if (start){
      start = new Date(start.getFullYear(), 
        start.getMonth(), 
        start.getDate(),
        0,0,0);
    }
    if (end){
      end = new Date(end.getFullYear(), 
        end.getMonth(), 
        end.getDate(),
        23,59,59);
    }
    
    this.stockRequestService.getStockRequests(status, code, start, end)
    .subscribe((d: any) => this.requests = d);
  }

  getColor(status: string) {
    if (status === 'Approved'){
      return "green";
    }else if (status === 'Rejected'){
      return "red";
    }
    return "orange";
  }

  open(): void {
    this.rangepicker?.show();
  }

  onSearch() {
    this.getStockRequestsByEmployee(this.statusValue, 
      this.searchValue, 
      this.dateRange[0], 
      this.dateRange[1]);   
  }

  onChangeSelect(status: any): void {
    this.statusValue = status;
    if (status === "All"){
      this.statusValue = "";  
    }
  }

  reset(): void {
    this.dateRange = [];
  }

}
