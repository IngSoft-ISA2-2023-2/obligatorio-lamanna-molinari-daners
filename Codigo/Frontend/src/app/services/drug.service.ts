import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Drug, DrugRequest } from '../interfaces/drug';
import { environment } from '../../environments/environment';
import { CommonService } from './CommonService';
import { StorageManager } from '../utils/storage-manager';

@Injectable({ providedIn: 'root' })
export class DrugService {

  private url = environment.apiUrl + '/api/drug';

  httpOptions = {
    headers: new HttpHeaders()
     .set('Content-Type', 'application/json')
     .set('Authorization', 'f4522298-a723-4c47-ad43-594f09eeae66')
  };

  constructor(
    private http: HttpClient,
    private commonService: CommonService,
    private storageManager: StorageManager) { }

  getHttpHeaders(): HttpHeaders {
    let login = JSON.parse(this.storageManager.getLogin());
    let token = login ? login.token : "";
    
    return new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('Authorization', token);
  }

  /** GET drugs from the server */
  getDrugs(): Observable<Drug[]> {
    return this.http.get<Drug[]>(this.url, {headers: this.getHttpHeaders() })
      .pipe(
        tap(),
        catchError(this.handleError<Drug[]>('Get Drugs', []))
      );
  }

  getDrugsFilter(pharmacyId: string, pharmacyName: string): Observable<Drug[]> {
    let filter = "";
    if (pharmacyName.trim() !== ""){
      filter += "&Name=" + pharmacyName;
    }
    if (pharmacyId !== "0"){
      filter += "&PharmacyId=" + pharmacyId;
    }
    filter = filter.replace("&", "?");

    return this.http.get<Drug[]>(this.url + filter, {headers: this.getHttpHeaders() })
      .pipe(
        tap(),
        catchError(this.handleError<Drug[]>('Get Filtered Drugs', []))
      );
  }

  /** GET drug by id. Will 404 if id not found */
  getDrug(id: number): Observable<Drug> {
    const url = `${this.url}/${id}`;
    return this.http.get<Drug>(url, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<Drug>(`Get Drug id=${id}`))
    );
  }

  getDrugsByUser(): Observable<Drug[]> {
    const url = `${this.url}/user`;
    return this.http.get<Drug[]>(url, {headers: this.getHttpHeaders() })
      .pipe(
        tap(),
        catchError(this.handleError<Drug[]>('Get Drugs By User', []))
      );
  }

  /** POST Create Drug */
  createDrug(drug: DrugRequest): Observable<Drug> {
    return this.http.post<Drug>(this.url, drug, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<Drug>('Create Drug'))
    );
  }

  /** DELETE Delete Drug */
  deleteDrug(id: number): Observable<any> {
    const url = `${this.url}/${id}`;
    return this.http.delete<any>(url, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<any>('Delete Drug'))
    );
  }
  /**
   * Handle Http operation that failed.
   * Let the app continue.
   *
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      //console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  /** Log a DrugService error with the MessageService */
  private log(message: string) {
    this.commonService.updateToastData(message, "danger", "Error");
  }
}
