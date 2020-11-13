
import {Component, Input, OnInit, Output} from '@angular/core';
import { Router } from '@angular/router';
import {Subject, Subscription} from 'rxjs';
import { Company } from 'src/app/models/Company';
import {HttpService} from '../../http.service';
import { User } from '../../models/User';

@Component({
  selector: 'app-registercompany',
  templateUrl: './registercompany.component.html',
  styleUrls: ['./registercompany.component.css']
})
export class RegistercompanyComponent implements OnInit {

  private routeSub: Subscription;

  //Megkérdezni Szimit ez mire kell
  @Output()
  saveClicked: Subject<void> = new Subject();

  @Input()
  company: Company = {
    id: -1,
    nev: '',
    cegadminId: ''
  };
  email: String;

  constructor(
    private httpService: HttpService,
    private router: Router) {}

  ngOnInit() {
  }

  submitCompany() {
    this.httpService.getUserByEmail(this.email).subscribe((user: User) => {
      this.company.cegadminId = user.id;
      this.company.id = 0;
      this.httpService.addCompany(this.company).subscribe((c: Company) => {
        this.router.navigate(['/company/list'])
      });
    })
  }
}
