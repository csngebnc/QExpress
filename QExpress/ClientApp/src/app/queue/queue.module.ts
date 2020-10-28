import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HistoryComponent } from './history/history.component';
import { CurrentComponent, NewDialog } from './current/current.component';
import { MatDialogModule } from '@angular/material';



@NgModule({
  declarations: [
    HistoryComponent, 
    CurrentComponent,
    NewDialog,
  ],

  entryComponents: [
    NewDialog,
  ],

  imports: [
    CommonModule,
    MatDialogModule,
  ],

  bootstrap: [
    CurrentComponent,
  ]
})
export class QueueModule { }
