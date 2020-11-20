import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../http.service';
import { Category } from '../../models/Category';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";


@Component({
  selector: 'app-editcategory',
  templateUrl: './editcategory.component.html',
  styleUrls: ['./editcategory.component.css']
})
export class EditcategoryComponent implements OnInit {

  form: FormGroup;

  errors;

  category: Category = {
    megnevezes: '',
    id: -1,
    cegId: -1
  };

  constructor(
    private httpService: HttpService,
    private activatedRoute: ActivatedRoute,
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
    const categoryId = this.activatedRoute.snapshot.paramMap.get('categoryid');
    this.httpService.getCategory(categoryId).subscribe(
      category => {
        this.category = category
        this.form.get('megnevezes').setValue(category.megnevezes)
      }
    );
  }

  submitPressed(data) {

    this.errors = {
      'megnevezes': []
    }

    var valid = true;

    //Üres név
    if (data.megnevezes.trim() === '') {
      this.errors.Nev.push("Nem lehet üres a megnevezés!");
      valid = false;
    }

    //Nem megfelelő hosszúságú név
    if (data.megnevezes.trim().length < 10 || data.megnevezes.trim().length > 50) {
      this.errors.Nev.push("A megnevezésnek 10 és 50 karakter között kell lennie!");
      valid = false;
    }

    if (valid)
      this.submitCategory(data)
  }

  submitCategory(data) {
    this.category.megnevezes = data.megnevezes
    this.httpService.editCategoryName(this.category).subscribe(
      (c: Category) => this.router.navigate(['category/list']),
      (err) => this.errors = err.error
    )
  }
}
