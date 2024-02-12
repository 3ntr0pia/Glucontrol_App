import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { IRegister } from '../interfaces/register.interface';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { IFinalRegister } from '../interfaces/finalregister.interface';
import {
  ILogin,
  IUserLoginResponse,
} from '../interfaces/loginResponse.interface';

@Injectable({
  providedIn: 'root',
})
export class AuthServiceService {
  private API_URL = environment.apiUrl;
  //Esta variable es para guardar el usuario que se logeara
  private currentUserSubject: BehaviorSubject<IUserLoginResponse | null>;
  //Y con esta obtenemos el valor del usuario que se ha logueado ( es decir, sus datos)
  public user: Observable<IUserLoginResponse | null>;

  constructor(private http: HttpClient) {
    //Esta linea es para guardar el usuario que se ha logueado en el localstorage del navegador
    this.currentUserSubject = new BehaviorSubject<IUserLoginResponse | null>(
      JSON.parse(localStorage.getItem('user') || '{}')
    );
    // Un observable es un objeto que emite notificaciones cuando cambia el valor de una propiedad
    //Con esto podemos obtener el usuario que se ha logueado
    this.user = this.currentUserSubject.asObservable();
  }

  //Metodo para acceder al valor del usuario que se ha logueado
  public get userValue(): IUserLoginResponse | null {
    return this.currentUserSubject.value;
  }

  registerUser(datoRegistro: IFinalRegister): Observable<IFinalRegister> {
    return this.http.post<IFinalRegister>(
      `${this.API_URL}/Users/registro`,
      datoRegistro
    );
  }

  loginUser(datoLogin: ILogin): Observable<IUserLoginResponse> {
    return (
      this.http
        .post<IUserLoginResponse>(`${this.API_URL}/Users/login`, datoLogin)
        //Dentro del pipe, con map, podemos modificar el valor que devuelve el observable , con el fin de guardarlo en el localstorage
        .pipe(
          map((user: IUserLoginResponse) => {
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSubject.next(user);
            return user;
          })
        )
    );
  }
  //Esto recarga los datos del localstorage, por si se ha modificado algun dato del usuario
  updateUser(user: IUserLoginResponse): void {
    localStorage.setItem('user', JSON.stringify(user));
    //Con next podemos emitir un nuevo valor, en este caso el usuario que se ha modificado
    this.currentUserSubject.next(user);
  }

  //Metodo para cerrar sesion
  logoutUser(): void {
    //Eliminamos el usuario del localstorage
    localStorage.removeItem('user');
    //Con next podemos emitir un nuevo valor, en este caso null, por que el usuario se ha deslogueado
    this.currentUserSubject.next(null);
  }
}
