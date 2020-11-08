import {Component, OnInit} from '@angular/core';
import {Company} from '../../models/Company';
import {HttpService} from '../../http.service';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-edit-company',
  templateUrl: './edit-company.component.html',
  styleUrls: ['./edit-company.component.css']
})
export class EditCompanyComponent implements OnInit {

  company: Company = {
    nev: 'meglévő',
    id: 2,
    cegadminId: 'Hello'
  };

  constructor(
    private activatedRoute: ActivatedRoute,
    private httpService: HttpService) {
  }

  ngOnInit() {
    const companyId = this.activatedRoute.snapshot.paramMap.get('companyid');
    console.log(companyId);
    this.httpService.getCompany(companyId).subscribe(
      company => this.company = company
    );
  }

  editCompany(): void {
    this.httpService.addCompany(this.company).subscribe(
      res => console.log(res)
    );
  }

}
