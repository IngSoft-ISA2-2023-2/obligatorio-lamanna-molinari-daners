export class StorageManager {

  public saveData(key: string, value: any) {
    localStorage.setItem(key, value);
  }
  
  public getData(key: string) : any {
    return localStorage.getItem(key);
  }
  
  public removeData(key: string) {
    localStorage.removeItem(key);
  }
  
  public deleteData() {
    localStorage.clear();
  }

  public getLogin(){
    let login = this.getData('login');
    if (login) {
      return login; 
    }
    // avoid undefined or ''
    return null;
  }
}
