import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { PurchaseModelResponseStatus, PurchaseRequest, PurchaseResponse } from '../interfaces/purchase';
import { CommonService } from './CommonService';
import { StorageManager } from '../utils/storage-manager';

@Injectable({ providedIn: 'root' })
export class PurchaseService {

  private url = environment.apiUrl + '/api/purchases';

  httpOptions = {
    headers: new HttpHeaders()
     .set('Content-Type', 'application/json')
     .set('Authorization', '8548f3c8-8db3-4d42-b770-d7dd5c65a97b')
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

    getPurchaseByTrackingCode(code: string): Observable<PurchaseResponse> {
      const url = `${this.url}/tracking?code=${code}`;
      return this.http.get<PurchaseResponse>(url, {headers: this.getHttpHeaders() })
      .pipe(
        tap(),
        catchError(this.handleError<PurchaseResponse>
          (`Get Purchase By Tracking Code code=${code}`))
      );
    }

    getPurchases(): Observable<PurchaseResponse> {
      const url = `${this.url}`;
      return this.http.get<PurchaseResponse>(url, {headers: this.getHttpHeaders() })
      .pipe(
        tap(),
        catchError(this.handleError<PurchaseResponse>
          (`Get Purchases`))
      );
    }

    getPurchasesByDate(start: Date, end: Date): Observable<PurchaseResponse> {
      const url = `${this.url}/bydate?start=${start.toISOString()}&end=${end.toISOString()}`;
      return this.http.get<PurchaseResponse>(url, {headers: this.getHttpHeaders() })
      .pipe(
        tap(),
        catchError(this.handleError<PurchaseResponse>
          (`Get Purchases By Date`))
      );
    }
    
  //////// Save methods //////////

  /** POST: add a new purchase to the server */
  addPurchase(purchase: PurchaseRequest): Observable<PurchaseResponse> {
    return this.http.post<PurchaseResponse>(this.url, purchase, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<PurchaseResponse>('Add Purchase'))
    );
  }

  approvePurchase(id: number, pharmacyId: number, drugCode: string): Observable<PurchaseModelResponseStatus> {
    let purchase = {
      "pharmacyId": pharmacyId,
      "drugCode": drugCode
    };
    return this.http.put<PurchaseModelResponseStatus>(`${this.url}/approve/${id}`, purchase, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<PurchaseModelResponseStatus>('Approve Purchase Drug'))
    );
  }

  rejectPurchase(id: number, pharmacyId: number, drugCode: string): Observable<PurchaseModelResponseStatus> {
    let purchase = {
      "pharmacyId": pharmacyId,
      "drugCode": drugCode
    };
    return this.http.put<PurchaseModelResponseStatus>(`${this.url}/reject/${id}`, purchase, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<PurchaseModelResponseStatus>('Reject Purchase Drug'))
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
