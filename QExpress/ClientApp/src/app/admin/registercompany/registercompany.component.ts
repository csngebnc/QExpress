
import { HttpClient } from '@angular/common/http';
import {Component, Input, OnInit, Output} from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {Subject, Subscription} from 'rxjs';
import { CompanyImage } from 'src/app/models/CompanyImage';
import {HttpService} from '../../http.service';
import { User } from '../../models/User';
import { mimeType } from './mime-type.validator';

@Component({
  selector: 'app-registercompany',
  templateUrl: './registercompany.component.html',
  styleUrls: ['./registercompany.component.css']
})
export class RegistercompanyComponent implements OnInit {

  private routeSub: Subscription;
  form: FormGroup;
  errors;

  company: CompanyImage = {
    id: 2,
    cegadminId: '',
    nev: '',
    image: {} as File
  }

  constructor(
    private activatedRoute: ActivatedRoute,
    private httpService: HttpService,
    private http: HttpClient,
    private router: Router,
    private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      name: ['', Validators.compose([
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20),
      ])],
      companyimage: new FormControl(null,
        [Validators.required],
        [mimeType]
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
  }

  submitPressed(data){
    
    this.errors = {
      'Nev': [],
      'email': [],
      'image': []
    }

    var valid = true;

    //Üres név
    if(data.name.trim() === ''){
      this.errors.Nev.push("Nem lehet üres a cég neve!");
      valid = false;
    }

    //Nem megfelelő hosszúságú név
    if(data.name.trim().length < 3 || data.name.trim().length > 20){
      this.errors.Nev.push("A cég nevének 3 és 20 karakter között kell lennie!");
      valid = false;
    }

    //E-mail formátum ellenőrzés
    if(!this.form.get("email").valid){
      this.errors.Nev.push("Rossz e-mail formátum!");
      valid = false;
    }

    //Üres logó
    if(!this.form.get('companyimage').touched){
      this.errors.image.push("Nincsen feltöltött cég logó!")
      valid = false;
    }

    //Valid kép
    else if (!this.form.get('companyimage').valid) {
      this.errors.image.push("A kiválaszott fájl nem kép!")
      valid = false;
    }

    if(valid)
      this.submitCompany(data)
  }

  onImagePicked(event: Event) {
    this.form.get('companyimage').markAsTouched();

    const file = (event.target as HTMLInputElement).files[0];
    this.form.patchValue({ companyimage: file });
    this.form.get('companyimage').updateValueAndValidity();
  }

  submitCompany(data) {
    this.httpService.getUserByEmail(data.email).subscribe(
      (user: User) => {
        const company_w_image = new FormData();
        company_w_image.append('CegadminId', user.id);
        company_w_image.append('Id', "0");
        company_w_image.append('Nev', data.name.trim());
        company_w_image.append('image', data.companyimage);
        this.httpService.addCompany(company_w_image).subscribe(
          (c: CompanyImage) => this.router.navigate(['/company/list']),
          (err) => this.errors = err.error
        );
      },
      (err) => this.errors = err.error
    )
  }
}
