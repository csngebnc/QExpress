import { Component, OnInit } from '@angular/core';
import { faEdit, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import {HttpService} from '../../http.service';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css']
})
export class EmployeesComponent implements OnInit {

  faEdit = faEdit;
  faTrashAlt = faTrashAlt;

  public alkalmazottak: any[] = [];

  constructor(private httpService: HttpService) {
  }

  ngOnInit() {
    console.log('Init Alkalamzottak');
    this.getAlkalmazottak();
  }

  private getAlkalmazottak(): void {
    console.log('Get alkalmazottak');

    this.httpService.getFelhasznalo().subscribe(
      alkalmazottak => this.handleAlkalmazottakResponse(alkalmazottak),
      // error => console.log(error)
    );
  }

  private handleAlkalmazottakResponse(alkalmazottak): void {
    console.log(alkalmazottak);
    this.alkalmazottak = alkalmazottak;
  }
}
