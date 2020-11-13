import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from 'src/app/http.service';
import { Site } from 'src/app/models/Site';
import { UserSite } from 'src/app/models/UserSite';

@Component({
  selector: 'app-editemployee',
  templateUrl: './editemployee.component.html',
  styleUrls: ['./editemployee.component.css']
})
export class EditemployeeComponent implements OnInit {

  sites: Site[] = [];
  userSite: UserSite = {
    felhasznaloId: '',
    telephelyId: -1
  }
  email: String;
  siteId: Number;
  
  editEmployeeForm;

  constructor(
    private activatedRoute: ActivatedRoute,
    public httpService: HttpService,
    public router: Router,
    private formBuilder: FormBuilder) {
      this.editEmployeeForm = this.formBuilder.group({
        siteId: -1
      })
    }

  ngOnInit() {
    this.loadEmployee();
    this.loadSites();
  }

  loadEmployee(){
    this.httpService.getEmployeeById(this.activatedRoute.snapshot.paramMap.get('employeeid')).subscribe((usite: UserSite) => {
      this.userSite = usite;
    })
    console.log(this.userSite);
  }

  loadSites(){
    this.httpService.getOwnSites().subscribe((sites: Site[]) => {
      this.sites = sites;
    })
  }

  editEmployee(employeeData){
    this.userSite.telephelyId = employeeData.siteId
    this.httpService.editEmployee(this.userSite).subscribe(() => {
      this.router.navigate(['/employee/list']);
    })
  }

}
