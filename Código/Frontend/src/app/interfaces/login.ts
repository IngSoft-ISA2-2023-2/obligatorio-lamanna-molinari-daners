export class LoginRequest {
  userName: string = "";
  password: string = "";

    constructor(userName: string, 
                password: string){
        this.userName = userName;
        this.password = password;
    }
}


export interface LoginResponse {
  token: string;
  role: string;
  userName: string;
}


