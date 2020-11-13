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

var routes = [
  {
    component: EditUserComponent,
    path: 'profile'
  },

  {
    component: HistoryComponent,
    path: 'history'
  },
  {
    component: WaitingComponent,
    path: 'waiting'
  },
  {
    component: CurrentComponent,
    path: ''
  },
  {
    component: CompaniesComponent,
    path: 'company/list'
  },
  {
    component: EditCompanyComponent,
    path: 'company/edit/:companyid'
  },
  {
    component: RegistercompanyComponent,
    path: 'company/add'
  },
  {
    component: EditsiteComponent,
    path: 'site/edit/:siteid'
  },
  {
    component: SitesComponent,
    path: 'site/list'
  },
  {
    component: AddsiteComponent,
    path: 'site/add'
  },
  {
    component: AddemployeeComponent,
    path: 'employee/add'
  },
  {
    component: EditemployeeComponent,
    path: 'employee/edit/:employeeid'
  },
  {
    component: EmployeesComponent,
    path: 'employee/list'
  },
  {
    component: EditcategoryComponent,
    path: 'category/edit/:categoryid'
  },
  {
    component: CategoriesComponent,
    path: 'category/list'
  },
  {
    component: CurrentComponent,
    path: ''
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
    {provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}
