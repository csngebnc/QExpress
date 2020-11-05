import {Component, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import {HttpService} from '../../http.service';

@Component({
  selector: 'app-registercompany',
  templateUrl: './registercompany.component.html',
  styleUrls: ['./registercompany.component.css']
})
export class RegistercompanyComponent implements OnInit {

  private routeSub: Subscription;
  private companyid;

  constructor(private httpService: HttpService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.routeSub = this.route.params.subscribe(params => {
      this.companyid = params['companyid'];
    })
    console.log(this.companyid);
  }

  public getCompany(id): void{
    this.httpService.getCompany
  }

  public save(data): void {
    this.httpService.addCompany(data).subscribe(
      response => console.log(response)
    );
  }

}
