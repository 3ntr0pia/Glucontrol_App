import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RecordarPassService {


  private API_URL = environment.apiUrl;
  

  constructor(private http : HttpClient) { }

  recordarPass(email:string){
    const body = { email: email };
    return this.http.post(`${this.API_URL}/ChangePasswordControllers/changePasswordMail`,body);
  }


}
