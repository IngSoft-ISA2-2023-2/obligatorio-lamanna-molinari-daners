import { Pharmacy } from "./pharmacy";
import { Role } from "./role";

export interface Invitation {
    id: number;
    pharmacy: Pharmacy;
    userName: string;
    role: Role;
    userCode: string;
    isActive: boolean;
}

export class InvitationRequest {
    pharmacy: string;
    userName: string;
    role: string;
    userCode: string;

    constructor(
        pharmacyName: string,
        userName: string,
        roleName: string,
        userCode: string){
            this.pharmacy = pharmacyName;
            this.userName = userName;
            this.role = roleName;
            this.userCode = userCode;
        }
  }