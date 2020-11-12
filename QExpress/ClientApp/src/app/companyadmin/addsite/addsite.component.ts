import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpService } from 'src/app/http.service';
import { Company } from 'src/app/models/Company';
import { Site } from 'src/app/models/Site';

@Component({
  selector: 'app-addsite',
  templateUrl: './addsite.component.html',
  styleUrls: ['./addsite.component.css']
})
export class AddsiteComponent implements OnInit {

  @Input()
  site: Site = {
    id: 0,
    companyid: 0,
    cim: ''
  }

  constructor(
    private httpService: HttpService,
    private router: Router) {}

  ngOnInit() {
  }

  submitSite(){
    this.httpService.addSite(this.site).subscribe(() => {
      this.router.navigate(['/site/list']);
    });
  }
}
