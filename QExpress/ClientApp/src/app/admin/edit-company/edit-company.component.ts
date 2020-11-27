import { Component, OnInit } from '@angular/core';
import { Company } from '../../models/Company';
import { User } from '../../models/User';
import { HttpService } from '../../http.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AbstractControl } from '@angular/forms'
import { mimeType } from '../registercompany/mime-type.validator';

@Component({
  selector: 'app-edit-company',
  templateUrl: './edit-company.component.html',
  styleUrls: ['./edit-company.component.css'],
})
export class EditCompanyComponent implements OnInit {

  company: Company;
  email: String;
  form: FormGroup;
  errors;

  constructor(
    private activatedRoute: ActivatedRoute,
    private httpService: HttpService,
    private router: Router,
    private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      name: ['', Validators.compose([
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(20)
      ])],
      companyimage: new FormControl(null,
        [mimeType],
      ),
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
        this.form.get("name").setValue(company.nev)
        this.httpService.getUserById(this.company.cegadminId).subscribe((u: User) => {
          this.form.get("email").setValue(u.email)
        })
      }
    );
  }

  submitPressed(data) {

    this.errors = {
      'Nev': [],
      'email': []
    }

    var valid = true;

    //Üres név
    if (data.name.trim() === '') {
      this.errors.Nev.push("Nem lehet üres a név!");
      valid = false;
    }
 
    //Nem megfelelő hosszúságú név
    if (data.name.trim().length < 6 || data.name.trim().length > 20) {
      this.errors.Nev.push("Nem megfelelő hosszúságú név!");
      valid = false;
    }

    //E-mail formátum ellenőrzés
    if (!this.form.get("email").valid) {
      this.errors.Nev.push("Rossz e-mail formátum!");
      valid = false;
    }

    //Valid kép
    if (this.form.get('companyimage') && !this.form.get('companyimage').valid) {
      this.errors.image.push("A kiválaszott fájl nem kép!")
      valid = false;
    }

    if (valid)
      this.onSubmit(data)
  }

  onImagePicked(event: Event) {
    this.form.get('companyimage').markAsTouched();

    const file = (event.target as HTMLInputElement).files[0];
    this.form.patchValue({ companyimage: file });
    this.form.get('companyimage').updateValueAndValidity();
  }

  onSubmit(companyData) {
    this.httpService.getUserByEmail(companyData.email).subscribe((user: User) => {
      const company_w_image = new FormData();
      company_w_image.append('Id', this.activatedRoute.snapshot.paramMap.get('companyid'));
      company_w_image.append('CegadminId', user.id);
      company_w_image.append('Nev', companyData.name.trim());
      company_w_image.append('image', companyData.companyimage);
      this.company.nev = companyData.name.trim();
      this.company.cegadminId = user.id;
      this.httpService.editCompany(company_w_image).subscribe(
        (c: Company) => this.router.navigate(['company/list']),
        (err) => this.errors = err.error
      );
    },
      (err) => this.errors = err.error
    )
  }
}
