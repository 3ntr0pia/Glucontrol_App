import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { IRegister } from '../register/interfaces/register.interface';
import { Observable } from 'rxjs';
import { IFinalRegister } from '../register/interfaces/finalregister.interface';
import { ILogin } from '../login/interfaces/login.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {

  private API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  registerUser(datoRegistro: IFinalRegister) : Observable<IFinalRegister> {
    return this.http.post<IFinalRegister>(`${this.API_URL}/Users/registro`, datoRegistro);
  }

  loginUser(datoLogin: ILogin) : Observable<ILogin> {
    return this.http.post<ILogin>(`${this.API_URL}/Users/login`, datoLogin);
  }

}


