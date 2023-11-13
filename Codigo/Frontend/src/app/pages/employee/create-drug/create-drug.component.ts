import { Component, OnInit } from '@angular/core';
import { cilBarcode, cilPencil, cilPaint, cilAlignCenter, cilDollar, cilLibrary, cilLoop1, cilTask, cilShortText } from '@coreui/icons';
import { AbstractControl, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Pharmacy } from '../../../interfaces/pharmacy';
import { UnitMeasureService } from '../../../services/unitmeasure.service';
import { UnitMeasure } from '../../../interfaces/unitmeasure';
import { PresentationService } from '../../../services/presentation.service';
import { Presentation } from '../../../interfaces/presentation';
import { DrugService } from '../../../services/drug.service';
import { DrugRequest } from '../../../interfaces/drug';
import { CommonService } from '../../../services/CommonService';

@Component({
  selector: 'app-create-drug',
  templateUrl: './create-drug.component.html',
  styleUrls: ['./create-drug.component.css'],
})
export class CreateDrugComponent implements OnInit {
  form: FormGroup | any;
  pharmacys: Pharmacy[] = [];
  unitMeasure: UnitMeasure[] = [];
  presentation: Presentation[] = [];

  icons = { cilBarcode, cilPencil, cilAlignCenter, cilLibrary,
    cilDollar, cilLoop1, cilTask, cilShortText, cilPaint };

  constructor(
    private commonService: CommonService,
    private drugService: DrugService,
    private unitMeasureService: UnitMeasureService,
    private presentationService: PresentationService,
    private fb: FormBuilder
  ) {
    
    this.form = this.fb.group({
        name: new FormControl(),
        code: new FormControl(),
        symptom: new FormControl(), 
        quantity: new FormControl(),
        price: new FormControl(),
        prescription: new FormControl(),
        presentationControl: 0,
        unitMeasureControl: 0,
        prescriptionControl: false
      });
  }

  ngOnInit(): void {
    this.getUnitMeasures();
    this.getPresentations();
  }

  getUnitMeasures(): void {
    this.unitMeasureService
    .getUnitMeasure()
    .subscribe((unit_measure) => {
      this.unitMeasure = unit_measure;
      this.setDefaultUnitMeasure();
    });
  }

  setDefaultUnitMeasure(): void {
    let unit = this.unitMeasure.length > 0 ? this.unitMeasure[0].id : 0;
    this.form.controls.unitMeasureControl.setValue(unit);
  }

  getPresentations(): void {
    this.presentationService
    .getPresentations()
    .subscribe((presentation) => {
      this.presentation = presentation;
      this.setDefaultPresentation();
    });
  }

  setDefaultPresentation(): void {
    let p = this.presentation.length > 0 ? this.presentation[0].id : 0;
    this.form.controls.presentationControl.setValue(p);

  }

  get name(): AbstractControl {
    return this.form.controls.name;
  }

  get code(): AbstractControl {
    return this.form.controls.code;
  }

  get symptom(): AbstractControl {
    return this.form.controls.symptom;
  }

  get quantity(): AbstractControl {
    return this.form.controls.quantity;
  }
  
  get price(): AbstractControl {
    return this.form.controls.price;
  }

  get presentation_id(): AbstractControl {
    return this.form.controls.presentationControl;
  }

  get unit_measure_id(): AbstractControl {
    return this.form.controls.unitMeasureControl;
  }

  get prescription(): AbstractControl {
    return this.form.controls.prescriptionControl;
  }

  createDrug(): void {
    let name = this.name.value ? this.name.value : "";
    let code = this.code.value ? this.code.value : "";
    let symptom = this.symptom.value ? this.symptom.value : "";
    let quantity = this.quantity.value ? this.quantity.value : 0;
    let price = this.price.value ? this.price.value : 0;
    let unitMeasureId = this.unit_measure_id.value ? this.unit_measure_id.value : 0;
    let presentationId = this.presentation_id.value ? this.presentation_id.value : 0;
    let prescription = this.prescription.value ? this.prescription.value : false;

    let drugRequest = new DrugRequest(code, name, symptom, quantity, price, prescription, 
                                      unitMeasureId, presentationId, "");
        this.drugService.createDrug(drugRequest).subscribe((drug) => {
        this.form.reset();
        this.setDefaultPresentation();
        this.setDefaultUnitMeasure();

        if (drug){
          this.commonService.updateToastData(
            `Success creating "${drug.code} - ${drug.name}"`,
            'success',
            'Drug created.'
          );
        }
      });

  }
}
