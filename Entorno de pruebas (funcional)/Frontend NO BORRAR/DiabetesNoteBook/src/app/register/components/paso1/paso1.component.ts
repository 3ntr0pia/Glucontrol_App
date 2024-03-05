import { Component, EventEmitter, Output, Input } from '@angular/core';
import { Sexo, Actividad, TipoDiabetes } from '../../../enums/register.enum';
import { IRegister } from '../../../interfaces/register.interface';
import { AvatarService } from 'src/app/services/avatar.service';

@Component({
  selector: 'register-paso1',
  templateUrl: './paso1.component.html',
  styleUrls: ['./paso1.component.css'],
})
export class Paso1Component {
  @Output() siguientePaso = new EventEmitter<IRegister>();

  @Input() datosRegistro: IRegister = {
    username: '',
    avatar: '',
    nombre: '',
    apellido: '',
    apellido2: '',
    email: '',
    password: '',
    password2: '',
    mediciones: {
      edad: 0,
      peso: 0,
      altura: 0,
      sexo: Sexo.hombre,
      actividad: Actividad.sedentario,
      tipoDiabetes: {
        tipo: TipoDiabetes.tipo1,
        medicacion: [],
        insulina: false,
      },
    },
  };

  password2: string = '';
  avatar: string = '';
  hasError: boolean = false;
  defaultAvatar: string = 'assets/avatar.png';

  constructor(private avatarService: AvatarService) {}

  generarAvatar() {
    this.datosRegistro.avatar = this.avatarService.getRandomAvatar();
  }

  formularioInvalido(): boolean {
    return (
      !this.datosRegistro.nombre ||
      !this.datosRegistro.username ||
      !this.datosRegistro.apellido ||
      !this.datosRegistro.apellido2 ||
      !this.datosRegistro.email ||
      !this.datosRegistro.password ||
      !this.datosRegistro.avatar
    );
  }

  

  validarPassword(password: string): boolean {
    const patron =
      /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
    return patron.test(password);
  }
  validarUsuario(usuario: string): boolean {
    const patronUsuario = /^[A-Za-z0-9_-]{6,18}$/;
    return patronUsuario.test(usuario);
  }
  validarEmail(email: string): boolean {
    const patronEmail = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    return patronEmail.test(email);
  }
  setAvatar(avatar: string): void {
    this.datosRegistro.avatar = avatar;
  }
}
