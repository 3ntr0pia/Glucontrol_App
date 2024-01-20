import { Injectable } from '@angular/core';
import { environment } from 'src/app/environments/environment';
import { IUsuarioUpdate } from '../interfaces/usuario.interface';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  private API_URL = environment.apiUrl;

  constructor( private http : HttpClient) { }


  getUsuarioYPersonaInfo(idUsuario: number, idPersona: number): Observable<[IUsuarioUpdate, IUsuarioUpdate]> {
    let usuario = this.http.get<IUsuarioUpdate>(`${this.API_URL}/Users/usuarioPorId/${idUsuario}`);
    let persona = this.http.get<IUsuarioUpdate>(`${this.API_URL}/Person/personaPorId/${idPersona}`);
    //Con forkJoin podemos hacer varias peticiones http a la vez y juntarlo en un solo observable
    return forkJoin([usuario, persona]); //El forkJoin devuelve un array con los resultados de las peticiones, por que espera que todas las peticiones se completen
  }

  actualizarUsuario (usuario : IUsuarioUpdate) : Observable<IUsuarioUpdate> {
    return this.http.put<IUsuarioUpdate>(`${this.API_URL}/Users/cambiardatosusuarioypersona`, usuario);
  }

}
