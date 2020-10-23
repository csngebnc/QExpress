import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HistoryComponent } from './history/history.component';
import { CurrentComponent, NewDialog } from './current/current.component';
import { MatDialogModule } from '@angular/material/dialog';



@NgModule({
  declarations: [HistoryComponent, CurrentComponent,NewDialog,],
  entryComponents: [NewDialog,],
  exports: [MatDialogModule],
  imports: [
    CommonModule,
    MatDialogModule
  ]
})
export class QueueModule { }
