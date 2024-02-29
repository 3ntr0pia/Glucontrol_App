import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable, map } from 'rxjs';
import {
  IMedicamento,
  IRespuestaServicio,
} from '../interfaces/medicamento.interface';

@Injectable({
  providedIn: 'root',
})
export class VademecumService {
  connectionString: string =
    'https://cima.aemps.es/cima/rest/medicamentos?nombre=';
  private API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getMedicamentoInfo(nombre: string): Observable<IRespuestaServicio> {
    return this.http.get<IRespuestaServicio>(
      `${this.connectionString}${nombre}`
    );
  }
}
