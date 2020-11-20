import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router'
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
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddemployeeComponent } from './addemployee/addemployee.component';
import {AddCategoryComponent} from "./add-category/add-category.component";

@NgModule({
  declarations: [
    CategoriesComponent,
    EmployeesComponent,
    EditcategoryComponent,
    EditsiteComponent,
    EditemployeeComponent,
    SitesComponent,
    AddsiteComponent, AddemployeeComponent, AddCategoryComponent],
  imports: [
    CommonModule,
    FontAwesomeModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
  ]
})
export class CompanyadminModule {
  constructor(library: FaIconLibrary){
    library.addIcons(faEdit, faTrashAlt);
  }
}
