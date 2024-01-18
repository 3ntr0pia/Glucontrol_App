import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Chart } from 'chart.js';
import { UsuarioService } from '../../services/usuario.service';
import { AuthServiceService } from 'src/app/services/auth-service.service';
import { IUserLoginResponse } from 'src/app/interfaces/loginResponse.interface';
import { IUsuarioUpdate } from '../../interfaces/usuario.interface';

@Component({
  selector: 'app-mis-datos',
  templateUrl: './mis-datos.component.html',
  styleUrls: ['./mis-datos.component.css'],
})
export class MisDatosComponent implements OnInit {
  usuarioLogeado: IUserLoginResponse | null = null;
  usuario: IUsuarioUpdate = {
    id: 0,
    avatar: '',
    userName: '',
    nombre: '',
    primerApellido: '',
    segundoApellido: '',
    sexo: '',
    edad: 0,
    peso: 0,
    altura: 0,
    actividad: '',
    tipoDiabetes: '',
    medicacion: [],
    insulina: false,
  };

  nuevoAvatar: string = '';

  constructor(
    private usuarioService: UsuarioService,
    private authService: AuthServiceService
  ) {}

  ngOnInit(): void {
    this.authService.user.subscribe((user) => {
      this.usuarioLogeado = user;
      if (this.usuarioLogeado) {
        this.getUsuarioInfo(this.usuarioLogeado.id);
      }
    });
  }

  setAvatar(avatar: string): void {
    this.nuevoAvatar = avatar;
    console.log(this.nuevoAvatar);
  }

  getUsuarioInfo(usuarioId: number): void {
    this.usuarioService.getUsuarioInfo(usuarioId).subscribe({
      next: (res) => {
        this.usuario = res;
        console.log(res);
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  actualizarUsuario(): void {
    this.usuario.avatar = this.nuevoAvatar;
    this.usuarioService.actualizarUsuario(this.usuario).subscribe({
      next: (res) => {
        console.log(res);
      },
      error: (err) => {
        console.error(err);
      },
    });
  }
}
