import {Component, ErrorHandler, OnInit} from '@angular/core';
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {HttpService} from '../../http.service';
import {Company} from '../../models/Company'
import { catchError, tap } from 'rxjs/operators';

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

  companies: Company[] = [];

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
    this.httpService.getCompanies().subscribe((companies: Company[])=>{
      this.companies = companies;
      console.log(companies);
    })
  }
}
