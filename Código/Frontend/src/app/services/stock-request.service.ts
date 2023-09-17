import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { from, Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Drug, DrugRequest } from '../interfaces/drug';
import { environment } from '../../environments/environment';
import { CommonService } from './CommonService';
import { RequestDetail, RequestDetailHeader, StockRequest } from '../interfaces/stock-request';
import { StorageManager } from '../utils/storage-manager';

@Injectable({ providedIn: 'root' })
export class StockRequestService {

  private url = environment.apiUrl + '/api/stockrequest';

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
  
  /** GET Stock Requests from the server */
  getStockRequests(status: string, code: string, start?: Date, end?: Date): Observable<StockRequest[]> {
    let fromDate = "";
    let toDate = "";
    if (start){
      fromDate = start.toISOString();
    }
    if (end){
      toDate = end.toISOString();
    } 
    const url = `${this.url}/byemployee?FromDate=${fromDate}&ToDate=${toDate}&Status=${status}&Code=${code}`;
    return this.http.get<StockRequest[]>(url, {headers: this.getHttpHeaders() })
      .pipe(
        tap(),
        catchError(this.handleError<StockRequest[]>('Get Stock Requests By Employee', []))
      );
  }

  /** GET Stock Requests from the server */
  getStockRequestsByOwner(): Observable<StockRequest[]> {
    return this.http.get<StockRequest[]>(this.url, {headers: this.getHttpHeaders() })
      .pipe(
        tap(),
        catchError(this.handleError<StockRequest[]>('Get Stock Requests By Owner', []))
      );
  }

  /** Post Stock Requests from the server */
  createStockRequest(request: RequestDetailHeader[]): Observable<any> {
    let req = new RequestDetail(request);
    return this.http.post<any>(this.url, req, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<any>('Create Stock Request'))
    );
  }

  approveStockRequest(requestId: number): Observable<any> {
    const url = `${this.url}/approveStockRequest/${requestId}`;
    return this.http.put<any>(url, requestId, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<any>('Approve Stock Request'))
    );
  }

  rejectStockRequest(requestId: number): Observable<any> {
    const url = `${this.url}/rejectStockRequest/${requestId}`;
    return this.http.put<any>(url, requestId, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<any>('Reject Stock Request'))
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
