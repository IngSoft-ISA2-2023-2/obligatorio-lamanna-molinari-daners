import { Component, OnInit } from '@angular/core';
import { cilCart, cilPlus, cilCompass } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { Drug } from '../../../interfaces/drug';
import { DrugService } from '../../../services/drug.service';
import { CommonService } from '../../../services/CommonService';
import { StorageManager } from 'src/app/utils/storage-manager';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  drugs: Drug[] = [];
  cart: Drug[] = [];
  
  constructor(
    public iconSet: IconSetService,
    private drugService: DrugService,
    private commonService: CommonService,
    private storageManager: StorageManager) {
    iconSet.icons = { cilCart, cilPlus, cilCompass };

    this.commonService.onSearchDataUpdate.subscribe((data: any) => {
      this.drugs = data;
    });
  }

  ngOnInit(): void {
    this.updateCart();
    this.getDrugs();
    this.storageManager.saveData('total', JSON.stringify(0));
  }

  getDrugs(): void {
    this.drugService.getDrugs()
    .subscribe(drugs => this.drugs = drugs);
  }

  updateCart(): void {
    this.cart = JSON.parse(this.storageManager.getData('cart'));
    if (!this.cart) {
      this.cart = [];
      this.storageManager.saveData('cart', JSON.stringify(this.cart));
    }
    this.commonService.updateHeaderData(this.cart.length);
  }
}
