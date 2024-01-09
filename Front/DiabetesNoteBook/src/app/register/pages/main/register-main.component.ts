import { Component } from '@angular/core';
import { Sexo, Actividad, TipoDiabetes } from '../../interfaces/register.enum';
import { IRegister } from '../../interfaces/register.interface';

@Component({
  selector: 'register-main',
  templateUrl: './register-main.component.html',
  styleUrls: ['./register-main.component.css']
})
export class RegisterMainComponent {

  public Sexo = Sexo;
  public Actividad = Actividad;
  public TipoDiabetes = TipoDiabetes;
  


  datosRegistro : IRegister = {
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
      sexo : this.Sexo.hombre,
      actividad : this.Actividad.sedentario,
      tipoDiabetes : {
        tipo : this.TipoDiabetes.tipo1,
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
    console.log(this.datosRegistro);
    if(this.paso<3){
      this.paso++;
    }
    
  }
  pasoAnterior():void{
    this.paso--;
  }
  registrar():void{
    console.log(this.datosRegistro);
  }

  
  
};
