import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
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
    const body = { token: recover.token, newPass: recover.newPass };
    return this.http.post(
      `${this.API_URL}/ChangePasswordControllers/changePasswordMailConEnlace`,
      body
    );
  }
}
