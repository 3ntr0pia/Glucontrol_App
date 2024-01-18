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


  getUsuarioYPersonaInfo (idUsuario : number , idPersona : number) : Observable<IUsuarioUpdate> {
    let usuario = this.http.get<IUsuarioUpdate>(`${this.API_URL}/Users/usuarioPorId/${idUsuario}`);
    let persona = this.http.get<IUsuarioUpdate>(`${this.API_URL}/Users/personaPorId/${idPersona}`);
    return usuario && persona;
  }

  actualizarUsuario (usuario : IUsuarioUpdate) : Observable<IUsuarioUpdate> {
    return this.http.put<IUsuarioUpdate>(`${this.API_URL}/Users/cambiardatosusuarioypersona`, usuario);
  }

}
