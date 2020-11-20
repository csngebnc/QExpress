import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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

  form: FormGroup;
  errors;

  constructor(
    public httpService: HttpService,
    public activatedRoute: ActivatedRoute,
    public router: Router,
    private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      siteId: -1
    })
  }

  ngOnInit() {
    this.loadEmployee();
    this.loadSites();
  }

  loadEmployee() {
    this.httpService.getEmployeeById(this.activatedRoute.snapshot.paramMap.get('employeeid')).subscribe((usite: UserSite) => {
      this.userSite = usite;
      this.form.get("siteId").setValue(this.userSite.telephelyId)
    })
  }

  loadSites() {
    this.httpService.getOwnSites().subscribe((sites: Site[]) => {
      this.sites = sites;
    })
  }

  submitPressed(data) {
    this.editEmployee(data)
  }

  editEmployee(data) {
    this.userSite.telephelyId = data.siteId
    this.httpService.editEmployee(this.userSite).subscribe(
      (us: UserSite) => this.router.navigate(['/employee/list']),
      (err) => this.errors = err.error
    )
  }
}
