import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { StorageManager } from '../utils/storage-manager';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationGuard implements CanActivate {
  
    constructor(private storageManager: StorageManager, private router: Router){}
  
    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        let roles = route.data['roles'] as Array<string>;
        let login = JSON.parse(this.storageManager.getData("login"));
        if (login){
            for(let role of roles){
                if (role === login.role){
                    return true;
                }
            }
        }
        this.router.navigate(['/unauthorized']);
        return false;
    }
}
