import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompaniesComponent } from './companies/companies.component';
import { RegistercompanyComponent } from './registercompany/registercompany.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome'
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import {FormsModule} from '@angular/forms';
import { AddCompanyComponent } from './add-company/add-company.component';
import { EditCompanyComponent } from './edit-company/edit-company.component';



@NgModule({
  declarations: [CompaniesComponent, RegistercompanyComponent, AddCompanyComponent, EditCompanyComponent],
    imports: [
        CommonModule,
        FontAwesomeModule,
        FormsModule
    ]
})
export class AdminModule {
  constructor(library: FaIconLibrary){
    library.addIcons(faEdit, faTrashAlt);
  }
}
