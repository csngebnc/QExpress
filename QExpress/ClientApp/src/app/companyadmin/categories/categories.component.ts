import {Component, OnInit} from '@angular/core';
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {HttpService} from '../../http.service';
import {Category} from '../../models/Category';

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
    this.loadCategories();
  }

  loadCategories(): void {
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
