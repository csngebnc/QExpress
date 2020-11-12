import {Component, OnInit} from '@angular/core';
import {Company} from '../../models/Company';
import { User } from '../../models/User';
import {HttpService} from '../../http.service';
import {ActivatedRoute, Router} from '@angular/router';
import { FormBuilder } from '@angular/forms'

@Component({
  selector: 'app-edit-company',
  templateUrl: './edit-company.component.html',
  styleUrls: ['./edit-company.component.css'],
})
export class EditCompanyComponent implements OnInit {

  company: Company;
  email: String;
  editCompanyForm;

  constructor(
    private activatedRoute: ActivatedRoute,
    private httpService: HttpService,
    private router: Router,
    private formBuilder: FormBuilder) {
      this.editCompanyForm = this.formBuilder.group({
        name: '1',
        email: ''
      })
  }

  ngOnInit() {
    const companyId = this.activatedRoute.snapshot.paramMap.get('companyid');
    this.httpService.getCompany(companyId).subscribe(
      company => this.company = company
    );
    this.httpService.getUserById(this.company.cegadminId).subscribe((u: User) =>{
      this.email = u.email;
    })
    this.editCompanyForm = this.formBuilder.group({
      name: '2',
      email: ''
    })
  }

  onSubmit(companyData) {
    this.httpService.getUserByEmail(companyData.email).subscribe((user: User) => {
      this.company.nev = companyData.name;
      this.company.cegadminId = user.id;
      this.httpService.editCompany(this.company).subscribe((c: Company) => {
        this.router.navigate(['/company/list'])
      });
    })
  }
}
