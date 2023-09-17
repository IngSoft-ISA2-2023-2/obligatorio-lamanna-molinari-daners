import { Component, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { Pharmacy } from "src/app/interfaces/pharmacy";
import { Role } from "src/app/interfaces/role";
import { CommonService } from "src/app/services/CommonService";
import { InvitationRequest } from 'src/app/interfaces/invitation';
import { InvitationService } from "src/app/services/invitation.service";
import { PharmacyService } from "src/app/services/pharmacy.service";
import { RoleService } from "src/app/services/role.service";
import { cilShortText, cilPencil, cilSync } from '@coreui/icons';
import { ActivatedRoute, Router } from "@angular/router";
import { Invitation } from "src/app/interfaces/invitation";

@Component({
    selector: 'app-update-invitation',
    templateUrl: './update-invitation.component.html',
    styleUrls: ['./update-invitation.component.css'],
})

export class UpdateInvitationComponent implements OnInit {
    form: FormGroup | any;
    pharmacies: Pharmacy[] = [];
    roles: Role[] = [];
    invitation: Invitation | any;

    icons = { cilShortText, cilPencil, cilSync }

    constructor(
        private fb: FormBuilder,
        private pharmacyService: PharmacyService,
        private roleService: RoleService,
        private invitationService: InvitationService,
        private commonService: CommonService,
        private route: ActivatedRoute) {
            this.form = fb.group({
                pharmacyControl: 0,
                userName: new FormControl(),
                roleControl: 0 ,
                userCode: new FormControl()
            });
            this.invitation = null;
        };

    ngOnInit(): void {
        this.getPharmacies();
        this.getRoles();

        let id = this.route.snapshot.paramMap.get('id');
        this.getInvitationById(id);
    }

    getInvitationById(id: any): void {
        this.invitationService
        .getInvitationById(id)
        .subscribe((invitation) => {
            this.invitation = invitation;
            console.log(invitation);

            if (this.invitation.pharmacy !== null) {
                this.setDetaultPharmacy(this.invitation.pharmacy.name);
            };

            this.setDefaultUserName(this.invitation.userName);

            if (this.invitation.role !== null) {
                this.setDetaultRole(this.invitation.role.name);
            };
            console.log(this.invitation.userCode);
            this.setDefaultUserCode(this.invitation.userCode);
        })
        
    }

    getPharmacies(): void {
        this.pharmacyService
        .getPharmacys()
        .subscribe((pharmacies) => {
            this.pharmacies = pharmacies;
            
        })
    }
    getRoles(): void {
        this.roleService
        .getRoles()
        .subscribe((roles) => {
            this.roles = roles;
        })
    }

    setDetaultPharmacy(pharmacyName: string): void {
            this.form.controls.pharmacyControl.setValue(pharmacyName);
    }
    setDefaultUserName(userName: string): void {
        this.form.controls.userName.setValue(userName);
    }
    setDetaultRole(roleName: string): void {
        this.form.controls.roleControl.setValue(roleName);
    }
    setDefaultUserCode(userCode: string): void {
        this.form.controls.userCode.setValue(userCode);
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

    get user_code(): AbstractControl {
        return this.form.controls.userCode;
    }

    get invitation_id() {
        return Number(this.route.snapshot.paramMap.get('id'));;
    }


    newUserCode(): void {
        this.invitationService.getNewUserCode().subscribe((invitation) => {
            console.log(invitation.userCode);
            this.setDefaultUserCode(invitation.userCode);
        })
    }

    updateInvitation(): void {
        let pharmacyName = this.pharmacy_name.value ? this.pharmacy_name.value : "";
        let userName = this.user_name.value ? this.user_name.value : "";
        let roleName = this.role_name.value ? this.role_name.value : "";
        let userCode = this.user_code.value ? this.user_code.value : "";
        let id = this.invitation_id;

        let invitationRequest = new InvitationRequest(pharmacyName, userName, roleName, userCode);
        this.invitationService.updateInvitation(id, invitationRequest).subscribe((invitation) => {
            if (invitation){
                this.commonService.updateToastData("Success updating invitation", 'success', 'Invitation updated.');
            }
        });
    }
}