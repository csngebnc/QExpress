import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import {RouterModule} from '@angular/router';
import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {ApiAuthorizationModule} from 'src/api-authorization/api-authorization.module';
import {AuthorizeInterceptor} from 'src/api-authorization/authorize.interceptor';
import {CompaniesComponent} from './admin/companies/companies.component';
import {RegistercompanyComponent} from './admin/registercompany/registercompany.component';
import {CategoriesComponent} from './companyadmin/categories/categories.component';
import {EmployeesComponent} from './companyadmin/employees/employees.component';
import {EditcategoryComponent} from './companyadmin/editcategory/editcategory.component';
import {EditemployeeComponent} from './companyadmin/editemployee/editemployee.component';
import {EditsiteComponent} from './companyadmin/editsite/editsite.component';
import {SitesComponent} from './companyadmin/sites/sites.component';
import {WaitingComponent} from './employee/waiting/waiting.component';
import {CurrentComponent} from './queue/current/current.component';
import {HistoryComponent} from './queue/history/history.component';
import {EditUserComponent} from './user/edit-user/edit-user.component';
import {MatDialogModule} from '@angular/material/dialog';
import {NewDialog} from './queue/current/current.component';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {EditCompanyComponent} from './admin/edit-company/edit-company.component';
import { AddsiteComponent } from './companyadmin/addsite/addsite.component';
import { AddemployeeComponent } from './companyadmin/addemployee/addemployee.component';
import {AddCategoryComponent} from "./companyadmin/add-category/add-category.component";
import { RoleGuard } from 'src/api-authorization/role.guard';

var routes = [
  {
    component: HistoryComponent,
    path: 'history',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 1
    }
  },
  {
    component: WaitingComponent,
    path: 'waiting',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 2
    }
  },
  {
    component: CurrentComponent,
    path: '',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 1
    }
  },
  {
    component: CompaniesComponent,
    path: 'company/list',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 4
    }
  },
  {
    component: EditCompanyComponent,
    path: 'company/edit/:companyid',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 4
    }
  },
  {
    component: RegistercompanyComponent,
    path: 'company/add',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 4
    }
  },
  {
    component: EditsiteComponent,
    path: 'site/edit/:siteid',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 3
    }
  },
  {
    component: SitesComponent,
    path: 'site/list',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 3
    }
  },
  {
    component: AddsiteComponent,
    path: 'site/add',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 3
    }
  },
  {
    component: AddemployeeComponent,
    path: 'employee/add',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 3
    }
  },
  {
    component: EditemployeeComponent,
    path: 'employee/edit/:employeeid',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 3
    }
  },
  {
    component: EmployeesComponent,
    path: 'employee/list',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 3
    }
  },
  {
    component: EditcategoryComponent,
    path: 'category/edit/:categoryid',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 3
    }
  },
  {
    component: AddCategoryComponent,
    path: 'category/add',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 3
    }
  },
  {
    component: CategoriesComponent,
    path: 'category/list',
    canActivate: [RoleGuard],
    data: {
      expectedRole: 3
    }
  },
];

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    CurrentComponent,
    HistoryComponent,
    EditUserComponent,
    EditcategoryComponent,
    AddCategoryComponent,
    EditemployeeComponent,
    EditsiteComponent,
    SitesComponent,
    CategoriesComponent,
    EmployeesComponent,
    RegistercompanyComponent,
    EditCompanyComponent,
    CompaniesComponent,
    WaitingComponent,
    NewDialog,
    AddsiteComponent,
    AddemployeeComponent,
  ],
  entryComponents: [
    NewDialog,
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes),
    MatDialogModule,
    FontAwesomeModule,
    BrowserAnimationsModule,
    ApiAuthorizationModule,
  ],
  exports: [
    MatDialogModule,
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true},
    RoleGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}
