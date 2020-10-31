import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private baseUrl = 'https://localhost:5001/api/';

  constructor(private httpClient: HttpClient) {
  }

  // Összes lekérés
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

  public addCompany(data): Observable<any> {
    return this.postRequest('Ceg/AddCegParams', data);
  }

  public addCategory(data): Observable<any> {
    return this.postRequest('Kategoria/AddKategoria', data);
  }

  // GET kérés, a base URL beépítve, csak a kiegészítő útvonal hiányzik
  // Például: getRequest('Ceg') a baseUrl/Ceg útvonalat fogja hívni
  private getRequest(route: string): Observable<any> {
    return this.httpClient.get(this.baseUrl + route);
  }

  // Delete kérés, a GET-hez hasonlóan
  // Adott ID-val rendelkező elem törlése esetén a route része legyen az ID
  // Például: deleteRequest('Ceg/1') az 1-es ID-val rendelkező céget törli
  private deleteRequest(route: string): Observable<any> {
    return this.httpClient.delete(this.baseUrl + route);
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
  }
}
