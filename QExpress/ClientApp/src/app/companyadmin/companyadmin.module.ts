import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoriesComponent } from './categories/categories.component';
import { EmployeesComponent } from './employees/employees.component';
import { EditcategoryComponent } from './editcategory/editcategory.component';
import { EditsiteComponent } from './editsite/editsite.component';
import { EditemployeeComponent } from './editemployee/editemployee.component';
import { SitesComponent } from './sites/sites.component';



@NgModule({
  declarations: [CategoriesComponent, EmployeesComponent, EditcategoryComponent, EditsiteComponent, EditemployeeComponent, SitesComponent],
  imports: [
    CommonModule
  ]
})
export class CompanyadminModule { }
