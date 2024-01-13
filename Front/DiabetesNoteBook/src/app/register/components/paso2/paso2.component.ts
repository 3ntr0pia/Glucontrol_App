import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Sexo, Actividad, TipoDiabetes } from '../../interfaces/register.enum';
import { IRegister } from '../../interfaces/register.interface';

@Component({
  selector: 'register-paso2',
  templateUrl: './paso2.component.html',
  styleUrls: ['./paso2.component.css']
})
export class Paso2Component {

  @Output() siguientePaso = new EventEmitter<IRegister>();
  public Sexo = Sexo;
  public Actividad = Actividad;
  public TipoDiabetes = TipoDiabetes;
  
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
      sexo : this.Sexo.hombre,
      actividad : this.Actividad.sedentario,
      tipoDiabetes : {
        tipo : this.TipoDiabetes.tipo1,
        
        medicacion: "",
        insulina: false
      }
    }
  }

  get imc(): number {
   let calculo = this.calcularIMC();
   if(calculo == Infinity || Number.isNaN(calculo)){
    return 0;
   }else{
    return Math.round(calculo);
   }
   
  }
  
  obtenerIMC(): string {
    const imc = this.calcularIMC();
    if (imc < 18.5) {
      return 'assets/figures/person_thin.png';
    } else if (imc >= 18.5 && imc < 28) {
      return 'assets/figures/person_normal.png';
    }else if(imc >= 28 && imc < 38){
      return 'assets/figures/person_fat.png';
    } else if (imc >= 38) {
      return 'assets/figures/person_ob.png';
    } else{
      return 'assets/figures/person_normal.png';
    }
  }
  imcColor(): string {
    const imc = this.calcularIMC();
    if (imc < 18.5) {
      return 'red';
    } else if (imc >= 18.5 && imc < 28) {
      return 'green';
    }else if(imc >= 28 && imc < 38){
      return 'yellow';
    } else if (imc >= 38) {
      return 'red';
    } else{
      return 'text-success';
    }
  }

  calcularIMC(): number {
    let alturaEnMetros = this.datosRegistro.mediciones.altura / 100;
    return this.datosRegistro.mediciones.peso / Math.pow(alturaEnMetros, 2);
  }

  formularioInvalido(): boolean {
    return !this.datosRegistro.mediciones.edad ||
           !this.datosRegistro.mediciones.peso ||
           !this.datosRegistro.mediciones.altura ||
           !this.datosRegistro.mediciones.sexo ||
           !this.datosRegistro.mediciones.actividad ||
           !this.datosRegistro.mediciones.tipoDiabetes.tipo;
  }
}
