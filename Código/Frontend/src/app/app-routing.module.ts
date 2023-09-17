import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Pages
import { HomeComponent } from './pages/home/home/home.component';
import { CartComponent } from './pages/home/cart/cart.component';
import { ChoComponent } from './pages/home/cho/cho.component';
import { DetailComponent } from './pages/home/detail/detail.component';
import { TrackingComponent } from './pages/home/tracking/tracking.component';
import { LoginComponent } from './pages/login/login.component';
import { AdminComponent } from './pages/admin/admin/admin.component';
import { RegisterComponent } from './pages/register/register.component';
import { Page404Component } from './pages/home/page404/page404.component';
import { EmployeeComponent } from './pages/employee/employee/employee.component';
import { PurchaseStatusComponent } from './pages/employee/purchase-status/purchase-status.component';
import { CreateDrugComponent } from './pages/employee/create-drug/create-drug.component';
import { DeleteDrugComponent } from './pages/employee/delete-drug/delete-drug.component';
import { StockRequestComponent } from './pages/employee/stock-request/stock-request.component';
import { CreateRequestComponent } from './pages/employee/create-request/create-request.component';
import { ExportDrugsComponent } from './pages/employee/export-drugs/export-drugs.component';
import { AuthenticationGuard } from './guards/authentication.guard';
import { Page401Component } from './pages/home/page401/page401.component';
import { CreateInvitationComponent } from './pages/admin/create-invitation/create-invitation.component';
import { ListInvitationComponent } from './pages/admin/list-invitation/list-invitation.component';
import { UpdateInvitationComponent } from './pages/admin/update-invitation/update-invitation.component';
import { OwnerComponent } from './pages/owner/owner/owner.component';
import { PurchaseByDateComponent } from './pages/owner/purchase-by-date/purchase-by-date.component';
import { InvitationComponent } from './pages/owner/invitation/invitation.component';
import { CreatePharmacyComponent } from './pages/admin/create-pharmacy/create-pharmacy.component';
import { StockRequestOwnerComponent } from './pages/owner/stock-request-owner/stock-request-owner.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'home/cart', component: CartComponent },
  { path: 'home/cart/cho', component: ChoComponent },
  { path: 'home/detail/:id', component: DetailComponent },
  { path: 'home/tracking', component: TrackingComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'employee', component: EmployeeComponent, canActivate: [AuthenticationGuard], data: {roles: ['Employee'] }},
  { path: 'employee/purchase-status', component: PurchaseStatusComponent, canActivate: [AuthenticationGuard], data: {roles: ['Employee'] } },
  { path: 'employee/delete-drug', component: DeleteDrugComponent, canActivate: [AuthenticationGuard], data: {roles: ['Employee'] } },
  { path: 'employee/create-drug', component: CreateDrugComponent, canActivate: [AuthenticationGuard], data: {roles: ['Employee'] } },
  { path: 'employee/stock-request', component: StockRequestComponent, canActivate: [AuthenticationGuard], data: {roles: ['Employee'] } },
  { path: 'employee/create-request', component: CreateRequestComponent, canActivate: [AuthenticationGuard], data: {roles: ['Employee'] } },
  { path: 'employee/export-drugs', component: ExportDrugsComponent, canActivate: [AuthenticationGuard], data: {roles: ['Employee'] } },
  { path: 'admin', component: AdminComponent, canActivate: [AuthenticationGuard], data: {roles: ['Administrator'] }},
  { path: 'admin/create-invitation', component: CreateInvitationComponent, canActivate: [AuthenticationGuard], data: {roles: ['Administrator'] }},
  { path: 'admin/list-invitation', component: ListInvitationComponent, canActivate: [AuthenticationGuard], data: {roles: ['Administrator'] }},
  { path: 'admin/update-invitation', component: UpdateInvitationComponent, canActivate: [AuthenticationGuard], data: {roles: ['Administrator'] }},
  { path: 'admin/create-pharmacy', component: CreatePharmacyComponent, canActivate: [AuthenticationGuard], data: {roles: ['Administrator'] }},
  { path: 'owner', component: OwnerComponent, canActivate: [AuthenticationGuard], data: {roles: ['Owner'] }},
  { path: 'owner/purchase-by-date', component: PurchaseByDateComponent, canActivate: [AuthenticationGuard], data: {roles: ['Owner'] }},
  { path: 'owner/invitation', component: InvitationComponent, canActivate: [AuthenticationGuard], data: {roles: ['Owner']}},
  { path: 'owner/stock-request', component: StockRequestOwnerComponent, canActivate: [AuthenticationGuard], data: {roles: ['Owner'] } },
  { path: 'unauthorized', component: Page401Component },
  { path: '**', component: Page404Component }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
