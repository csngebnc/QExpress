import {Component, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Company } from 'src/app/models/Company';
import {HttpService} from '../../http.service';

@Component({
  selector: 'app-registercompany',
  templateUrl: './registercompany.component.html',
  styleUrls: ['./registercompany.component.css']
})
export class RegistercompanyComponent implements OnInit {

  private routeSub: Subscription;
  private company: Company;

  constructor(private httpService: HttpService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    var companyid;
    
    this.routeSub = this.route.params.subscribe(params => {
      companyid = params['companyid'];
    })

    if(companyid !== 'new'){
      this.httpService.getCompany(companyid).subscribe((company: Company) =>
      this.company = company)
    }
    
    console.log(companyid);
  }

  public save(data): void {
    this.httpService.addCompany(data).subscribe(
      response => console.log(response)
    );
  }

}
