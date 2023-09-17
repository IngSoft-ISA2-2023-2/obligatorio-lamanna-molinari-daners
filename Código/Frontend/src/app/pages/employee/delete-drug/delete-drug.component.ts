import { Component, OnInit } from '@angular/core';
import { cilCheckAlt, cilX } from '@coreui/icons';
import { DrugService } from '../../../services/drug.service';
import { Drug } from '../../../interfaces/drug';
import { CommonService } from '../../../services/CommonService';

@Component({
  selector: 'app-delete-drug',
  templateUrl: './delete-drug.component.html',
  styleUrls: ['./delete-drug.component.css'],
})
export class DeleteDrugComponent implements OnInit {
  drugs: Drug[] = [];
  icons = { cilCheckAlt, cilX };
  targetItem: any = undefined;
  visible = false;
  modalTitle = '';
  modalMessage = '';

  constructor(
    private commonService: CommonService,
    private drugService: DrugService
  ) {}

  ngOnInit(): void {
    this.getDrugsByUser();
  }

  getDrugsByUser() {
    this.drugService.getDrugsByUser().subscribe((d: any) => (this.drugs = d));
  }

  deleteDrug(index: number): void {
    for (let drug of this.drugs) {
      if (drug.id === index) {
        this.targetItem = drug;
        break;
      }
    }
    if (this.targetItem) {
      this.modalTitle = 'Delete Drug';
      this.modalMessage = `Deleting '${this.targetItem.code} - ${this.targetItem.name}'. Are you sure ?`;
      this.visible = true;
    }
  }

  closeModal(): void {
    this.visible = false;
  }

  saveModal(event: any): void {
    if (event) {
      this.drugService.deleteDrug(this.targetItem.id).subscribe((p: any) => {
        if (p) {
          this.visible = false;
          this.getDrugsByUser();
          this.commonService.updateToastData(
            `Success deleting drug "${this.targetItem.code} - ${this.targetItem.name}"`,
            'success',
            'Drug deleted.'
          );
        }
      });
    }
  }
}
