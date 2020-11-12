import { Component, OnInit } from '@angular/core';
import { faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { Site } from 'src/app/models/Site';
import { User } from 'src/app/models/User';
import { UserSite } from 'src/app/models/UserSite';
import {HttpService} from '../../http.service';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css']
})
export class EmployeesComponent implements OnInit {

  faEdit = faEdit;
  faTrashAlt = faTrashAlt;

  public employees: UserSite[] = [];
  public employeeNames: String[] = [];
  public employeeEmails: String[] = [];
  public employeeSites: String[] = [];

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
    this.loadEmployees();
  }

  loadEmployees(){
    this.httpService.getEmployees().subscribe( (employees: UserSite[]) =>{
      employees.forEach(e => {
        this.employees.push(e);
        this.httpService.getUserById(e.felhasznaloId).subscribe((u: User) => {
          this.employeeNames.push(u.userName);
          this.employeeEmails.push(u.email);
        })
        this.httpService.getSiteById(e.telephelyId).subscribe((s: Site) => {
          this.employeeSites.push(s.cim);
        })
      });
    })
  }

  deleteEmployee(id: String){
    this.httpService.deleteEmployee(id).subscribe(()=>{
      this.loadEmployees();
    })
  }
}
