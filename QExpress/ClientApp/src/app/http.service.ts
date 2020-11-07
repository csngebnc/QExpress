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

  // GET
  // Összes cég lekérése
  public getCompanies(): Observable<any> {
    console.log('get companies');
    return this.getRequest('Ceg');
  }

  // felhasználók lekérése
  public getFelhasznalo(): Observable<any> {
    console.log('Get felhasznalo http');
    return this.getRequest('Felhasznalo');
  }

  // kategóriák lekérése
  public getCategories(): Observable<any> {
    console.log('Get categories http');
    return this.getRequest('Kategoria');
  }

  // Sorszam lekérése
  public getSorszam(): Observable<any> {
    console.log('Get sorszam http');
    return this.getRequest('Sorszam');
  }

  // Telephely lekérése
  public getTelephely(): Observable<any> {
    console.log('Get telephely http');
    return this.getRequest('Telephely');
  }

  // Levelek lekérése
  public getUgyfLevelek(): Observable<any> {
    console.log('Get levelek http');
    return this.getRequest('UgyfLevelek');
  }

  // POST
  // Cég
  public addCompany(data): Observable<any> {
    return this.postRequest('Ceg/AddCegParams', data);
  }

  // Felhasznalo
  public addUser(data): Observable<any> {
    return this.postRequest('Felhasznalo/AddFelhasznalo', data);
  }

  public setTelephely(data): Observable<any> {
    return this.postRequest('Felhasznalo/SetTelephely', data);
  }

  // Kategoria
  public addCategory(data): Observable<any> {
    return this.postRequest('Kategoria/AddKategoria', data);
  }

  // Sorszam
  public addSorszam(data): Observable<any> {
    return this.postRequest('Sorszam/AddSorszam', data);
  }

  // Telephely
  public addTelephely(data): Observable<any> {
    return this.postRequest('Telephely/AddTelephely', data);
  }

  // UgyfLevelek
  public addUgyfLevelek(data): Observable<any> {
    return this.postRequest('UgyfLevelek/AddUgyfLevelek', data);
  }

  // DELETE
  public deleteUgyfLevelek(data): Observable<any> {
    return this.deleteRequest('UgyfLevelek/{data}', data);
  }

  // GET kérés, a base URL beépítve, csak a kiegészítő útvonal hiányzik
  // Például: getRequest('Ceg') a baseUrl/Ceg útvonalat fogja hívni
  private getRequest(route: string): Observable<any> {
    return this.httpClient.get(this.baseUrl + route);
  }

  // Delete kérés, a GET-hez hasonlóan
  // Adott ID-val rendelkező elem törlése esetén a route része legyen az ID
  // Például: deleteRequest('Ceg/1') az 1-es ID-val rendelkező céget törli
  private deleteRequest(route: string, body): Observable<any> {
    return this.httpClient.delete(this.baseUrl + route, body);
  }

  // POST kérés, GET-hez hasonló
  // Plusz adat a body, ez lesz a kérés body-ja JSON formátumban
  // A kapott objektum automatikusan JSON sorosítva lesz
  private postRequest(route: string, body): Observable<any> {
    return this.httpClient.post(this.baseUrl + route, body);
  }

  // PUT kérés, a POST-hoz hasonló
  private putRequest(route: string, body): Observable<any> {
    return this.httpClient.put(this.baseUrl + route, body);
  //Adott ID-jű cég lekérése
  public getCompany(id: any): Observable<Company>{
    return this.httpClient.get<Company>(this.baseUrl + 'Ceg/GetCeg/' + id)
  }

  /*
  private deleteRequest(route: string): Observable<any> {
    return this.httpClient.delete(this.baseUrl + route);
  }
   */
}
