import {Component, OnInit} from '@angular/core';
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {HttpService} from '../../http.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {

  faEdit = faEdit;
  faTrashAlt = faTrashAlt;

  public categories: any[] = [];

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
    console.log('Init categories');
    this.getCategories();
  }

  private getCategories(): void {
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

}
