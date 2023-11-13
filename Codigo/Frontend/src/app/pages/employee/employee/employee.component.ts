import { Component, OnInit } from '@angular/core';
import { cilUser, cilLockLocked, cilArrowThickRight } from '@coreui/icons';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css'],
})
export class EmployeeComponent implements OnInit {

  icons = { cilUser, cilLockLocked, cilArrowThickRight };
  constructor() {}
  ngOnInit(): void {}

}
