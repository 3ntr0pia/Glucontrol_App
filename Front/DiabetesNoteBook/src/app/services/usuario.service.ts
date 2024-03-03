import { Injectable } from '@angular/core';
import { environment } from 'src/app/environments/environment';
import { IUsuarioUpdate } from '../interfaces/usuario.interface';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  private API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  actualizarUsuario(usuario: IUsuarioUpdate): Observable<IUsuarioUpdate> {
    return this.http.put<IUsuarioUpdate>(
      `${this.API_URL}/Users/cambiardatosusuario`,
      usuario
    );
  }

  cambiarPass(data: { id: number; NewPass: string }): Observable<string> {
    return this.http.put(
      `${this.API_URL}/ChangePasswordControllers/changePassword`,
      data,
      { responseType: 'text' }
    );
  }
}
