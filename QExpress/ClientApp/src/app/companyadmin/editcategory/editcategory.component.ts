import {Component, OnInit} from '@angular/core';
import {HttpService} from '../../http.service';

export class Category {
  nev: string;
}

@Component({
  selector: 'app-editcategory',
  templateUrl: './editcategory.component.html',
  styleUrls: ['./editcategory.component.css']
})
export class EditcategoryComponent implements OnInit {

  public category: Category = {
    nev: 'Example name',
  };

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
  }

  public submitCategory(): void {
    this.httpService.addCategory(this.category).subscribe(
      res => console.log(res)
    );
  }

}
