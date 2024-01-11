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

  obtenerIMC(): string {
    const imc = this.calcularIMC();
    if (imc < 18.5) {
      return 'assets/figures/person_thin.png';
    } else if (imc >= 18.5 && imc < 25) {
      return 'assets/figures/person_normal.png';
    } else if(imc >= 33) {
      return 'assets/figures/person_fat.png';
    } else{
      return 'assets/figures/person_normal.png';
    }
  }

  calcularIMC(): number {
    let alturaEnMetros = this.datosRegistro.mediciones.altura / 100;
    return this.datosRegistro.mediciones.peso / Math.pow(alturaEnMetros, 2);
  }
}
