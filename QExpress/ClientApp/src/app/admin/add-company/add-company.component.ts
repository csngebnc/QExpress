import { Component, OnInit } from '@angular/core';
import {Company} from '../../models/Company';
import {HttpService} from '../../http.service';

@Component({
  selector: 'app-add-company',
  templateUrl: './add-company.component.html',
  styleUrls: ['./add-company.component.css']
})
export class AddCompanyComponent implements OnInit {

  company: Company = {
    nev: null,
    id: null,
    cegadminId: null
  };

  constructor(private httpService: HttpService) { }

  ngOnInit() {
  }

  saveNewCompany(): void {
    this.httpService.addCompany(this.company).subscribe(
      res => console.log(res)
    );
  }
}
