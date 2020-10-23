import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WaitingComponent } from './waiting/waiting.component';

import { FaIconLibrary, FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';

@NgModule({
  declarations: [WaitingComponent],
  imports: [
    CommonModule,
    FontAwesomeModule,
  ]
})
export class EmployeeModule {
  constructor(library: FaIconLibrary){
    library.addIcons(faEdit, faTrashAlt);
  }
 }
