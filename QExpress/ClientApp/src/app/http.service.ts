import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Company} from './models/Company';
import {Category} from './models/Category';
import {Employe} from './models/Employe';
import {User} from './models/User';
import {Queue} from './models/Queue';
import {Site} from './models/Site';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    Authorization: 'my-auth-token'
  })
};

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private baseUrl = 'https://localhost:44390/api/';

  constructor(private httpClient: HttpClient) {
  }

  // GET kérés, a base URL beépítve, csak a kiegészítő útvonal hiányzik
  // Például: getRequest('Ceg') a baseUrl/Ceg útvonalat fogja hívni
  private getRequest(route: String): Observable<any> {
    return this.httpClient.get(this.baseUrl + route);
  }

  // Delete kérés, a GET-hez hasonlóan
  // Adott ID-val rendelkező elem törlése esetén a route része legyen az ID
  // Például: deleteRequest('Ceg/1') az 1-es ID-val rendelkező céget törli
  private deleteRequest(route: String): Observable<any> {
    return this.httpClient.delete(this.baseUrl + route);
  }

  // POST kérés, GET-hez hasonló
  // Plusz adat a body, ez lesz a kérés body-ja JSON formátumban
  // A kapott objektum automatikusan JSON sorosítva lesz
  private postRequest(route: String, body: any): Observable<any> {
    return this.httpClient.post(this.baseUrl + route, body, httpOptions);

    // return this.httpClient.post(this.baseUrl + route, body);
  }

  // PUT kérés, a POST-hoz hasonló
  private putRequest(route: String, body): Observable<any> {
    return this.httpClient.put(this.baseUrl + route, body, httpOptions);

    // return this.httpClient.put(this.baseUrl + route, body);
  }

  //// CEGEK ////

  // Egy adott ID-jű cég lekérése
  public getCompany(id: any): Observable<Company> {
    // return this.httpClient.get<Company>(this.baseUrl + 'Ceg/GetCeg/' + id)
    return this.getRequest('Ceg/GetCeg/' + id);
  }

  public editCompany(company: Company): Observable<Company> {
    return this.putRequest('Ceg/UpdateCeg', company);
  }

  // Cégek lekérése
  public getCompanies(): Observable<Company[]> {
    // return this.httpClient.get<Company[]>(this.baseUrl + 'Ceg/GetCegek')
    return this.getRequest('Ceg/GetCegek');
  }

  // Cég törlése
  public deleteCompany(id: Number): Observable<any> {
    return this.deleteRequest('Ceg/Delete/' + id.toString());
  }

  // Egy cég hozzáadása
  public addCompany(company: Company): Observable<Company> {
    return this.postRequest('Ceg/AddCeg', company);
    //return this.httpClient.post<Company>(this.baseUrl + 'Ceg/AddCeg', company, httpOptions);
  }

  // Egy cég nevének szerkesztése
  public editCompanyName(company: Company): Observable<Company> {
    // return this.putRequest('Ceg/' + company.id.toString() + '/NewName', company);
    return this.putRequest('Ceg/' + company.id.toString() + '/NewName', {
      uj_nev: company.nev
    });
  }

  // Egy cég adminjának szerkesztése
  public editCompanyAdmin(company: Company): Observable<Company> {
    // return this.putRequest('Ceg/' + company.id.toString() + '/UpdateAdmin', company);
    return this.putRequest('Ceg/' + company.id.toString() + '/UpdateAdmin', {
      // valószínűleg módosítani kell
      uj_admin_felhasznalonev: company.cegadminId
    });
  }

  //// KATEGORIA ////

  // Kategoriák lekerese
  public getCategories(): Observable<Category[]> {
    return this.getRequest('Kategoria/GetKategoriak');
  }

  public getCategory(id: any): Observable<Category> {
    return this.getRequest('Kategoria/GetKategoria/' + id);
  }

  public editCategorName(category: Category): Observable<Category> {
    // return this.putRequest('Kategoria/' + category.id.toString() + '/NewName', category);
    return this.putRequest('Kategoria/' + category.id.toString() + '/NewName', {
      uj_megnevezes: category.megnevezes
    });
  }

  public addCategory(category: Category): Observable<Category> {
    return this.postRequest('Kategoria/AddKategoria', category);
  }

  // Kategoria törlése
  public deleteCategory(id: Number): Observable<Category> {
    return this.deleteRequest('Kategoria/Delete/' + id.toString());
  }

  //// ALKALMAZOTTAK ////

  // Felhasználók lekérése
  public getUsers(): Observable<User[]> {
    return this.getRequest('Felhasznalo/GetFelhasznalok');
  }

  // Jelenlegi felhasznalo lekérése
  public getCurrentUser(): Observable<User> {
    return this.getRequest('Felhasznalo/GetCurrentFelhasznalo');
  }

  // Felhasznalo lekérése id alapján
  public getUserById(id: String): Observable<User> {
    return this.getRequest('Felhasznalo/GetFelhasznalo/' + id);
  }

  public getUserByEmail(email: String): Observable<User> {
    return this.getRequest('Felhasznalo/GetFelhasznaloByEmail/' + email);
  }

  // Felhasznalo aktív sorszámainak a lekérése
public getActiveQueue(): Observable<Queue[]> {
    return this.getRequest('Felhasznalo/AktivSorszamok');
  }

  // Felhasznalo előző sorszámainak a lekérése
  public getQueueHistory(): Observable<Queue[]> {
    return this.getRequest('Felhasznalo/KorabbiSorszamok');
  }

  // Felhasznalo hozzáadása
  public addUser(user: User): Observable<User> {
    return this.postRequest('Felhasznalo/AddFelhasznalo', user);
  }

  // Felhasznalo email megváltoztatása
  public editEmail(user: User): Observable<User> {
    return this.postRequest('Felhasznalo/NewEmail', user);
  }

  //// Telephelyek ////

  // Telephelyek lekérése
  public getSites(): Observable<Site[]> {
    return this.getRequest('Telephely/GetTelephelyekCegadmin');
  }

  // Egy telephely lekérése
  public getSiteById(id: Number): Observable<Site> {
    return this.getRequest('Telephely/GetTelephely/' + id);
  }

  // Egy telephely hozzáadása
  public addSite(site: Site): Observable<Site> {
    return this.postRequest('Telephely/AddTelephely', site)
  }

  // Egy telephely törlése
  public deleteSite(id: Number): Observable<Site> {
    return this.deleteRequest('Telephely/Delete/' + id);
  }

  public editSite(site: Site): Observable<Site> {
    return this.putRequest('Telephely/UpdateTelephely', site);
  }

  /*
  private deleteRequest(route: String): Observable<any> {
    return this.httpClient.delete(this.baseUrl + route);
  }
   */
}
