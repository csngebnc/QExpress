import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/app/http.service';
import { Queue } from 'src/app/models/Queue';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {

  history: Queue[] = [];

  constructor(private httpService: HttpService) { }

  ngOnInit() {
    this.loadHistory();
  }

  loadHistory(){
    this.httpService.getQueueHistory().subscribe((qh: Queue[]) => {
      this.history = qh;
    })
  }

}
