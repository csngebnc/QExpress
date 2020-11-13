import {Component, OnInit} from '@angular/core';
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {HttpService} from '../../http.service';
import {Category} from '../../models/Category';
import {Company} from '../../models/Company';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {

  faEdit = faEdit;
  faTrashAlt = faTrashAlt;

  public categories: Category[] = [];

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
    // console.log('Init categories');
    /* this.getCategories(); */
    this.loadCategories();
  }

  /* private getCategories(): void {
    console.log('Get categories');

    this.httpService.getCategories().subscribe(
      categories => this.handleCategoriesResponse(categories),
      // error => console.log(error)
    );
  }

  private handleCategoriesResponse(categories): void {
    console.log(categories);
    this.categories = categories;
  }

  /*
  private exampleCompanyCreate(): void {
    this.httpService.addCompany({
      id: 0,
      nev: 'string',
      CegId: 'string'
    }).subscribe(res => console.log(res));
  }
  */

  loadCategories(): void {
    // this.httpService.getOwnCategories().subscribe((categories: Category[]) => {
    //   this.categories = categories;
    //   console.log(categories);
    // });
    this.httpService.getCategoriesAll().subscribe((categories: Category[]) => {
      this.categories = categories;
      console.log(categories);
    });
  }

  deleteCategory(id: number): void {
    this.httpService.deleteCategory(id).subscribe(
      () => this.loadCategories()
    );
  }
}
