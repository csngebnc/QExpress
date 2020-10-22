import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { CompaniesComponent } from './admin/companies/companies.component';
import { RegistercompanyComponent } from './admin/registercompany/registercompany.component';
import { CategoriesComponent } from './companyadmin/categories/categories.component';
import { EmployeesComponent } from './companyadmin/employees/employees.component';
import { EditcategoryComponent } from './companyadmin/editcategory/editcategory.component'
import { EditemployeeComponent } from './companyadmin/editemployee/editemployee.component'
import { EditsiteComponent } from './companyadmin/editsite/editsite.component'
import { SitesComponent } from './companyadmin/sites/sites.component';
import { WaitingComponent } from './employee/waiting/waiting.component';
import { CurrentComponent } from './queue/current/current.component';
import { HistoryComponent } from './queue/history/history.component';
import { EditUserComponent } from './user/edit-user/edit-user.component';
import { MatDialogModule } from '@angular/material/dialog'
import { NewDialog } from './queue/current/current.component'

var routes = [
  {
    component:CurrentComponent,
    path: ''
  },
  {
    component:EditUserComponent,
    path: 'profile'
  },
  
  {
    component:HistoryComponent,
    path: 'history'
  },
  {
    component:WaitingComponent,
    path: 'waiting'
  },
  {
    component:CurrentComponent,
    path: ''
  },
  {
    component: CompaniesComponent,
    path: 'company/list'
  },
  {
    component: RegistercompanyComponent,
    path: 'company/register'
  },
  {
    component: CompaniesComponent,
    path: 'company/list'
  },
  {
    component: EditsiteComponent,
    path: 'site/edit'
  },
  {
    component:SitesComponent,
    path: 'site/list'
  },
  {
    component:EditemployeeComponent,
    path: 'employee/edit'
  },
  {
    component:EmployeesComponent,
    path: 'employee/list'
  },
  {
    component:EditcategoryComponent,
    path: 'category/edit'
  },
  {
    component:CategoriesComponent,
    path: 'category/list'
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
    CompaniesComponent,
    WaitingComponent,
    NewDialog,
  ],
  entryComponents: [
    NewDialog,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot(routes),
    MatDialogModule,
  ],
  exports:[
    MatDialogModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
