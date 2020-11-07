import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompaniesComponent } from './companies/companies.component';
import { RegistercompanyComponent } from './registercompany/registercompany.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome'
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';



@NgModule({
  declarations: [CompaniesComponent, RegistercompanyComponent],
  imports: [
    CommonModule,
    FontAwesomeModule
  ]
})
export class AdminModule { 
  constructor(library: FaIconLibrary){
    library.addIcons(faEdit, faTrashAlt);
  }
}