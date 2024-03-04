import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable, catchError, throwError } from 'rxjs';
import { IMedicionesAzucar } from '../interfaces/mediciones.interface';
import { IUserLoginResponse } from '../interfaces/loginResponse.interface';

@Injectable({
  providedIn: 'root',
})
export class MedicionesService {
  private API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getMediciones(userId: number): Observable<IMedicionesAzucar[]> {
    return this.http.get<IMedicionesAzucar[]>(
      `${this.API_URL}/Mediciones/getmedicionesporidusuario/${userId}`
    );
  }

  postMediciones(medicion: IMedicionesAzucar): Observable<string> {
    // El backend devuelve texto plano y hay que indicarlo en el `responseType: 'text'`. IMPORTANTE: el argumento genérico (`<string>`) en el método `post` solo se puede emplear para `responseType: 'json'` (valor por defecto).
    return this.http
      .post(`${this.API_URL}/Mediciones`, medicion, {
        responseType: 'text',
      })
      .pipe(catchError(this.handleError));
  }

  deleteMediciones(idMedicion: number): Observable<any> {
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
      { ...options, responseType: 'text' as 'json' }
    );
  }

  private handleError(err: HttpErrorResponse): Observable<never> {
    let errorMsg = '';

    if (err.error instanceof ErrorEvent) {
      // Client-side or network error.
      errorMsg = `An error ocurred: ${err.error.message}`;
    } else {
      // Unsuccessful response code. Body could contain additional information.
      errorMsg = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }

    console.error(errorMsg);
    return throwError(() => errorMsg);
  }
}
