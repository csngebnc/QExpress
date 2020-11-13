import { Component, OnInit } from '@angular/core';
import { faBullhorn, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { HttpService } from 'src/app/http.service';
import { Queue } from 'src/app/models/Queue';

@Component({
  selector: 'app-waiting',
  templateUrl: './waiting.component.html',
  styleUrls: ['./waiting.component.css']
})
export class WaitingComponent implements OnInit {

  queue: Queue[];

  constructor(private httpService: HttpService) { }

  loadWaiting(){
    this.httpService.getWaiting().subscribe((queue: Queue[]) => {
      this.queue = queue;
    })
  }

  ngOnInit() {
    this.loadWaiting();
  }

  deleteQueue(id: Number){
    this.httpService.deleteQueue(id).subscribe(() => {});
    this.loadWaiting();
  }

  setQueueCalled(id: Number){
    this.httpService.setQueueCalled(id).subscribe((q: Queue) => {
      this.loadWaiting();
    })
  }

  faTrashAlt = faTrashAlt;
  bullhorn = faBullhorn;

}
