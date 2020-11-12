import { Component, OnInit } from '@angular/core';
import { faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { User } from 'src/app/models/User';
import {HttpService} from '../../http.service';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css']
})
export class EmployeesComponent implements OnInit {

  faEdit = faEdit;
  faTrashAlt = faTrashAlt;

  public alkalmazottak: User[] = [];

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
    
  }
}
