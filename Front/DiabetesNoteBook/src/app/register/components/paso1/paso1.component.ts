import { Component, EventEmitter, Output, Input } from '@angular/core';
import { Sexo, Actividad, TipoDiabetes } from '../../interfaces/register.enum';
import { IRegister } from '../../interfaces/register.interface';
import { AvatarService } from 'src/app/services/avatar.service';

@Component({
  selector: 'register-paso1',
  templateUrl: './paso1.component.html',
  styleUrls: ['./paso1.component.css']
})
export class Paso1Component {

  @Output() siguientePaso = new EventEmitter<IRegister>();

  @Input() datosRegistro : IRegister = {
    username: '',
    avatar: '',
    nombre: "",
    apellido: "",
    apellido2: "",
    email: "",
    password: "",
    password2: "",
    mediciones : {
      edad : 0,
      peso : 0,
      altura : 0,
      sexo : Sexo.hombre,
      actividad : Actividad.sedentario,
      tipoDiabetes : {
        tipo : TipoDiabetes.tipo1,
        medicacion: "",
        insulina: false
      }
    }
  };

  password2 : string = "";
  avatar : string = '';
  hasError:boolean = false;
  defaultAvatar : string ="assets/avatar.png";


  constructor(private avatarService: AvatarService) {}

  generarAvatar() {
    
    this.datosRegistro.avatar = this.avatarService.getRandomAvatar();
    
  }
  

  formularioInvalido(): boolean {
    return !this.datosRegistro.nombre ||
           !this.datosRegistro.apellido ||
           !this.datosRegistro.apellido2 ||
           !this.datosRegistro.email ||
           !this.datosRegistro.password ;
           
  }
  validarPassword(password: string): boolean {
    const minLength = 8;
    const hasUpperCase = /[A-Z]/.test(password);
    const hasLowerCase = /[a-z]/.test(password);
    const hasNumbers = /\d/.test(password);
    const hasSpecialChar = /[\W_]/.test(password);
  
    return password.length >= minLength && hasUpperCase && hasLowerCase && hasNumbers && hasSpecialChar;
  }
  
}
