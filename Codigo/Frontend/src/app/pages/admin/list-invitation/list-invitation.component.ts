import { Component, Input, OnInit } from "@angular/core";
import { Invitation } from "src/app/interfaces/invitation";
import { CommonService } from "src/app/services/CommonService";
import { InvitationService } from "src/app/services/invitation.service";
import { cilSync } from '@coreui/icons';
import { Router } from "@angular/router";
import { Pharmacy } from "src/app/interfaces/pharmacy";
import { PharmacyService } from "src/app/services/pharmacy.service";
import { RoleService } from "src/app/services/role.service";
import { Role } from "src/app/interfaces/role";
import { TitleStrategy } from "@angular/router";

@Component({
    selector: 'app-list-invitation',
    templateUrl: './list-invitation.component.html',
    styleUrls: ['./list-invitation.component.css'],
})

export class ListInvitationComponent implements OnInit {
    pharmacies: Pharmacy[] = [];
    roles: Role[] = [];
    invitations: Invitation[] = [];
    pharmacyName: string = "";
    roleName: string = "";
    userName: string = "";

    @Input() select: boolean = true;
    @Input() search: boolean = true;

    icons = { cilSync };

    constructor(
        private invitationService: InvitationService,
        private commonService: CommonService,
        private route: Router,
        private pharmacyService: PharmacyService,
        private roleServices: RoleService){}

    ngOnInit(): void {
        this.getPharmacies();
        this.getRoles();
        this.getInvitations();
    };

    getPharmacies(): void {
        this.pharmacyService
          .getPharmacys()
          .subscribe((pharmacys) => (this.pharmacies = pharmacys));
      }
    
    getRoles(): void {
        this.roleServices
          .getRoles()
          .subscribe((roles) => (this.roles = roles));
    }

    getInvitations(): void {
        this.invitationService
        .getFilterInvitations("", "", "")
        .subscribe((invitations) => {
            this.invitations = invitations;
        })
    }

    getFilterInvitations(pharmacyName: string, roleName: string, userName: string): void {
        this.invitationService
        .getFilterInvitations(pharmacyName, userName, roleName)
        .subscribe((invitations) => {
            this.invitations = invitations;
        })
    }

    update(id: number): void {
        this.route.navigate(['admin/update-invitation', 
        { id: id }])
    }

    onPharmacyChange(name: any): void {
        this.pharmacyName = name;
        if (name === "0") {
            this.pharmacyName = "";
        }
        this.getFilterInvitations(this.pharmacyName, this.roleName, this.userName);
    }

    onRoleChange(name: any): void {
        this.roleName = name;
        if (name === "0") { 
            this.roleName = "";
        }
        this.getFilterInvitations(this.pharmacyName, this.roleName, this.userName);
    }

    onSearch() {
        this.getFilterInvitations(this.pharmacyName, this.roleName, this.userName);
    }
}