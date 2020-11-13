import {Component, Input, OnInit} from '@angular/core';
import {Category} from "../../models/Category";
import {HttpService} from "../../http.service";
import {ActivatedRoute, Router} from "@angular/router";
import {FormBuilder} from "@angular/forms";
import {Subscription} from "rxjs";
import {User} from "../../models/User";
import {Company} from "../../models/Company";

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent implements OnInit {

  private routeSub: Subscription;

  companies: Company[] = [];

  category: Category = {
    megnevezes: '',
    id: 22,
    cegId: null
  };

  constructor(
    private httpService: HttpService,
    private router: Router) {
  }

  ngOnInit() {
    this.loadCompanies();
  }

  loadCompanies(): void {
    this.httpService.getCompanies().subscribe(
      cegek => {
        this.companies = cegek
        console.log(cegek);
      }
    )
  }

  submitCategory() {
    this.httpService.addCategory(this.category).subscribe((c: Category) => {
      this.router.navigate(['/category/list'])
    });
  }

}
