import { HttpClient } from '@angular/common/http';
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

  postMediciones (medicion : IMedicionesAzucar) : Observable<IMedicionesAzucar> {
    return this.http.post<IMedicionesAzucar>(`${this.API_URL}/Mediciones`, medicion);
  }

}
