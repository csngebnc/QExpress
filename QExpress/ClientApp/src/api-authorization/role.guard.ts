import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { AuthorizeService } from "./authorize.service";

@Injectable()
export class RoleGuard implements CanActivate {

  constructor(private _auth: AuthorizeService,
    private _router: Router) { }

    redirects = {
        "0": "authentication/login",
        "1": "/",
        "2": "waiting",
        "3": "employee/list",
        "4": "company/list"
    }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    const expectedRole = route.data.expectedRole
    const role = this._auth.getUserRole()
    if (this._auth.isAuthenticated() && expectedRole == role) {
      return true;
    } else {
      this._router.navigate(this.redirects[role.toString()]);
      return false;
    }
  }
}