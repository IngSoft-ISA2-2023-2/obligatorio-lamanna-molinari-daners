import { Component, OnInit } from '@angular/core';
import { cilCart, cilPlus, cilCompass, cilCheckCircle, cilTrash } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { Drug } from 'src/app/interfaces/drug';
import { StorageManager } from '../../../utils/storage-manager';
import { Router } from '@angular/router';
import { CommonService } from '../../../services/CommonService';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cart: Drug[] = [];
  total: number = 0;

  constructor(
    public iconSet: IconSetService,
    private storageManager: StorageManager,
    private router: Router,
    private commonService: CommonService) {
    iconSet.icons = { cilCart, cilPlus, cilCompass, cilCheckCircle, cilTrash };
  }

  ngOnInit(): void {
    this.cart = JSON.parse(this.storageManager.getData('cart'));
    if (!this.cart) {
      this.cart = [];
      this.storageManager.saveData('cart', JSON.stringify(this.cart));
    }
    this.storageManager.saveData('total', JSON.stringify(0));
    this.updateTotal();
  }

  delete(index: number){
    this.cart = JSON.parse(this.storageManager.getData('cart'));
    this.cart.splice(index, 1);
    this.storageManager.saveData('cart', JSON.stringify(this.cart));
    this.updateTotal();
    this.updateHeader(this.cart.length);
  }

  updateTotal(){
    this.total = 0;
    this.cart = JSON.parse(this.storageManager.getData('cart'));
    for(let item of this.cart){
      this.total += (item.price * item.quantity);
    }
  }

  updateHeader(value: number){
    this.commonService.updateHeaderData(value);
   }

  goToCho(){
    this.storageManager.saveData('total', JSON.stringify(this.total));
    this.router.navigate(['/home/cart/cho']);
  }
  
}
