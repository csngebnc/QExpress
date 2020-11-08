import {Component, OnInit} from '@angular/core';
import {HttpService} from '../../http.service';
import {Company} from '../../models/Company';
import {Category} from '../../models/Category';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-editcategory',
  templateUrl: './editcategory.component.html',
  styleUrls: ['./editcategory.component.css']
})
export class EditcategoryComponent implements OnInit {

  // public category: Category = {
  //   nev: 'Example name',
  // };

  category: Category = {
    megnevezes: 'meglévő',
    id: 2,
    cegId: 6
  };

  constructor(private httpService: HttpService, private activatedRoute: ActivatedRoute) {
  }

  ngOnInit() {
    // const categoryId = this.activatedRoute.snapshot.paramMap.get('categoryid');
    // mivel number az id
    const categoryId = Number.parseInt(this.activatedRoute.snapshot.paramMap.get('categoryid'), 10);
    console.log(categoryId);
    this.httpService.getCategory(categoryId).subscribe(
      category => this.category = category
    );
  }

   public submitCategory(): void {
    this.httpService.editCategorName(this.category).subscribe(
      res => console.log(res)
    );
  }

}
