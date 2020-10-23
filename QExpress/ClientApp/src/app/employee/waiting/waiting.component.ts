import { Component, OnInit } from '@angular/core';
import { faBullhorn, faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-waiting',
  templateUrl: './waiting.component.html',
  styleUrls: ['./waiting.component.css']
})
export class WaitingComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  bullhorn = faBullhorn;

}
