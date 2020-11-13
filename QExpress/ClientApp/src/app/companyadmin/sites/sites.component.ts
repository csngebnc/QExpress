import { Component, OnInit } from '@angular/core';
import { faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { HttpService } from '../../http.service';
import { Site } from '../../models/Site'

@Component({
  selector: 'app-sites',
  templateUrl: './sites.component.html',
  styleUrls: ['./sites.component.css']
})
export class SitesComponent implements OnInit {

  faEdit = faEdit;
  faTrashAlt = faTrashAlt;

  sites: Site[] = [];

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
    this.loadSites();
  }

  loadSites(){
    this.httpService.getOwnSites().subscribe((sites: Site[]) =>{
      this.sites = sites;
    })
  }

  deleteSite(id: Number){
    this.httpService.deleteSite(id).subscribe(() => {
      this.loadSites();
    })
  }
}
