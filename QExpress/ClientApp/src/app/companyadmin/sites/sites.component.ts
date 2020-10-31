import { Component, OnInit } from '@angular/core';
import { faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import {HttpService} from '../../http.service';

@Component({
  selector: 'app-sites',
  templateUrl: './sites.component.html',
  styleUrls: ['./sites.component.css']
})
export class SitesComponent implements OnInit {

  faEdit = faEdit;
  faTrashAlt = faTrashAlt;

  public telephelyek: any[] = [];

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
    console.log('Init Telephely');
    this.getTelephely();
  }

  private getTelephely(): void {
    console.log('Get Telephely');

    this.httpService.getTelephely().subscribe(
      telephelyek => this.handleTelephelyekResponse(telephelyek),
      // error => console.log(error)
    );
  }

  private handleTelephelyekResponse(telephelyek): void {
    console.log(telephelyek);
    this.telephelyek = telephelyek;
  }
}
