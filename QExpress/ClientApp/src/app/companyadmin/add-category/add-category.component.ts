import { Component, OnInit } from '@angular/core';
import { Category } from "../../models/Category";
import { HttpService } from "../../http.service";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { Company } from "../../models/Company";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent implements OnInit {

  form: FormGroup
  errors;

  category: Category = {
    megnevezes: '',
    id: -1,
    cegId: -1
  };

  constructor(
    private httpService: HttpService,
    private router: Router,
    private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      megnevezes: [
        '',
        Validators.compose([
          Validators.required,
          Validators.minLength(10),
          Validators.maxLength(50)
        ])
      ]
    })
  }

  ngOnInit() {
  }

  submitPressed(data) {

    this.errors = {
      'megnevezes': []
    }

    console.log(data)

    var valid = true;

    //Üres név
    if (data.megnevezes.trim() === '') {
      this.errors.megnevezes.push("Nem lehet üres a megnevezés!");
      valid = false;
    }

    //Nem megfelelő hosszúságú név
    if (data.megnevezes.trim().length < 10 || data.megnevezes.trim().length > 50) {
      this.errors.megnevezes.push("A megnevezésnek 10 és 50 karakter között kell lennie!");
      valid = false;
    }

    if (valid)
      this.submitCategory(data)
  }

  submitCategory(data) {
    this.category.megnevezes = data.megnevezes
    this.httpService.addCategory(this.category).subscribe(
      (c: Category) => this.router.navigate(['category/list']),
      (err) => this.errors = err.error
    )
  }
}
