import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HistoryComponent } from './history/history.component';
import { CurrentComponent } from './current/current.component';



@NgModule({
  declarations: [HistoryComponent, CurrentComponent],
  imports: [
    CommonModule
  ]
})
export class QueueModule { }
