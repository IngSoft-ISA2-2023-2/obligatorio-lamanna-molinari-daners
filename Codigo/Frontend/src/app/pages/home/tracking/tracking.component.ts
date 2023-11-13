import { Component, OnInit } from '@angular/core';
import { cilCart, cilPlus, cilCompass } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { Drug } from 'src/app/interfaces/drug';
import { PurchaseResponse } from '../../../interfaces/purchase';
import { PurchaseService } from '../../../services/purchase.service';
import { StorageManager } from '../../../utils/storage-manager';
import { CommonService } from '../../../services/CommonService';

@Component({
  selector: 'app-tracking',
  templateUrl: './tracking.component.html',
  styleUrls: ['./tracking.component.css']
})
export class TrackingComponent implements OnInit {
  
  purchase: PurchaseResponse | undefined;
  code: string = "";
  tracking: string[] = [];
  cart: Drug[] = [];

  constructor(
    public iconSet: IconSetService,
    private purchaseService: PurchaseService,
    private storageManager: StorageManager,
    private commonService: CommonService) {
    iconSet.icons = { cilCart, cilPlus, cilCompass };
  }

  ngOnInit(): void {
    let track = JSON.parse(this.storageManager.getData('tracking'));
    if (!track){
      this.tracking = [];
    }else{
      this.tracking = track;
    }

    this.updateCart();
  }

  getPurchaseByTrackingCode(){
    this.purchaseService.getPurchaseByTrackingCode(this.code)
    .subscribe((p: PurchaseResponse) => this.purchase = p);

    if (this.tracking.length > 4){
      this.tracking.shift();
    }
    this.tracking.push(this.code);
    this.storageManager.saveData('tracking', JSON.stringify(this.tracking));
  }

  getColor(status: string) {
    if (status === 'Approved'){
      return "green";
    }else if (status === 'Rejected'){
      return "red";
    }
    // Pending
    return "orange";
  }

  updateCart() : void{
    this.cart = JSON.parse(this.storageManager.getData('cart'));
    if (!this.cart) {
      this.cart = [];
      this.storageManager.saveData('cart', JSON.stringify(this.cart));
    }
    this.commonService.updateHeaderData(this.cart.length);
  }

  
}
