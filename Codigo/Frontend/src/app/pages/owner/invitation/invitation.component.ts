import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Pharmacy } from 'src/app/interfaces/pharmacy';
import { Role } from 'src/app/interfaces/role';
import { cilShortText, cilPencil } from '@coreui/icons';
import { PharmacyService } from 'src/app/services/pharmacy.service';
import { RoleService } from 'src/app/services/role.service';
import { InvitationRequest } from 'src/app/interfaces/invitation';
import { InvitationService } from 'src/app/services/invitation.service';
import { CommonService } from 'src/app/services/CommonService';

@Component({
    selector: 'app-invitation',
    templateUrl: './invitation.component.html',
    styleUrls: ['./invitation.component.css'],
})

export class InvitationComponent implements OnInit {
    form: FormGroup | any;
    pharmacies: Pharmacy[] = [];
    roles: Role[] = [];

    icons = { cilShortText, cilPencil }

    constructor(
        private fb: FormBuilder,
        private pharmacyService: PharmacyService,
        private roleService: RoleService,
        private invitationService: InvitationService,
        private commonService: CommonService) {
            this.form = fb.group({
                pharmacyControl: 0,
                userName: new FormControl(),
                roleControl: 0 
        });
    };

    ngOnInit(): void {
        this.getPharmacies();
        this.getRoles();
    };

    getPharmacies(): void {
        this.pharmacyService
        .getPharmacys()
        .subscribe((pharmacies) => {
            this.pharmacies = pharmacies;
            this.setDefaultPharmacy();
        })
    }

    setDefaultPharmacy(): void {
        let name = "";
        this.form.controls.pharmacyControl.setValue(name);
    }

    getRoles(): void {
        this.setDefaultRole();
    }

    setDefaultRole(): void {
        let name = "Employee";
        this.form.controls.roleControl.setValue(name);
    }

    get pharmacy_name(): AbstractControl {
        return this.form.controls.pharmacyControl;
    }

    get user_name(): AbstractControl {
        return this.form.controls.userName;
    }

    get role_name(): AbstractControl {
        return this.form.controls.roleControl;
    }

    createInvitation() : void{
        let pharmacyName = this.pharmacy_name.value ? this.pharmacy_name.value : "";
        let userName = this.user_name.value ? this.user_name.value : "";
        let roleName = this.role_name.value ? this.role_name.value : "";

        let invitationRequest = new InvitationRequest("", userName, "", "");
        this.invitationService.createInvitation(invitationRequest).subscribe((invitation) => {
            if (invitation){
                this.form.reset();
                this.setDefaultPharmacy();
                this.setDefaultRole();
                this.commonService.updateToastData("Success creating invitation", 'success', 'Invitation created.');
          }
        });
    }
}