import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { CommonService } from './CommonService';
import { UserRequest, UserResponse } from '../interfaces/user';

@Injectable({ providedIn: 'root' })
export class UserService {

  private url = environment.apiUrl + '/api/users';

  constructor(
    private http: HttpClient,
    private commonService: CommonService) { }

  /** POST Users from the server */
  createUser(user: UserRequest): Observable<UserResponse> {
    return this.http.post<UserResponse>(this.url, user).pipe(
      tap(),
      catchError(this.handleError<UserResponse>('Create User'))
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
