import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { IMedicionesAzucar } from '../interfaces/mediciones.interface';

@Injectable({
  providedIn: 'root'
})
export class MedicionesService {

  private API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}


  getMediciones( userId : number) : Observable<IMedicionesAzucar[]> {
    return this.http.get<IMedicionesAzucar[]>(`${this.API_URL}/Mediciones/getmedicionesporidusuario/${userId}`);
  }

  postMediciones (medicion : IMedicionesAzucar) : Observable<any> {
    return this.http.post<any>(`${this.API_URL}/Mediciones`, medicion);
  }

  deleteMediciones(idMedicion: number): Observable<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json', 
      }),
      body: {
        id: idMedicion
      },
    };
  
    return this.http.delete<any>(`${this.API_URL}/Mediciones/eliminarmedicion`, options);
  }

}
