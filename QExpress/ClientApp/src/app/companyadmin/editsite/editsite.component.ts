import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from 'src/app/http.service';
import { Site } from 'src/app/models/Site';

@Component({
  selector: 'app-editsite',
  templateUrl: './editsite.component.html',
  styleUrls: ['./editsite.component.css']
})
export class EditsiteComponent implements OnInit {

  site: Site;
  editSiteForm;
  
  constructor(
    private activatedRoute: ActivatedRoute,
    private httpService: HttpService,
    private router: Router,
    private formBuilder: FormBuilder) {
      this.editSiteForm = this.formBuilder.group({
        cim: ''
      })
  }

  ngOnInit() {
    const siteId= this.activatedRoute.snapshot.paramMap.get('siteid')
    this.httpService.getSiteById(parseInt(siteId)).subscribe(
      (site: Site) => {
        this.site = site;
      })
  }

  onSubmit(siteData){
    this.site.cim = siteData.cim;
    this.httpService.editSite(this.site).subscribe((site: Site) => {
      this.router.navigate(['/site/list']);
    })
  }
}
