import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';

// DatePicker
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

// QR
import { QRCodeModule } from 'angularx-qrcode';

// Core Ui Components
import { FooterModule } from '@coreui/angular';
import { GridModule } from '@coreui/angular';
import { HeaderModule } from '@coreui/angular';
import { NavModule } from '@coreui/angular';
import { DropdownModule } from '@coreui/angular';
import { CardModule } from '@coreui/angular';
import { ButtonModule } from '@coreui/angular';
import { IconModule, IconSetService } from '@coreui/icons-angular';
import { BadgeModule } from '@coreui/angular';
import { UtilitiesModule } from '@coreui/angular';
import { FormModule } from '@coreui/angular';
import { TableModule } from '@coreui/angular';
import { AlertModule } from '@coreui/angular';
import { ToastModule } from '@coreui/angular';
import { BreadcrumbModule } from '@coreui/angular';
import { SidebarModule } from '@coreui/angular';
import { WidgetModule } from '@coreui/angular';
import { ModalModule } from '@coreui/angular';

// Pages
import { HomeComponent } from './pages/home/home/home.component';
import { CartComponent } from './pages/home/cart/cart.component';
import { ChoComponent } from './pages/home/cho/cho.component';


import { DetailComponent } from './pages/home/detail/detail.component';
import { TrackingComponent } from './pages/home/tracking/tracking.component';
import { Page404Component } from './pages/home/page404/page404.component';

// Admin
import { AdminComponent } from './pages/admin/admin/admin.component';
import { CreateInvitationComponent } from './pages/admin/create-invitation/create-invitation.component';
import { ListInvitationComponent } from './pages/admin/list-invitation/list-invitation.component';
import { UpdateInvitationComponent } from './pages/admin/update-invitation/update-invitation.component';
import { CreatePharmacyComponent } from './pages/admin/create-pharmacy/create-pharmacy.component';

// Employee
import { EmployeeComponent } from './pages/employee/employee/employee.component';
import { PurchaseStatusComponent } from './pages/employee/purchase-status/purchase-status.component';
import { CreateDrugComponent } from './pages/employee/create-drug/create-drug.component';
import { DeleteDrugComponent } from './pages/employee/delete-drug/delete-drug.component';
import { ExportDrugsComponent } from './pages/employee/export-drugs/export-drugs.component';

// Login & Register
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';

// Custom components
import {CustomFooterComponent} from './custom/custom-footer/custom-footer.component';
import {CustomHeaderComponent} from './custom/custom-header/custom-header.component';
import { StorageManager } from './utils/storage-manager';
import { CommonService } from './services/CommonService';
import { CustomToastComponent } from './custom/custom-toast/custom-toast.component';
import { StockRequestComponent } from './pages/employee/stock-request/stock-request.component';
import { CreateRequestComponent } from './pages/employee/create-request/create-request.component';
import { Page401Component } from './pages/home/page401/page401.component';
import { UserHeaderComponent } from './custom/custom-user-header/custom-user-header.component';
import { OwnerComponent } from './pages/owner/owner/owner.component';
import { PurchaseByDateComponent } from './pages/owner/purchase-by-date/purchase-by-date.component';

// Owner
import { InvitationComponent } from './pages/owner/invitation/invitation.component';
import { StockRequestOwnerComponent } from './pages/owner/stock-request-owner/stock-request-owner.component';

@NgModule({
  declarations: [
    AppComponent,
    // Pages
    HomeComponent,
    CartComponent,
    ChoComponent,
    DetailComponent,
    TrackingComponent,
    // Custom components
    CustomFooterComponent,
    CustomHeaderComponent,
    CustomToastComponent,
    // Login & Register
    LoginComponent,
    RegisterComponent,
    Page404Component,
    Page401Component,
    UserHeaderComponent,
    // Admin
    AdminComponent,
    CreateInvitationComponent,
    ListInvitationComponent,
    UpdateInvitationComponent,
    CreatePharmacyComponent,
    // Employee
    EmployeeComponent,
    PurchaseStatusComponent,
    CreateDrugComponent,
    DeleteDrugComponent,
    StockRequestComponent,
    CreateRequestComponent,
    ExportDrugsComponent,
    // Owner
    OwnerComponent,
    PurchaseByDateComponent,
    InvitationComponent,
    StockRequestOwnerComponent,
    

  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    //core ui
    FooterModule,
    GridModule,
    HeaderModule,
    NavModule,
    DropdownModule,
    CardModule,
    ButtonModule,
    IconModule,
    BadgeModule,
    UtilitiesModule,
    FormModule,
    TableModule,
    AlertModule,
    ToastModule,
    BreadcrumbModule,
    SidebarModule,
    WidgetModule,
    ModalModule,
    // QR
    QRCodeModule,
    // DatePicker
    BsDatepickerModule,
  ],
  providers: [IconSetService, StorageManager, CommonService],
  bootstrap: [AppComponent]
})
export class AppModule { }
