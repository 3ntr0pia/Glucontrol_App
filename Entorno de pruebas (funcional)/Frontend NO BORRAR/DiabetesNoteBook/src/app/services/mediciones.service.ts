import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { IMedicionesAzucar } from '../interfaces/mediciones.interface';

@Injectable({
  providedIn: 'root',
})
export class MedicionesService {
  private API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // getMediciones(userId: number): Observable<IMedicionesAzucar[]> {
  //   return this.http.get<IMedicionesAzucar[]>(
  //     `${this.API_URL}/Mediciones/getmedicionesporidusuario/${userId}`
  //   );
  // }
  getMediciones(userId: number): Observable<IMedicionesAzucar[]> {
    const token = localStorage.getItem('token');
    console.log('esto es el token: ' + token);
    // Verifica si el token está presente en el localStorage

    // Configura el encabezado de autorización con el token
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    // Realiza la solicitud HTTP con el encabezado de autorización
    return this.http.get<IMedicionesAzucar[]>(
      `${this.API_URL}/Mediciones/getmedicionesporidusuario/${userId}`,
      { headers: headers }
    );
  }
  descargarMedicionesPDF(): Observable<Blob> {
    const token = localStorage.getItem('token');
    console.log('esto es el token: ' + token);
    // Verifica si el token está presente en el localStorage

    // Configura el encabezado de autorización con el token
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.get(`${this.API_URL}/Mediciones/descargarMedicionesPDF`, {
      responseType: 'blob',
      headers: headers,
    });
  }
  enviarMediciones(): Observable<any> {
    const token = localStorage.getItem('token');

    // Configura el encabezado de autorización con el token
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http.get<any>(`${this.API_URL}/Mediciones/send-mediciones`, {
      headers,
    });
  }

  postMediciones(medicion: IMedicionesAzucar): Observable<any> {
    const token = localStorage.getItem('token');
    console.log('esto es el token: ' + token);
    // Verifica si el token está presente en el localStorage

    // Configura el encabezado de autorización con el token
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.post<any>(`${this.API_URL}/Mediciones`, medicion, {
      headers: headers,
      responseType: 'text' as 'json',
    });
  }

  deleteMediciones(idMedicion: number): Observable<any> {
    const token = localStorage.getItem('token');
    console.log('esto es el token: ' + token);
    // Verifica si el token está presente en el localStorage

    // Configura el encabezado de autorización con el token
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: {
        id: idMedicion,
      },
    };

    return this.http.delete<any>(
      `${this.API_URL}/Mediciones/eliminarmedicion`,
      { ...options, responseType: 'text' as 'json', headers: headers }
    );
  }
}
