import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Invitation, InvitationRequest } from '../interfaces/invitation';
import { environment } from '../../environments/environment';
import { CommonService } from './CommonService';
import { StorageManager } from '../utils/storage-manager';

@Injectable({ providedIn: 'root' })
export class InvitationService {

  private url = environment.apiUrl + '/api/invitations';

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

  /** POST Create Invitation */
  createInvitation(invitation: InvitationRequest): Observable<Invitation> {
    return this.http.post<Invitation>(this.url, invitation, {headers: this.getHttpHeaders() }).pipe(
      tap(),
      catchError(this.handleError<Invitation>('Create Invitation'))
    );
  }

  /** GET invitations from the server */
  getFilterInvitations(pharmacyName: string, userName: string, roleName: string): Observable<Invitation[]> {
    let filter = "";
    if (pharmacyName.trim() !== ""){
      filter += "&Pharmacy=" + pharmacyName;
    }
    if (roleName.trim() !== ""){
      filter += "&Role=" + roleName;
    }
    if (userName.trim() !== ""){
      filter += "&UserName=" + userName;
    }
    filter = filter.replace("&", "?");

    return this.http.get<Invitation[]>(this.url + filter, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<Invitation[]>("Get Filtered Invitations", []))
    );
  }

  getInvitationById(id: number): Observable<Invitation> {
    const url = `${this.url}/${id}`;
    return this.http.get<Invitation>(url).pipe(
      tap(),
      catchError(this.handleError<Invitation>(`Get Invitation id=${id}`))
    );
  }

  getNewUserCode(): Observable<Invitation> {
    const url = `${this.url}/UserCode`;
    return this.http.get<Invitation>(url, {headers: this.getHttpHeaders() }).pipe(
      tap(),
      catchError(this.handleError<Invitation>(`Get new User Code.`))
    );
  }

  /* PUT Update Invitation */
  updateInvitation(id: number, invitation: InvitationRequest): Observable<Invitation> {
    const url = `${this.url}/${id}`;
    return this.http.put<Invitation>(url, invitation, {headers: this.getHttpHeaders() }).pipe(
      tap(),
      catchError(this.handleError<Invitation>('Update Invitation'))
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

  /** Log a InvitationService error with the MessageService */
  private log(message: string) {
    this.commonService.updateToastData(message, "danger", "Error");
  }
}
