import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Role } from '../interfaces/role';
import { environment } from '../../environments/environment';
import { CommonService } from './CommonService';

@Injectable({ providedIn: 'root' })
export class RoleService {

  private url = environment.apiUrl + '/api/roles';  // URL to web api

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient,
    private commonService: CommonService
    ) { }

  /** GET roles from the server */
  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(this.url)
      .pipe(
        tap(),
        catchError(this.handleError<Role[]>('Get Roles', []))
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

  /** Log a HeroService error with the MessageService */
  private log(message: string) {
      this.commonService.updateToastData(message, "danger", "Error");
  }
}
