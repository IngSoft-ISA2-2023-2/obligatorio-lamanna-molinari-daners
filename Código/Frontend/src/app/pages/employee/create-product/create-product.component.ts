import { Component, OnInit } from '@angular/core';
import { cilBarcode, cilPencil, cilPaint, cilAlignCenter, cilDollar, cilLibrary, cilLoop1, cilTask, cilShortText } from '@coreui/icons';
import { AbstractControl, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Pharmacy } from '../../../interfaces/pharmacy';
import { UnitMeasureService } from '../../../services/unitmeasure.service';
import { UnitMeasure } from '../../../interfaces/unitmeasure';
import { PresentationService } from '../../../services/presentation.service';
import { Presentation } from '../../../interfaces/presentation';
import { ProductService } from '../../../services/product.service';
import { ProductRequest } from '../../../interfaces/product';
import { CommonService } from '../../../services/CommonService';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.css'],
})
export class CreateProductComponent implements OnInit {
  form: FormGroup | any;
  pharmacys: Pharmacy[] = [];

  icons = { cilBarcode, cilPencil, cilAlignCenter, cilLibrary,
    cilDollar, cilLoop1, cilTask, cilShortText, cilPaint };

  constructor(
    private commonService: CommonService,
    private productService: ProductService,
    private fb: FormBuilder
  ) {
    
    this.form = this.fb.group({
        name: new FormControl(),
        code: new FormControl(),
        description: new FormControl(), 
        price: new FormControl(),
      });
  }

  ngOnInit(): void {
    
  }

  get name(): AbstractControl {
    return this.form.controls.name;
  }

  get code(): AbstractControl {
    return this.form.controls.code;
  }

  get description(): AbstractControl {
    return this.form.controls.description;
  }

  get price(): AbstractControl {
    return this.form.controls.price;
  }

  createProduct(): void {
    let name = this.name.value ? this.name.value : "";
    let code = this.code.value ? this.code.value : "";
    let description = this.description.value ? this.description.value : "";
    let price = this.price.value ? this.price.value : 0;
    
    console.log("el precio fue" + price);

    let productRequest = new ProductRequest(code, name, description, price, "");
        this.productService.createProduct(productRequest).subscribe((product) => {
        this.form.reset();

        if (product){
          this.commonService.updateToastData(
            `Success creating "${product.code} - ${product.name}"`,
            'success',
            'Product created.'
          );
        }
      });

  }
}
