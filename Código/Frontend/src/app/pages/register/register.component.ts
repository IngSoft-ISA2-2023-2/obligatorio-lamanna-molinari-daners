import { Component, OnInit } from '@angular/core';
import { cilUser, cilLockLocked, cilAddressBook, cilPen } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { CommonService } from '../../services/CommonService';
import { UserService } from 'src/app/services/user.service';
import { UserRequest } from 'src/app/interfaces/user';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  form: FormGroup | any;

  constructor(
    public iconSet: IconSetService,
    private commonService: CommonService,
    private router: Router,
    private userService: UserService
  ) {
    iconSet.icons = { cilUser, cilLockLocked, cilAddressBook, cilPen };

    this.form = new FormGroup({
      username: new FormControl(),
      code: new FormControl(),
      email: new FormControl(),
      password: new FormControl(),
      confirm_password: new FormControl(),
      address: new FormControl(),
    });
  }

  onPasswordChange() {
    if (this.confirm_password.value == this.password.value) {
      this.confirm_password.setErrors(null);
    } else {
      this.confirm_password.setErrors({ mismatch: true });
    }
  }

  // getting the form control elements
  get password(): AbstractControl {
    return this.form.controls['password'];
  }

  get confirm_password(): AbstractControl {
    return this.form.controls['confirm_password'];
  }

  get username(): AbstractControl {
    return this.form.controls['username'];
  }

  get code(): AbstractControl {
    return this.form.controls['code'];
  }

  get email(): AbstractControl {
    return this.form.controls['email'];
  }

  get address(): AbstractControl {
    return this.form.controls['address'];
  }

  ngOnInit(): void {}

  goRegister(): void {
    let now = new Date().toISOString();
    let username = this.username.value ? this.username.value : '';
    let code = this.code.value ? this.code.value : '';
    let email = this.email.value ? this.email.value : '';
    let password = this.password.value ? this.password.value : '';
    let address = this.address.value ? this.address.value : '';

    let userRequest = new UserRequest(
      username,
      code,
      email,
      password,
      address,
      now
    );
    this.userService.createUser(userRequest).subscribe((user) => {
      if (user) {
        this.commonService.updateToastData(
          'Welcome: ' + user.userName,
          'success',
          'Register successful.'
        );
        this.router.navigate(['/login']);
      }
    });
  }
}
