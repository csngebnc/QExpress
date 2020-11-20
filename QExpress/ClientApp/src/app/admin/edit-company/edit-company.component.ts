import {Component, OnInit} from '@angular/core';
import {Company} from '../../models/Company';
import { User } from '../../models/User';
import {HttpService} from '../../http.service';
import {ActivatedRoute, Router} from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AbstractControl } from '@angular/forms'

@Component({
  selector: 'app-edit-company',
  templateUrl: './edit-company.component.html',
  styleUrls: ['./edit-company.component.css'],
})
export class EditCompanyComponent implements OnInit {

  company: Company;
  email: String;
  editCompanyForm: FormGroup;
  errors;

  constructor(
    private activatedRoute: ActivatedRoute,
    private httpService: HttpService,
    private router: Router,
    private formBuilder: FormBuilder) {
      this.editCompanyForm = this.formBuilder.group({
        name: ['', Validators.compose([
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(20)
        ])],
        email: ['', Validators.compose([
          Validators.required,
          Validators.email,
          Validators.minLength(6),
          Validators.maxLength(20)
        ])]
      })
    }

  ngOnInit() {
    const companyId = this.activatedRoute.snapshot.paramMap.get('companyid');
    this.httpService.getCompany(companyId).subscribe(
      company => {
        this.company = company
        this.editCompanyForm.get("name").setValue(company.nev)
        this.httpService.getUserById(this.company.cegadminId).subscribe((u: User) =>{
          this.editCompanyForm.get("email").setValue(u.email)
        })
      }
    );
  }

  onSubmit(companyData) {
    this.httpService.getUserByEmail(companyData.email).subscribe((user: User) => {
      this.company.nev = companyData.name;
      this.company.cegadminId = user.id;
      this.httpService.editCompany(this.company).subscribe(
        (c: Company) => this.router.navigate(['company/list']),
        (err) => this.errors = err);
    }, 
    (err) => this.handleErrors(err.error))
  }

  handleErrors(errors){
    this.errors = errors
  }

}
