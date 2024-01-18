import { Injectable } from '@angular/core';
import { environment } from 'src/app/environments/environment';
import { IUsuarioResponse } from '../interfaces/usuario.interface';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  private API_URL = environment.apiUrl;

  constructor( private http : HttpClient) { }


  getUsuarioInfo (id : number) : Observable<IUsuarioResponse> {
    return this.http.get<IUsuarioResponse>(`${this.API_URL}/Users/usuarioPorId/${id}`);
  }


}
