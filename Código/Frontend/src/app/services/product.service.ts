import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Product, ProductRequest, UpdateProductRequest } from '../interfaces/product';
import { environment } from '../../environments/environment';
import { CommonService } from './CommonService';
import { StorageManager } from '../utils/storage-manager';

@Injectable({ providedIn: 'root' })
export class ProductService {

  private url = environment.apiUrl + '/api/product';

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

  /** GET products from the server */
  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.url, {headers: this.getHttpHeaders() })
      .pipe(
        tap(),
        catchError(this.handleError<Product[]>('Get Products', []))
      );
  }

  getProductsFilter(pharmacyId: string, pharmacyName: string): Observable<Product[]> {
    let filter = "";
    if (pharmacyName.trim() !== ""){
      filter += "&Name=" + pharmacyName;
    }
    if (pharmacyId !== "0"){
      filter += "&PharmacyId=" + pharmacyId;
    }
    filter = filter.replace("&", "?");

    return this.http.get<Product[]>(this.url + filter, {headers: this.getHttpHeaders() })
      .pipe(
        tap(),
        catchError(this.handleError<Product[]>('Get Filtered Products', []))
      );
  }

  /** GET product by id. Will 404 if id not found */
  getProduct(id: number): Observable<Product> {
    const url = `${this.url}/${id}`;
    return this.http.get<Product>(url, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<Product>(`Get Product id=${id}`))
    );
  }

  getProductByUser(): Observable<Product[]> {
    const url = `${this.url}`;
    return this.http.get<Product[]>(url, {headers: this.getHttpHeaders() })
      .pipe(
        tap(),
        catchError(this.handleError<Product[]>('Get Products By User', []))
      );
  }

  /** POST Create Product */
  createProduct(product: ProductRequest): Observable<Product> {
   return this.http.post<Product>(this.url, product, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<Product>('Create Product'))
    );
   
  }

  updateProduct(id: number, product: UpdateProductRequest) : Observable<Product> {
    const url = `${this.url}/${id}`;
    return this.http.put<Product>(url, product, {headers: this.getHttpHeaders()}).pipe(
      tap(),
      catchError(this.handleError<Product>('Update Product'))
    );
  }

  /** DELETE Delete Product */
  deleteProduct(id: number): Observable<any> {
    const url = `${this.url}/${id}`;
    return this.http.delete<any>(url, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<any>('Delete Product'))
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
