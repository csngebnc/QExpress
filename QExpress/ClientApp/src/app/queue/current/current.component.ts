import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import {MatDialog} from '@angular/material';
import { Router } from '@angular/router';
import { HttpService } from 'src/app/http.service';
import { Category } from 'src/app/models/Category';
import { Company } from 'src/app/models/Company';
import { Queue } from 'src/app/models/Queue';
import { Site } from 'src/app/models/Site';

@Component({
  selector: 'app-current',
  templateUrl: './current.component.html',
  styleUrls: ['./current.component.css'],
})
export class CurrentComponent implements OnInit{

  activeQueue: Queue[] = [];

  constructor(
    public dialog: MatDialog,
    public httpService: HttpService,
    ) {
  }

  ngOnInit(){
    this.loadQueue();
  }

  loadQueue(){
    this.httpService.getActiveQueue().subscribe((aq: Queue[]) => {
      this.activeQueue = aq;
    })
  }

  openDialog() {
    const dialogRef = this.dialog.open(NewDialog);

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }
}

@Component({
  selector: 'queue-new',
  templateUrl: './new-dialog.html',
  styleUrls: ['./new-dialog.css']
})
export class NewDialog implements OnInit{

  companies: Company[] = [];
  sites: Site[] = [];
  categories: Category[] = [];
  selectedCompany: Company;
  newQueueForm;

  constructor(
    private httpService: HttpService,
    private formBuilder: FormBuilder,
    private router: Router){
      this.newQueueForm = this.formBuilder.group({
        companyId: '',
        categoryId: '',
        siteId: ''
      })
  }

  ngOnInit(){
    this.loadCompanies();
  }

  loadCompanies(){
    this.httpService.getCompanies().subscribe((c: Company[]) => {
      this.companies = c;
      this.selectedCompany = this.companies[0];
      this.loadCategories();
      this.loadSites();
    })
  }

  // loadCategories(){
  //   this.httpService.getCategories(this.selectedCompany.id).subscribe((c: Category[]) => {
  //     this.categories = c;
  //   })
  // }

  loadCategories(){
      this.httpService.getCategory(this.selectedCompany.id).subscribe((c: Category[]) => {
        this.categories = c;
      })
    }

  loadSites(){
    this.httpService.getSites(this.selectedCompany.id).subscribe((s: Site[]) => {
      this.sites = s;
    })
  }


  onCompanyChanged(value){
    this.httpService.getCompany(value).subscribe((c: Company) => {
      this.selectedCompany = c;
      this.loadCategories();
      this.loadSites();
    })
  }

  getQueue(queueData){
    var newQueue: Queue = {
      id: -1,
      sorszamIdTelephelyen: -1,
      ugyfelId: '',
      telephelyId: -1,
      kategoriaId: -1,
      idopont: new Date("2034-12-12"),
      allapot: '',
      sorbanAllokSzama: -1,
      telephely: '',
      kategoria: '',
      ugyfel: '',
      ceg: '',
    }
    newQueue.kategoriaId = queueData.categoryId;
    newQueue.telephelyId = queueData.siteId;
    this.httpService.newQueue(newQueue).subscribe(() => {
      this.router.navigate(['/'])
    })
  }
}
