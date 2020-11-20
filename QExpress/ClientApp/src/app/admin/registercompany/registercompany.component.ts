
import {Component, Input, OnInit, Output} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {Subject, Subscription} from 'rxjs';
import { Company } from 'src/app/models/Company';
import {HttpService} from '../../http.service';
import { User } from '../../models/User';

@Component({
  selector: 'app-registercompany',
  templateUrl: './registercompany.component.html',
  styleUrls: ['./registercompany.component.css']
})
export class RegistercompanyComponent implements OnInit {

  private routeSub: Subscription;
  form: FormGroup;
  errors;

  company: Company = {
    id: 2,
    cegadminId: '',
    nev: ''
  }

  constructor(
    private activatedRoute: ActivatedRoute,
    private httpService: HttpService,
    private router: Router,
    private formBuilder: FormBuilder) {
      this.form = this.formBuilder.group({
        name: ['', Validators.compose([
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(20),
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
  }

  submitPressed(data){
    
    this.errors = {
      'Nev': [],
      'email': []
    }

    var valid = true;

    //Üres név
    if(data.name.trim() === ''){
      this.errors.Nev.push("Nem lehet üres a cég neve!");
      valid = false;
    }

    //Nem megfelelő hosszúságú név
    if(data.name.trim().length < 6 || data.name.trim().length > 20){
      this.errors.Nev.push("A cég nevének 6 és 20 karakter között kell lennie!");
      valid = false;
    }

    //E-mail formátum ellenőrzés
    if(!this.form.get("email").valid){
      this.errors.Nev.push("Rossz e-mail formátum!");
      valid = false;
    }

    if(valid)
      this.submitCompany(data)
  }

  submitCompany(data) {
    this.httpService.getUserByEmail(data.email).subscribe(
      (user: User) => {
        this.company.cegadminId = user.id;
        this.company.id = 0;
        this.company.nev = data.name.trim()
        this.httpService.addCompany(this.company).subscribe(
          (c: Company) => this.router.navigate(['/company/list']),
          (err) => this.errors = err.error
        );
      }, 
      (err) => this.errors = err.error
    )
  }
}
