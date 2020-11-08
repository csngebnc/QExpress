import { Component, OnInit } from '@angular/core';
//import { type } from 'os';
import { Observable } from 'rxjs';
import { AuthorizeService, IUser } from 'src/api-authorization/authorize.service'
import { HttpService } from 'src/app/http.service'
import { User } from '../models/User'

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit{
  isExpanded = false;
  user: User;

  // user level 4: cegek
  // user level 3: kategoriak, alkalmazottak, telephelyek
  private user_level: number;

  constructor(private authorizeService: AuthorizeService, private httpService: HttpService){

  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  ngOnInit() {
    this.user_level = 4;
  }
}
