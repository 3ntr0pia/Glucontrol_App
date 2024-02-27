import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IRecover } from '../interfaces/loginResponse.interface';

@Injectable({
  providedIn: 'root',
})
export class RecordarPassService {
  private API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  recordarPassLogin(email: string) {
    const body = { email: email };
    return this.http.post(
      `${this.API_URL}/ChangePasswordControllers/changePasswordMail`,
      body
    );
  }

  recordarPass(recover: IRecover) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json', 
    });
    const body = {
      Token: recover.token, 
      NewPass: recover.newPass 
    };
    return this.http.post(
      `${this.API_URL}/ChangePasswordControllers/changePasswordMailConEnlace`,
      body,
      { headers: headers ,
        responseType: 'text' //Si la respuesta llega de backend como texto, angular no lo va a parsear a json, por lo que hay que especificar que es texto
      }
    );
  }
}
