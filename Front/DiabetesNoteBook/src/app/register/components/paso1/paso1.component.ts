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
        
        medicacion: [],
        insulina: false
      }
    }
  };

  formularioInvalido(): boolean {
    return !this.datosRegistro.nombre ||
           !this.datosRegistro.apellido ||
           !this.datosRegistro.apellido2 ||
           !this.datosRegistro.email ||
           !this.datosRegistro.password ||
           !this.datosRegistro.password2 ||
           this.datosRegistro.password !== this.datosRegistro.password2;
  }
 
  constructor(private avatarService: AvatarService) { }
  avatar : string = '';
  hasError:boolean = false;

  generarAvatar() {
    const randomSeed = Math.random().toString(36).substring(7);
    const sprites = 'miniavs'; 
    this.avatar = this.avatarService.getAvatar(randomSeed, sprites);
  }
  
}
