import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap, catchError, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DrugExportationModel } from '../interfaces/drug-exportation-model';
import { Parameter } from '../interfaces/parameter';
import { CommonService } from './CommonService';
import { StorageManager } from '../utils/storage-manager';

@Injectable({
  providedIn: 'root',
})
export class ExportService {
  private basic_url = environment.apiUrl + '/api/Export';
  private exporters_url = this.basic_url + '/exporters';
  private parameters_url = this.basic_url + '/parameters';

  constructor(private http: HttpClient, private commonService: CommonService, private storageManager: StorageManager) {}

  getExporters(): Observable<string[]> {
    return this.http
      .get<string[]>(this.exporters_url, {headers: this.getHttpHeaders()})
      .pipe(tap(), catchError(this.handleError<string[]>(`Get Exporters`)));
  }

  getParameters(exporterName: string): Observable<Parameter[]> {
    const url = `${this.parameters_url}?exporterName=${exporterName}`;
    return this.http
      .get<Parameter[]>(url, {headers: this.getHttpHeaders()})
      .pipe(tap(), catchError(this.handleError<Parameter[]>('Get Parameters')));
  }

  export(
    drugExportModel: DrugExportationModel
  ): Observable<DrugExportationModel> {
    return this.http
      .post<DrugExportationModel>(this.basic_url, drugExportModel, {headers: this.getHttpHeaders()})
      .pipe(
        tap(),
        catchError(this.handleError<DrugExportationModel>('Export Drugs'))
      );
  }

  getHttpHeaders(): HttpHeaders {
    let login = JSON.parse(this.storageManager.getLogin());
    let token = login ? login.token : '';

    return new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('Authorization', token);
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      this.log(`${operation} failed: ${error.error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  private log(message: string) {
    this.commonService.updateToastData(message, 'danger', 'Error');
  }
}
