import { Injectable } from '@angular/core';
import { environment } from 'src/app/environments/environment';
import { IUsuarioUpdate } from '../interfaces/usuario.interface';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  private API_URL = environment.apiUrl;

  constructor( private http : HttpClient) { }


  getUsuarioInfo (id : number) : Observable<IUsuarioUpdate> {
    return this.http.get<IUsuarioUpdate>(`${this.API_URL}/Users/usuarioPorId/${id}`);
  }

  actualizarUsuario (usuario : IUsuarioUpdate) : Observable<IUsuarioUpdate> {
    return this.http.put<IUsuarioUpdate>(`${this.API_URL}/Users/cambiardatosusuarioypersona`, usuario);
  }

}
