import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from 'src/app/http.service';
import { Site } from 'src/app/models/Site';

@Component({
  selector: 'app-editsite',
  templateUrl: './editsite.component.html',
  styleUrls: ['./editsite.component.css']
})
export class EditsiteComponent implements OnInit {

  site: Site;
  form: FormGroup;
  errors;
  
  constructor(
    private activatedRoute: ActivatedRoute,
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
    const siteId= this.activatedRoute.snapshot.paramMap.get('siteid')
    this.httpService.getSiteById(parseInt(siteId)).subscribe(
      (site: Site) => {
        this.site = site;
        this.form.get("address").setValue(site.cim)
      })
  }

  submitPressed(data){
    
    this.errors = {
      'address': []
    }

    var valid = true;

    //Üres név
    if(data.address.trim() === ''){
      this.errors.address.push("Nem lehet üres a megnevezés!");
      valid = false;
    }

    //Nem megfelelő hosszúságú név
    if(data.address.trim().length < 3 || data.address.trim().length > 50){
      this.errors.address.push("A cimnek 3 és 50 karakter között kell lennie!");
      valid = false;
    }

    if(valid)
      this.editSite(data)
  }

  editSite(data){
    this.site.cim = data.address;
    this.httpService.editSite(this.site).subscribe(
      (site: Site) => this.router.navigate(['/site/list']),
      (err) => this.errors = err.error
    )
  }
}
