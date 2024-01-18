import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { IMedicamento } from '../user/interfaces/medicamento.interface';

@Injectable({
  providedIn: 'root',
})
export class VademecumService {
  connectionString: string =
    'https://cima.aemps.es/cima/rest/medicamentos?nombre=';
  private API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getMedicamentoInfo(nombre: string) : Observable<IMedicamento> {
    return this.http.get<IMedicamento>(this.connectionString + nombre);
  }

  getMedicamentoUser(id: string) {
    return this.http.get(`${this.API_URL}/Medicamentos/${id}`);
  }
}
