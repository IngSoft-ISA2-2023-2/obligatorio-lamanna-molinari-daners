import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/services/CommonService';
import { StorageManager } from 'src/app/utils/storage-manager';
import { cilArrowThickRight } from '@coreui/icons';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})

export class AdminComponent implements OnInit {

  icons = { cilArrowThickRight };
  constructor(
    private commonService: CommonService,
    private route: Router,
    private storageManager: StorageManager) {}

  ngOnInit(): void {
  }
}
