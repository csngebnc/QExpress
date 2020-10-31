import {Component, OnInit} from '@angular/core';
import {HttpService} from '../../http.service';

@Component({
  selector: 'app-registercompany',
  templateUrl: './registercompany.component.html',
  styleUrls: ['./registercompany.component.css']
})
export class RegistercompanyComponent implements OnInit {

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
  }

  public save(data): void {
    this.httpService.addCompany(data).subscribe(
      response => console.log(response)
    );
  }

}
