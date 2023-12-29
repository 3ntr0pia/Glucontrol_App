import { Component } from '@angular/core';
import { Actividad, IRegister, Sexo, TipoDiabetes } from '../../interfaces/register.interface';

@Component({
  selector: 'register-main',
  templateUrl: './register-main.component.html',
  styleUrls: ['./register-main.component.css']
})
export class RegisterMainComponent {



  datosRegistro : IRegister = {
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
        fecha_diagnostico : new Date(),
        medicacion: [],
        insulina: false
      }
    }
  };

  paso : number = 1;
  
  siguientePaso(info : IRegister):void{
    //this.datosRegistro = info;  SE CARGA TODO EL OBJETO;
    Object.assign(this.datosRegistro, info); // SE CARGA SOLO LA PARTE QUE SE HA MODIFICADO
    this.paso++;
  }
  pasoAnterior():void{
    this.paso--;
  }
  registrar():void{
    console.log(this.datosRegistro);
  }
  
};
