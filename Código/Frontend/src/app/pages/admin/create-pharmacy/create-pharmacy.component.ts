import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { cilShortText, cilPencil } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { PharmacyRequest } from 'src/app/interfaces/pharmacy';
import { CommonService } from 'src/app/services/CommonService';
import { PharmacyService } from 'src/app/services/pharmacy.service';

@Component({
  selector: 'app-create-pharmacy',
  templateUrl: './create-pharmacy.component.html',
  styleUrls: ['./create-pharmacy.component.css']
})
export class CreatePharmacyComponent implements OnInit {
  form: FormGroup | any;

  icons = { cilShortText, cilPencil }

  constructor(
    private fb: FormBuilder,
    private commonService: CommonService,
    private pharmacyService: PharmacyService
  ){
    this.form = fb.group({
      name: new FormControl(),
      address: new FormControl()
    })
  };

  ngOnInit(): void {
  }

  get pharmacy_name(): AbstractControl {
    return this.form.controls.name;
  }

  get pharmacy_address(): AbstractControl {
      return this.form.controls.address;
  }

  createPharmacy() : void{
    let pharmacyName = this.pharmacy_name.value ? this.pharmacy_name.value : "";
    let pharmacyAddress = this.pharmacy_address.value ? this.pharmacy_address.value : "";

    let pharmacyRequest = new PharmacyRequest(pharmacyName, pharmacyAddress);
    this.pharmacyService.createPharmacy(pharmacyRequest).subscribe((pharmacy) => {
      if (pharmacy){
        this.form.reset();
        this.commonService.updateToastData("Success creating pharmacy", "success", "Pharmacy created.");
      }
    })
  }
}
