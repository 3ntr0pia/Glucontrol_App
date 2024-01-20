import {
  AfterViewInit,
  Component,
  ElementRef,
  OnChanges,
  OnInit,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { Chart } from 'chart.js';
import { UsuarioService } from '../../services/usuario.service';
import { AuthServiceService } from 'src/app/services/auth-service.service';
import { IUserLoginResponse } from 'src/app/interfaces/loginResponse.interface';
import { IUsuarioUpdate } from '../../interfaces/usuario.interface';
import { Actividad, Sexo, TipoDiabetes } from 'src/app/interfaces/register.enum';

@Component({
  selector: 'app-mis-datos',
  templateUrl: './mis-datos.component.html',
  styleUrls: ['./mis-datos.component.css'],
})
export class MisDatosComponent implements OnInit {

  public Sexo = Sexo;
  public Actividad = Actividad;
  public TipoDiabetes = TipoDiabetes;

  usuarioLogeado: IUserLoginResponse | null = null;
  usuario: IUsuarioUpdate = {
    id: 0,
    avatar: '',
    userName: '',
    nombre: '',
    primerApellido: '',
    segundoApellido: '',
    sexo: Sexo.hombre,
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
        this.getUsuarioInfo(this.usuarioLogeado.id, this.usuarioLogeado.idPersona);
      }
    });
  }

  setAvatar(avatar: string): void {
    this.nuevoAvatar = avatar;
    console.log(this.nuevoAvatar);
  }

  getUsuarioInfo(usuarioId: number , personaPorId : number): void {
    this.usuarioService.getUsuarioYPersonaInfo(usuarioId, personaPorId).subscribe({
      next: (res) => {
        //Con spread (...) podemos copiar los valores de un objeto a otro y mezclarlos en el usuario
        this.usuario = {...res[0], ...res[1]};
       
        this.usuario.medicacion = ['Lorazepam', 'Paracetamol'];
        console.log(this.usuario);
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
        console.log('Usuario actualizado:', res);
      },
      error: (err) => {
        console.error(err);
      },
    });
  }
}
