import { BehaviorSubject } from 'rxjs';

export class CommonService {
  private headerDataUpdateSource = new BehaviorSubject<any>(0);
  onHeaderDataUpdate = this.headerDataUpdateSource.asObservable();

  private toastDataUpdateSource = new BehaviorSubject<any>({});
  onToastDataUpdate = this.toastDataUpdateSource.asObservable();

  private searchDataUpdateSource = new BehaviorSubject<any>([]);
  onSearchDataUpdate = this.searchDataUpdateSource.asObservable();

  updateHeaderData(message: any) {
    this.headerDataUpdateSource.next(message);
  }

  updateToastData(message: any, type: any, title: any) {
    this.toastDataUpdateSource.next({message, type, title});
  }

  updateSearchData(data: any) {
    this.searchDataUpdateSource.next(data);
  }
}
