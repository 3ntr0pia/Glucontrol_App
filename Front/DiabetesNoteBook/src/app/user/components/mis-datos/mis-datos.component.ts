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
import { UsuarioService } from '../../../services/usuario.service';
import { AuthServiceService } from 'src/app/services/auth-service.service';
import { IUserLoginResponse } from 'src/app/interfaces/loginResponse.interface';
import { IUsuarioUpdate } from '../../../interfaces/usuario.interface';
import { Sexo, Actividad, TipoDiabetes } from 'src/app/enums/register.enum';


@Component({
  selector: 'app-mis-datos',
  templateUrl: './mis-datos.component.html',
  styleUrls: ['./mis-datos.component.css'],
})
export class MisDatosComponent implements OnInit {
  public Sexo = Sexo;
  public Actividad = Actividad;
  public TipoDiabetes = TipoDiabetes;
  public nuevaAltura = 0;
  public nuevoPeso = 0;

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

  error: string = '';

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
    this.usuarioService.getUsuarioYPersonaInfo(usuarioId).subscribe({
      next: (res) => {
        // 0 seria el usuario y 1 la persona
        this.usuario = {
          id: res[0].id,
          avatar: res[0].avatar,
          userName: res[0].userName,
          nombre: res[1].nombre,
          primerApellido: res[1].primerApellido,
          segundoApellido: res[1].segundoApellido,
          sexo: res[1].sexo,
          edad: res[1].edad,
          peso: res[1].peso,
          altura: res[1].altura,
          actividad: res[1].actividad,
          tipoDiabetes: res[1].tipoDiabetes,
          medicacion: res[1].medicacion,
          insulina: res[1].insulina,
        };
        //Esto se puede hacer tambien con el operador spread pero no seria tan preciso
        console.log(this.usuario.id);
        this.usuario.medicacion = ['Lorazepam', 'Paracetamol'];
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  actualizarUsuario(): void {
    if (this.nuevoAvatar !== '') {
      this.usuario.avatar = this.nuevoAvatar;
    }
    this.usuarioService.actualizarUsuario(this.usuario).subscribe({
      next: (res) => {
        console.log('Usuario actualizado:');
        //Cuando se actualiza el usuario, se actualiza el usuario logeado
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  private validarFormulario(usuario: IUsuarioUpdate): boolean {
    return (
      usuario.nombre.trim() !== '' &&
      usuario.userName.trim() !== '' &&
      usuario.primerApellido.trim() !== '' &&
      usuario.segundoApellido.trim() !== '' &&
      usuario.edad > 0 &&
      usuario.actividad !== '' &&
      usuario.tipoDiabetes !== ''
    );
  }
}
