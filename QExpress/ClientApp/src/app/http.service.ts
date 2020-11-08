import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Company} from './models/Company';
import {Category} from './models/Category';
import {Employe} from './models/Employe';
import {User} from './models/User';
import {Queue} from './models/Queue';
import {Site} from './models/Site';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private baseUrl = 'https://localhost:5001/api/';

  constructor(private httpClient: HttpClient) {
  }

  /*
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
  */

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
    return this.httpClient.post(this.baseUrl + route, null, {
      params: body
    });

    // return this.httpClient.post(this.baseUrl + route, body);
  }

  // PUT kérés, a POST-hoz hasonló
  private putRequest(route: string, body): Observable<any> {
    return this.httpClient.put(this.baseUrl + route, null, {
      params: body
    });

    // return this.httpClient.put(this.baseUrl + route, body);
  }

  //// CEGEK ////

  // Egy adott ID-jű cég lekérése
  public getCompany(id: any): Observable<Company> {
    // return this.httpClient.get<Company>(this.baseUrl + 'Ceg/GetCeg/' + id)
    return this.getRequest('Ceg/GetCeg/' + id);
  }

  // Cégek lekérése
  public getCompanies(): Observable<Company[]> {
    // return this.httpClient.get<Company[]>(this.baseUrl + 'Ceg/GetCegek')
    return this.getRequest('Ceg/GetCegek');
  }

  // Cég törlése
  public deleteCompany(id: number): Observable<any> {
    return this.deleteRequest('Ceg/Delete/' + id.toString());
  }

  // Egy cég hozzáadása
  public addCompany(company: Company): Observable<Company> {
    // átírva a Cegkontroller, ha nem lenne, akkor ezzel működne
    // const backendCompany = {
    //   cegnev: company.nev,
    //   cegadmin_id: company.cegadminId
    // };
    // return this.postRequest('Ceg/AddCeg', backendCompany);

    return this.postRequest('Ceg/AddCeg', company);
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
  public deleteCategory(id: number): Observable<Category> {
    return this.deleteRequest('Kategoria/Delete/' + id.toString());
  }

  //// ALKALMAZOTTAK ////

  // Alkalmazottak lekérése
  public getEmployee(): Observable<User[]> {
    return this.getRequest('Felhasznalo/GetFelhasznalok');
  }

  // Jelenlegi felhasznalo lekérése
  public getEmployeCurrent(): Observable<User> {
    return this.getRequest('Felhasznalo/GetCurrentFelhasznalo');
  }

  // Felhasznalo lekérése id alapján
  public getEmployeId(id: any): Observable<User> {
    return this.getRequest('Felhasznalo/GetFelhasznalo/' + id);
  }

  // Felhasznalo aktív sorszámainak a lekérése
  public getEmployeCurrentQueue(): Observable<Queue[]> {
    return this.getRequest('Felhasznalo/AktivSorszamok');
  }

  // Felhasznalo előző sorszámainak a lekérése
  public getEmployeOldQueue(): Observable<Queue[]> {
    return this.getRequest('Felhasznalo/KorabbiSorszamok');
  }

  // Felhasznalo hozzáadása
  public addEmploye(user: User): Observable<User> {
    return this.postRequest('Felhasznalo/AddFelhasznalo', user);
  }

  // Felhasznalo email megváltoztatása
  public editEmployeEmail(user: User): Observable<User> {
    return this.postRequest('Felhasznalo/NewEmail', user);
  }

  //// Telephelyek ////

  // Telephelyek lekérése
  public getSites(site: Site): Observable<Site[]> {
    return this.getRequest('Telephely/GetTelephelyek');
  }

  // Egy telephely lekérése
  public getSiteId(id: any): Observable<Site> {
    return this.getRequest('Telephely/GetTelephelyek/' + id);
  }

  // Egy telephely hozzáadása
  public addSite(site: Site): Observable<Site> {
    return this.postRequest('Telephely/AddTelephely', site);
  }

  // Egy telephely törlése
  public deleteSite(id: any): Observable<Site> {
    return this.getRequest('Telephely/Delete' + id);
  }

  /*
  private deleteRequest(route: string): Observable<any> {
    return this.httpClient.delete(this.baseUrl + route);
  }
   */
}
