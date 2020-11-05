import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import { Company } from './models/Company'

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private baseUrl = 'https://localhost:44390/api/';

  constructor(private httpClient: HttpClient) {
  }

  //Cégek lekérése
  public getCompanies(): Observable<Company[]>{
    return this.httpClient.get<Company[]>(this.baseUrl + 'GetCegek')
  }

  //Adott ID-jű cég lekérése
  public getCompany(id: any): Observable<Company>{
    return this.httpClient.get<Company>(this.baseUrl + 'GetCeg/' + id)
  }
}
