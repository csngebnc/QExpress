import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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

  form: FormGroup;
  errors;

  constructor(
    public httpService: HttpService,
    public router: Router,
    private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      email: ['', Validators.compose([
        Validators.required,
        Validators.email,
        Validators.minLength(6),
        Validators.maxLength(20)
      ])],
      siteId: -1
    })
  }

  ngOnInit() {
    this.loadSites();
  }

  loadSites() {
    this.httpService.getOwnSites().subscribe((sites: Site[]) => {
      this.sites = sites;
    })
  }

  submitPressed(data) {

    this.errors = {
      'email': []
    }

    var valid = true;

    //Üres név
    if (data.email.trim() === '') {
      this.errors.email.push("Nem lehet üres az E-mail cim mező");
      valid = false;
    }

    //E-mail formátum ellenőrzés
    if (!this.form.get("email").valid) {
      this.errors.email.push("Rossz e-mail formátum!");
      valid = false;
    }

    if (valid)
      this.addEmployee(data)
  }

  addEmployee(data) {
    this.httpService.getUserByEmail(data.email).subscribe((u: User) => {
      this.userSite.felhasznaloId = u.id;
      this.userSite.telephelyId = data.siteId;
      this.httpService.addEmployee(this.userSite).subscribe(
        (us: UserSite) => this.router.navigate(['/employee/list']),
        (err) => this.errors = err.error
      )
    },
      (err) => this.errors = err.error
    )
  }
}
