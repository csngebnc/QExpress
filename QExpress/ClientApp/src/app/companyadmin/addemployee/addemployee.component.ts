import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpService } from 'src/app/http.service';
import { Site } from 'src/app/models/Site';
import { User } from 'src/app/models/User';
import { UserSite } from 'src/app/models/UserSite';

@Component({
  selector: 'app-addemployee',
  templateUrl: './addemployee.component.html',
  styleUrls: ['./addemployee.component.css']
})
export class AddemployeeComponent implements OnInit {

  sites: Site[] = [];
  userSite: UserSite = {
    felhasznaloId: '',
    telephelyId: -1
  }
  email: String;
  siteId: Number;
  
  addEmployeeForm;

  constructor(
    public httpService: HttpService,
    public router: Router,
    private formBuilder: FormBuilder) {
      this.addEmployeeForm = this.formBuilder.group({
        email: '',
        siteId: -1
      })
    }

  ngOnInit() {
    this.loadSites();
  }

  loadSites(){
    this.httpService.getSites().subscribe((sites: Site[]) => {
      this.sites = sites;
    })
  }

  addEmployee(employeeData){
    this.httpService.getUserByEmail(employeeData.email).subscribe((u: User) => {
      this.userSite.felhasznaloId = u.id;
      this.userSite.telephelyId = employeeData.siteId;
      this.httpService.addEmployee(this.userSite).subscribe((us: UserSite) => {
        this.router.navigate(['/employee/list']);
      })
    })
  }

}
