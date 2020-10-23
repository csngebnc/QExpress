import { Component, OnInit } from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import { faMobile } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-current',
  templateUrl: './current.component.html',
  styleUrls: ['./current.component.css']
})
export class CurrentComponent{

  active_queue = [];

  constructor(public dialog: MatDialog) {

    this.active_queue = 
    [{
      company: "T-Mobile",
      companylogo: "../../../assets/tmobile.png",
      address: "Bartók Béla utca 23.",
      number: 34,
      inline: 5
    },
    {
      company: "UPC",
      companylogo: "../../../assets/upc.png",
      address: "Bartók Béla utca 23.",
      number: 34,
      inline: 5
    },
    {
      company: "T-Mobile",
      companylogo: "../../../assets/tmobile.png",
      address: "Bartók Béla utca 23.",
      number: 34,
      inline: 5
    },
    {
      company: "T-Mobile",
      companylogo: "../../../assets/tmobile.png",
      address: "Bartók Béla utca 23.",
      number: 34,
      inline: 5
    },
    {
      company: "T-Mobile",
      companylogo: "../../../assets/tmobile.png",
      address: "Bartók Béla utca 23.",
      number: 34,
      inline: 5
    },
    {
      company: "T-Mobile",
      companylogo: "../../../assets/tmobile.png",
      address: "Bartók Béla utca 23.",
      number: 34,
      inline: 5
    },
  ]

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
  templateUrl: 'new-dialog.html',
  styleUrls: ['./new-dialog.css']
})
export class NewDialog {}
