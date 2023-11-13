import { Component, OnInit } from '@angular/core';
import { cilCart, cilPlus, cilCompass } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { Drug } from '../../../interfaces/drug';
import { DrugService } from '../../../services/drug.service';
import { CommonService } from '../../../services/CommonService';
import { StorageManager } from 'src/app/utils/storage-manager';
import { Product } from 'src/app/interfaces/product';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  drugs: Drug[] = [];
  products: Product[] = [];
  cart: Drug[] = [];
  cartProduct: Product[] = []
  
  constructor(
    public iconSet: IconSetService,
    private drugService: DrugService,
    private productService: ProductService,
    private commonService: CommonService,
    private commonServiceProduct: CommonService,
    private storageManager: StorageManager) {
    iconSet.icons = { cilCart, cilPlus, cilCompass };

    this.commonService.onSearchDataUpdate.subscribe((data: any) => {
      this.drugs = data;
    });

    //esto no sería otro common service ??
    this.commonServiceProduct.onSearchDataUpdate.subscribe((data:any) => {
      this.products = data;
    });
  }

  ngOnInit(): void {
    this.updateCart();
    this.getDrugs();
    this.getProducts();
    this.storageManager.saveData('total', JSON.stringify(0));
  }

  getDrugs(): void {
    this.drugService.getDrugs()
    .subscribe((drugs: Drug[]) => this.drugs = drugs);
  }

  getProducts() : void {
    this.productService.getProducts()
    .subscribe((products: Product[]) => this.products = products);
  }

  updateCart(): void {
    this.cart = JSON.parse(this.storageManager.getData('cart'));
    if (!this.cart) {
      this.cart = [];
      this.storageManager.saveData('cart', JSON.stringify(this.cart));
    }
/*
    this.cartProduct = JSON.parse(this.storageManager.getData('cartProduct'));
    if(!this.cartProduct)
    {
      this.cartProduct = [];
      this.storageManager.saveData('cartProduct', JSON.stringify(this.cartProduct));
    }*/
    this.commonServiceProduct.updateHeaderData(this.cartProduct.length + this.cart.length);
  }
}
