import { Component, OnInit} from '@angular/core';
import { cilCart, cilPlus, cilCompass } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { ActivatedRoute } from '@angular/router';
import { StorageManager } from '../../../utils/storage-manager';
import { Router } from '@angular/router'; 
import { CommonService } from '../../../services/CommonService';

import { Product } from 'src/app/interfaces/product';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-detail-product',
  templateUrl: './detail-product.component.html',
  styleUrls: ['./detail-product.component.css']
})
export class DetailProductComponent implements OnInit {
  product: Product | undefined;
  quantity: number = 1;
  cart: any[] = [];
  cartProduct: any[] = [];

  constructor(
    private route: ActivatedRoute,
    public iconSet: IconSetService,
    private productService: ProductService,
    private storageManager: StorageManager,
    private router: Router,
    private commonService: CommonService
  ) {
    iconSet.icons = { cilCart, cilPlus, cilCompass };
  }

  ngOnInit(): void {
    this.getProduct();
    this.storageManager.saveData('total', JSON.stringify(0));
  }

  getProduct(): void {
    const id = parseInt(this.route.snapshot.paramMap.get('id')!, 10);
    this.productService.getProduct(id).subscribe((product) => (this.product = product));
  }

  addToCart(product: Product) {
    if (product) {
      this.cart = JSON.parse(this.storageManager.getData('cart'));
      if (!this.cart) {
        this.cart = [];
        this.storageManager.saveData('cart', JSON.stringify(this.cart));
      }
      
      let exist: boolean = false;
      for (let item of this.cart) {
        if (item.code === product.code && item.id === product.id){
          item.quantity += this.quantity;
          exist = true;
          break;
        }
      }
      if (!exist){
        product.quantity = this.quantity;
        this.cart.push(product);
      }
      this.storageManager.saveData('cart', JSON.stringify(this.cart));
    }
    this.addToCartProduct(product);
    this.updateHeader(this.cart.length);
    this.router.navigate(['/home/cart']);
  }
  addToCartProduct(product: Product) {
    if (product) {
      this.cartProduct = JSON.parse(this.storageManager.getData('cartProduct'));
      if (!this.cartProduct) {
        this.cartProduct = [];
        this.storageManager.saveData('cartProduct', JSON.stringify(this.cartProduct));
      }
      
      let exist: boolean = false;
      for (let item of this.cartProduct) {
        if (item.code === product.code && item.id === product.id){
          item.quantity += this.quantity;
          exist = true;
          break;
        }
      }
      if (!exist){
        product.quantity = this.quantity;
        this.cartProduct.push(product);
      }
      this.storageManager.saveData('cartProduct', JSON.stringify(this.cartProduct));
    }
    console.log(this.storageManager.getData('cartProduct'));
    this.updateHeader(this.cartProduct.length);
    this.router.navigate(['/home/cart']);
  }

  updateHeader(value: number){
    this.commonService.updateHeaderData(value);
   }

}
