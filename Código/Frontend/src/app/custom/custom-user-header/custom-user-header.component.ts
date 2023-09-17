import { Component, Input } from '@angular/core';
import { cilAccountLogout, cilMenu, cilCog } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { ClassToggleService, HeaderComponent } from '@coreui/angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-header',
  templateUrl: './custom-user-header.component.html',
})
export class UserHeaderComponent extends HeaderComponent {

  @Input() sidebarId: string = "sidebar";
  @Input() title: string = "";
  @Input() link: string = "";

  constructor(
    private classToggler: ClassToggleService,
    private router: Router,
    public iconSet: IconSetService) {
      iconSet.icons = {cilAccountLogout, cilMenu, cilCog};     
    super();
  }

  logout(): void {
    this.router.navigate(['login']);
  }

}