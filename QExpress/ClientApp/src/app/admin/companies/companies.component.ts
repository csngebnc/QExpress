import {Component, OnInit} from '@angular/core';
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {HttpService} from '../../http.service';

/*
export class Ceg {
  name: string;
}
*/

@Component({
  selector: 'app-companies',
  templateUrl: './companies.component.html',
  styleUrls: ['./companies.component.css']
})
export class CompaniesComponent implements OnInit {

  faEdit = faEdit;
  faTrashAlt = faTrashAlt;

  // readonly url = 'https://localhost:44390/api/Ceg';

  cegek: any = [];

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
    console.log('hello');
    this.httpService.getCompanies().subscribe(
      companies => this.handleCompaniesResponse(companies)
    );
  }

  private handleCompaniesResponse(companies) {
    console.log(companies);
  }

}
