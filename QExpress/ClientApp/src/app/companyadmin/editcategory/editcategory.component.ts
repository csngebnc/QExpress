import {Component, OnInit} from '@angular/core';
import {HttpService} from '../../http.service';
import {Company} from '../../models/Company';
import {Category} from '../../models/Category';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder} from "@angular/forms";

@Component({
  selector: 'app-editcategory',
  templateUrl: './editcategory.component.html',
  styleUrls: ['./editcategory.component.css']
})
export class EditcategoryComponent implements OnInit {

  // public category: Category = {
  //   nev: 'Example name',
  // };

  // category: Category = {
  //   megnevezes: '',
  //   Id: '',
  //   CegId: ''
  // };
  category: Category;
  editCategoryForm;

  // category: Category = {
  //   megnevezes: 'meglévő',
  //   id: 2,
  //   cegId: 6
  // };

  constructor(
    private httpService: HttpService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder) {
    // this.editCategoryForm = this.formBuilder.group( {
    //   megnevezes: '1'
    // })
  }

  ngOnInit() {
    const categoryId = this.activatedRoute.snapshot.paramMap.get('categoryid');
    this.httpService.getCategory(categoryId).subscribe(
      category => {
        this.category = category
        this.editCategoryForm.get('megnevezes').setValue(category.megnevezes)
        this.editCategoryForm.get('id').setValue(category.id)
      }
    );
    this.editCategoryForm = this.formBuilder.group({
      megnevezes: '',
      id: null
    })
  }

  onSubmit() {

    this.httpService.editCategorName(this.editCategoryForm.value).subscribe((c: Category) => {
      this.router.navigate(['category/list'])
    })
  }

  // ngOnInit() {
  //   // const categoryId = this.activatedRoute.snapshot.paramMap.get('categoryid');
  //   // mivel number az id
  //   const categoryId = Number.parseInt(this.activatedRoute.snapshot.paramMap.get('categoryid'), 10);
  //   console.log(categoryId);
  //   this.httpService.getCategory(categoryId).subscribe(
  //     category => this.category = category
  //   );
  // }

  //  public submitCategory(): void {
  //   this.httpService.editCategorName(this.category).subscribe(
  //     res => console.log(res)
  //   );
  // }

  // {"uj_megnevezes":"tesztKategUj"}

}
