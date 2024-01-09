import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IRegister } from '../../interfaces/register.interface';
import { Sexo, Actividad, TipoDiabetes } from '../../interfaces/register.enum';

@Component({
  selector: 'register-paso3',
  templateUrl: './paso3.component.html',
  styleUrls: ['./paso3.component.css'],
})
export class Paso3Component {
  @Output() registrar = new EventEmitter<IRegister>();

  public Sexo = Sexo;
  public Actividad = Actividad;
  public TipoDiabetes = TipoDiabetes;

  @Input() datosRegistro: IRegister = {
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
      sexo: this.Sexo.hombre,
      actividad: this.Actividad.sedentario,
      tipoDiabetes: {
        tipo: this.TipoDiabetes.tipo1,
        fecha_diagnostico: new Date(),
        medicacion: [],
        insulina: false,
      },
    },
  };

  formularioInvalido(): boolean {
    return (
      !this.datosRegistro.mediciones.edad ||
      !this.datosRegistro.mediciones.peso ||
      !this.datosRegistro.mediciones.altura
    );
  }

  separarMedicacion(medicacion: string): void {
    medicacion.split(',');
    this.datosRegistro.mediciones.tipoDiabetes.medicacion.push(medicacion);
  }


}
