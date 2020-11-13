import { Component, OnInit } from '@angular/core';
import {MatDialog} from '@angular/material';
import { HttpService } from 'src/app/http.service';
import { Category } from 'src/app/models/Category';
import { Company } from 'src/app/models/Company';
import { Queue } from 'src/app/models/Queue';
import { Site } from 'src/app/models/Site';
import { FormBuilder} from '@angular/forms'

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
    private formBuilder: FormBuilder) {
      formBuilder.group()
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

  constructor(private httpService: HttpService){

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

  loadCategories(){
    this.httpService.getCategories(this.selectedCompany.id).subscribe((c: Category[]) => {
      this.categories = c;
    })
  }

  loadSites(){
    this.httpService.getSites(this.selectedCompany.id).subscribe((s: Site[]) => {
      this.sites = s;
    })
  }
    
}
