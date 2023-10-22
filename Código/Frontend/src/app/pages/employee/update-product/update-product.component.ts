import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { cilPencil, cilShortText, cilSync } from '@coreui/icons';
import { Product, ProductRequest } from 'src/app/interfaces/product';
import { CommonService } from 'src/app/services/CommonService';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-update-product',
  templateUrl: './update-product.component.html',
  styleUrls: ['./update-product.component.css']
})
export class UpdateProductComponent implements OnInit {
  form: FormGroup | any;
  products: Product[] = [];
  icons = { cilShortText, cilPencil, cilSync }
  targetItem: any = undefined;
  visible = false;
  product: Product | any;

  constructor(
    private fb: FormBuilder,
    private commonService: CommonService,
    private productService: ProductService,
    private route: ActivatedRoute) {
    this.form = fb.group({
      productControl: 0,
      name: new FormControl(),
      price: new FormControl(),
      description: new FormControl(),
      code: new FormControl()
    });
    this.product = null;
  }

  ngOnInit(): void {
    this.getProductByUser();
  }

  getProductByUser() {
    this.productService.getProductByUser().subscribe((p: any) => (this.products = p));
  }

  setDefaultName(name: string): void {
    this.form.controls.name.setValue(name);
  }

  setDefaultPrice(price: number): void {
    this.form.controls.price.setValue(price);
  }

  setDefaultDescription(description: string): void {
    this.form.controls.description.setValue(description);
  }

  setDefaultCode(code: string): void {
    this.form.controls.code.setValue(code);
  }

  get product_name(): AbstractControl {
    return this.form.controls.name;
  }

  get price_value(): AbstractControl {
    return this.form.controls.price;
  }

  get description_value(): AbstractControl {
    return this.form.controls.description;
  }

  get code_value(): AbstractControl {
    return this.form.controls.code;
  }

  get product_id() {
    return this.form.controls.productControl;
  }

  updateProduct(): void {
    let name = this.product_name.value ? this.product_name.value : "";
    let price = this.price_value.value ? this.price_value.value : "";
    let description = this.description_value.value ? this.description_value.value : "";
    let code = this.code_value.value ? this.code_value.value : "";
    let id = this.product_id;

    let productRequest = new ProductRequest(code, name, price, description, "");
    this.productService.updateProduct(id, productRequest).subscribe((product: any) => {
      if (product) {
        this.commonService.updateToastData("Success updating product", 'success', 'Product updated.');
      }
    });
  }
}
