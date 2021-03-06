import {Component, OnInit} from '@angular/core';
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import { User } from 'src/app/models/User';
import {HttpService} from '../../http.service';
import {Company} from '../../models/Company';

@Component({
  selector: 'app-companies',
  templateUrl: './companies.component.html',
  styleUrls: ['./companies.component.css']
})
export class CompaniesComponent implements OnInit {

  faEdit = faEdit;
  faTrashAlt = faTrashAlt;

  companies: Company[] = [];
  companyAdminEmails: String[] = [];

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
    this.loadCompanies();
  }

  deleteCompany(id: number): void {
    this.httpService.deleteCompany(id).subscribe(
      () => this.loadCompanies()
    );
  }

  loadCompanies(): void {
    this.httpService.getCompanies().subscribe((companies: Company[]) => {
      this.companies = companies;
      companies.forEach( c => {
        this.httpService.getUserById(c.cegadminId).subscribe((u: User) => {
          this.companyAdminEmails.push(u.email)
        })
      })
    });
  }
}
