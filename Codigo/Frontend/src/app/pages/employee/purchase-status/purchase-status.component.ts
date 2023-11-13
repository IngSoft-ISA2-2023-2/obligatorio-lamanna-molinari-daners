import { Component, OnInit } from '@angular/core';
import { cilX, cilCheckAlt } from '@coreui/icons';
import { Router } from '@angular/router';
import { StorageManager } from 'src/app/utils/storage-manager';
import { CommonService } from 'src/app/services/CommonService';
import {
  PurchaseModelResponseStatus,
  PurchaseResponse,
} from 'src/app/interfaces/purchase';
import { PurchaseService } from '../../../services/purchase.service';

@Component({
  selector: 'app-purchase-status',
  templateUrl: './purchase-status.component.html',
  styleUrls: ['./purchase-status.component.css'],
})
export class PurchaseStatusComponent implements OnInit {
  icons = { cilX, cilCheckAlt };
  purchases: PurchaseResponse[] = [];
  response: PurchaseModelResponseStatus | undefined;
  visible = false;
  modalTitle = '';
  modalMessage = '';
  targetItem: any = undefined;
  targetPurchase: any = undefined;
  approveAction = true;

  constructor(
    private commonService: CommonService,
    private router: Router,
    private storageManager: StorageManager,
    private purchaseService: PurchaseService
  ) {
    this.getPurchases();
  }

  ngOnInit(): void {}

  getPurchases() {
    this.purchaseService
      .getPurchases()
      .subscribe((p: any) => (this.purchases = p));
  }

  getColor(status: string) {
    if (status === 'Approved') {
      return 'green';
    } else if (status === 'Rejected') {
      return 'red';
    }
    return 'orange';
  }

  approve(index: number): void {
    for (let purchase of this.purchases) {
      let details = purchase.details;
      for (let detail of details) {
        if (detail.id === index) {
          this.targetItem = detail;
          this.targetPurchase = purchase;
          break;
        }
      }
    }
    if (this.targetItem) {
      this.modalTitle = 'Approve Drug from Purchase';
      this.modalMessage = `Approving '${this.targetItem.code} - ${this.targetItem.name}' from Purchase. Are you sure ?`;
      this.visible = true;
      this.approveAction = true;
    }
  }

  reject(index: number): void {
    for (let purchase of this.purchases) {
      let details = purchase.details;
      for (let detail of details) {
        if (detail.id === index) {
          this.targetItem = detail;
          this.targetPurchase = purchase;
          break;
        }
      }
    }
    if (this.targetItem) {
      this.modalTitle = 'Reject Drug from Purchase';
      this.modalMessage = `Rejecting '${this.targetItem.code} - ${this.targetItem.name}' from Purchase. Are you sure ?`;
      this.visible = true;
      this.approveAction = false;
    }
  }

  closeModal(): void {
    this.visible = false;
  }

  saveModal(event: any): void {
    if (this.approveAction && event) {
      this.purchaseService
        .approvePurchase(
          this.targetPurchase.id,
          this.targetItem.pharmacyId,
          this.targetItem.code
        )
        .subscribe((p: any) => {
          this.visible = false;
          if (p) {
            this.response = p;
            this.targetItem.status = this.response?.status;
          }
        });
    } else if (!this.approveAction && event) {
      this.purchaseService
        .rejectPurchase(
          this.targetPurchase.id,
          this.targetItem.pharmacyId,
          this.targetItem.code
        )
        .subscribe((p: any) => {
          this.visible = false;
          if (p) {
            this.response = p;
            this.targetItem.status = this.response?.status;
            this.targetPurchase.totalAmount -=
              this.response!.price * this.response!.quantity;
          }
        });
    }
  }
}
