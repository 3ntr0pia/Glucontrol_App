import { Injectable } from '@angular/core';
import { environment } from 'src/app/environments/environment';
import { IUsuarioUpdate } from '../interfaces/usuario.interface';
import { BehaviorSubject, Observable, catchError } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { forkJoin } from 'rxjs';
import { map } from 'rxjs/operators'; // Importamos el operador map
@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  private API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // getUsuarioYPersonaInfo(
  //   idUsuario: number
  // ): Observable<[IUsuarioUpdate, IUsuarioUpdate]> {
  //   let usuario = this.http.get<IUsuarioUpdate>(
  //     `${this.API_URL}/Users/usuarioPorId/${idUsuario}`
  //   );
  //   let persona = this.http.get<IUsuarioUpdate>(
  //     `${this.API_URL}/Person/personaPorId/${idUsuario}`
  //   );
  //   //Con forkJoin podemos hacer varias peticiones http a la vez y juntarlo en un solo observable
  //   return forkJoin([usuario, persona]); //El forkJoin devuelve un array con los resultados de las peticiones, por que espera que todas las peticiones se completen
  // }
  getUserById(userId: number): Observable<IUsuarioUpdate> {
    const token = localStorage.getItem('token');
    console.log('esto es el token: ' + token);
    // Verifica si el token está presente en el localStorage

    // Configura el encabezado de autorización con el token
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    });
    return this.http.get<IUsuarioUpdate>(
      `${this.API_URL}/Users/usuarioPorId/${userId}`,
      { headers: headers }
    );
  }
  // actualizarUsuario(usuario: IUsuarioUpdate): Observable<IUsuarioUpdate> {
  //   const token = localStorage.getItem('token');
  //   console.log('esto es el token: ' + token);
  //   // Verifica si el token está presente en el localStorage

  //   // Configura el encabezado de autorización con el token
  //   const headers = new HttpHeaders({
  //     Authorization: `Bearer ${token}`,
  //   });
  //   return this.http.patch<IUsuarioUpdate>(
  //     `${this.API_URL}/Users/cambiardatosusuario`,
  //     usuario,
  //     { headers: headers }
  //   );
  // }
  actualizarUsuario(usuario: IUsuarioUpdate): Observable<IUsuarioUpdate> {
    const token = localStorage.getItem('token');
    console.log('esto es el token: ' + token);
    // Verifica si el token está presente en el localStorage

    // Configura el encabezado de autorización con el token
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    });

    // Convierte la propiedad medicacion a un array de strings si es necesario
    if (typeof usuario.medicacion === 'string') {
      usuario.medicacion = [usuario.medicacion];
    }

    return this.http
      .patch<IUsuarioUpdate>(
        `${this.API_URL}/Users/cambiardatosusuario`,
        usuario,
        { headers: headers }
      )
      .pipe();
  }

  cambiarPass(data: { id: number; NewPass: string }): Observable<string> {
    return this.http.put(
      `${this.API_URL}/ChangePasswordControllers/changePassword`,
      data,
      { responseType: 'text' }
    );
  }
}
