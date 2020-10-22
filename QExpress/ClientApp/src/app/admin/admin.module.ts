import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompaniesComponent } from './companies/companies.component';
import { RegistercompanyComponent } from './registercompany/registercompany.component';



@NgModule({
  declarations: [CompaniesComponent, RegistercompanyComponent],
  imports: [
    CommonModule
  ]
})
export class AdminModule { }
