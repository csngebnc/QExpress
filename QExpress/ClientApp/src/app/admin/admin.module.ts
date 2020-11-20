import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompaniesComponent } from './companies/companies.component';
import { RegistercompanyComponent } from './registercompany/registercompany.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome'
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { EditCompanyComponent } from './edit-company/edit-company.component';
import {RouterModule} from "@angular/router";


@NgModule({
  declarations: [CompaniesComponent, RegistercompanyComponent, EditCompanyComponent],
    imports: [
        CommonModule,
        FontAwesomeModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
    ]
})
export class AdminModule {
  constructor(library: FaIconLibrary){
    library.addIcons(faEdit, faTrashAlt);
  }
}
