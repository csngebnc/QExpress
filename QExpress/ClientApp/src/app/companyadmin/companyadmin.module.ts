import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoriesComponent } from './categories/categories.component';
import { EmployeesComponent } from './employees/employees.component';
import { EditcategoryComponent } from './editcategory/editcategory.component';
import { EditsiteComponent } from './editsite/editsite.component';
import { EditemployeeComponent } from './editemployee/editemployee.component';
import { SitesComponent } from './sites/sites.component';
import { FaIconLibrary, FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { AddsiteComponent } from './addsite/addsite.component';
import { FormsModule } from '@angular/forms'

@NgModule({
  declarations: [
    CategoriesComponent, 
    EmployeesComponent, 
    EditcategoryComponent, 
    EditsiteComponent, 
    EditemployeeComponent, 
    SitesComponent, 
    AddsiteComponent],
  imports: [
    CommonModule,
    FontAwesomeModule,
    FormsModule
  ]
})
export class CompanyadminModule { 
  constructor(library: FaIconLibrary){
    library.addIcons(faEdit, faTrashAlt);
  }
}
