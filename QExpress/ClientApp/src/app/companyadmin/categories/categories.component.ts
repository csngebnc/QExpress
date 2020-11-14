import {Component, OnInit} from '@angular/core';
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {HttpService} from '../../http.service';
import {Category} from '../../models/Category';
import {User} from "../../models/User";
import {Company} from "../../models/Company";

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {

  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  user: User;
  company: Company;
  public categories: Category[] = [];

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
    this.getCurrentUser();
  }

  loadCategories(): void {
    this.httpService.getCategoriesForCompany(this.company.id).subscribe((categories: Category[]) => {
      this.categories = categories;
    });
  }

  getOwnCompany(): void {
    this.httpService.getOwnCompany(this.user.id).subscribe(
      company => this.handleCompanyResponse(company)
    )
  }

  handleCompanyResponse(company: Company): void {
    this.company = company;
    this.loadCategories();
  }

  deleteCategory(id: number): void {
    this.httpService.deleteCategory(id).subscribe(
      () => this.loadCategories()
    );
  }

  getCurrentUser(): void {
    this.httpService.getCurrentUser().subscribe(
      user => this.handleUserResponse(user)
    )
  }

  handleUserResponse(user): void {
    this.user = user;
    this.getOwnCompany();
  }
}
