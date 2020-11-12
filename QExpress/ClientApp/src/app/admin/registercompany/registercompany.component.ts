import {Component, Input, OnInit, Output} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {Subject, Subscription} from 'rxjs';
import { Company } from 'src/app/models/Company';
import {HttpService} from '../../http.service';

@Component({
  selector: 'app-registercompany',
  templateUrl: './registercompany.component.html',
  styleUrls: ['./registercompany.component.css']
})
export class RegistercompanyComponent implements OnInit {

  private routeSub: Subscription;

  @Output()
  saveClicked: Subject<void> = new Subject();

  @Input()
  company: Company = {
    nev: null,
    cegadminId: null,
     id: 1004
  };

  constructor(private httpService: HttpService) {}


  ngOnInit() {
  }

  submitCompany() {
    this.saveClicked.next();
    // this.httpService.addCompany(this.company).subscribe(
    //   res => console.log(res)
    // );
  }

}
