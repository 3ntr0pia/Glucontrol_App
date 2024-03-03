import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { IMedicacion } from '../interfaces/medicaciones.interface';

@Injectable({
  providedIn: 'root',
})
export class MedicacionService {
  private API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getMedicaciones(userId: number): Observable<IMedicacion> {
    return this.http.get<IMedicacion>(
      `${this.API_URL}/Medicaciones/getMedication/${userId}`
    );
  }

  postMediciones(medicacion: IMedicacion): Observable<string> {
    return this.http
      .post<string>(`${this.API_URL}//Medicaciones/postMedication`, medicacion)
      .pipe(
        catchError((error: any) => {
          console.error('Error en la petición POST:', error);
          return throwError(
            'Algo salió mal al agregar la medicacion. Por favor, intenta de nuevo.'
          );
        })
      );
  }

  deleteMediciones(userId: number, medicacionId: number): Observable<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: {
        id: userId,
        medicacionId: medicacionId,
      },
    };

    return this.http.delete<unknown>(
      `${this.API_URL}/Medicaciones/deleteMedication`,
      { ...options, responseType: 'text' as 'json' }
    );
  }
}
