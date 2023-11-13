import { Component, OnInit, ViewChild } from '@angular/core';
import { cilCalendar, cilSearch  } from '@coreui/icons';
import { BsDatepickerConfig, BsDaterangepickerDirective } from 'ngx-bootstrap/datepicker';
import { PurchaseService } from 'src/app/services/purchase.service';

@Component({
  selector: 'app-purchase-by-date',
  templateUrl: './purchase-by-date.component.html',
  styleUrls: ['./purchase-by-date.component.css'],
})
export class PurchaseByDateComponent implements OnInit {

  purchases: any[] = [];
  icons = {  cilCalendar, cilSearch };

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
    private purchaseService: PurchaseService,
  ) {}

  ngOnInit(): void {
    this.calanderConfigInit();
    this.getPurchasesByDate();
  }

  calanderConfigInit() {
    let date = new Date();
    let firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    this.dateRange[0] = firstDay;
    this.dateRange[1] = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 23, 59, 59);
  }

  getPurchasesByDate(){
    let start = this.dateRange[0];
    let end = this.dateRange[1];
    this.purchaseService.getPurchasesByDate(start, end)
    .subscribe((p: any) => this.purchases = p);
  }

  open(): void {
    this.rangepicker?.show();
  }

  getColor(status: string) {
    if (status === 'Approved'){
      return "green";
    }else if (status === 'Rejected'){
      return "red";
    }
    return "orange";
  }


}
