import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpService } from 'src/app/http.service';
import { Company } from 'src/app/models/Company';
import { Site } from 'src/app/models/Site';

@Component({
  selector: 'app-addsite',
  templateUrl: './addsite.component.html',
  styleUrls: ['./addsite.component.css']
})
export class AddsiteComponent implements OnInit {

  form: FormGroup;
  errors;

  site: Site = {
    id: -1,
    cim: '',
    companyid: -1
  }

  constructor(
    private httpService: HttpService,
    private router: Router,
    private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      address: [
        '',
        Validators.compose([
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(50)
        ])
      ]
    })
  }

  ngOnInit() {
  }

  submitPressed(data) {

    this.errors = {
      'address': []
    }

    var valid = true;

    //Üres név
    if (data.address.trim() === '') {
      this.errors.address.push("Nem lehet üres a megnevezés!");
      console.log(data)
      valid = false;
    }

    //Nem megfelelő hosszúságú név
    if (data.address.trim().length < 3 || data.address.trim().length > 50) {
      this.errors.address.push("A cimnek 3 és 50 karakter között kell lennie!");
      valid = false;
    }

    if (valid)
      this.submitSite(data)
  }

  submitSite(data) {
    this.site.cim = data.address
    this.httpService.addSite(this.site).subscribe(
      (s: Site) => this.router.navigate(['/site/list']),
      (err) => this.errors = err.error
    )
  }
}
