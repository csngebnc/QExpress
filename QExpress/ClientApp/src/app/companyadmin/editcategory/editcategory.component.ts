import {Component, OnInit} from '@angular/core';
import {HttpService} from '../../http.service';
import {Category} from '../../models/Category';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder} from "@angular/forms";
import {User} from "../../models/User";
import {Company} from "../../models/Company";

@Component({
  selector: 'app-editcategory',
  templateUrl: './editcategory.component.html',
  styleUrls: ['./editcategory.component.css']
})
export class EditcategoryComponent implements OnInit {

  category: Category;
  editCategoryForm;

  constructor(
    private httpService: HttpService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder) {
  }

  ngOnInit() {
    const categoryId = this.activatedRoute.snapshot.paramMap.get('categoryid');
    this.httpService.getCategory(categoryId).subscribe(
      category => {
        this.category = category
        this.editCategoryForm.get('megnevezes').setValue(category.megnevezes)
        this.editCategoryForm.get('id').setValue(category.id)
        this.editCategoryForm.get('cegId').setValue(category.cegId)
      }
    );
    this.editCategoryForm = this.formBuilder.group({
      megnevezes: '',
      id: null,
      cegId: null
    })
  }

  onSubmit() {

    this.httpService.editCategorName(this.editCategoryForm.value).subscribe((c: Category) => {
      this.router.navigate(['category/list'])
    })
  }




}
