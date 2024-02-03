import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { IRegister } from '../../../interfaces/register.interface';
import { Sexo, Actividad, TipoDiabetes } from '../../../enums/register.enum';
import { IFinalRegister } from '../../../interfaces/finalregister.interface';

@Component({
  selector: 'register-paso3',
  templateUrl: './paso3.component.html',
  styleUrls: ['./paso3.component.css'],
})
export class Paso3Component {
  @Output() registrar = new EventEmitter<IFinalRegister>();
  @Output() retroceder = new EventEmitter<void>();
  public Sexo = Sexo;
  public Actividad = Actividad;
  public TipoDiabetes = TipoDiabetes;
  public medicacion: string = '';
  public aceptar: boolean = false;
  public medicacionString: string = '';

  @Input() datosRegistro: IRegister = {
    avatar: '',
    username: '',
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
        medicacion: [],
        insulina: false,
      },
    },
  };

  @Input()  error : string = '';

  registroFinal: IFinalRegister = {
    avatar: '',
    userName: '',
    email: '',
    password: '',
    nombre: '',
    primerApellido: '',
    segundoApellido: '',
    sexo: '',
    edad: 0,
    peso: 0,
    altura: 0,
    actividad: '',
    tipoDiabetes: '',
    medicacion: [],
    insulina: false,
  };

  registroUsuario() {
    this.addMedicacion();
    this.registroFinal = {
      avatar: this.datosRegistro.avatar,
      userName: this.datosRegistro.username,
      email: this.datosRegistro.email,
      password: this.datosRegistro.password,
      nombre: this.datosRegistro.nombre,
      primerApellido: this.datosRegistro.apellido,
      segundoApellido: this.datosRegistro.apellido2,
      sexo: this.datosRegistro.mediciones.sexo,
      edad: this.datosRegistro.mediciones.edad,
      peso: this.datosRegistro.mediciones.peso,
      altura: this.datosRegistro.mediciones.altura,
      actividad: this.datosRegistro.mediciones.actividad,
      tipoDiabetes: this.datosRegistro.mediciones.tipoDiabetes.tipo,
      medicacion: this.datosRegistro.mediciones.tipoDiabetes.medicacion,
      insulina: this.datosRegistro.mediciones.tipoDiabetes.insulina,
    };
    this.registrar.emit(this.registroFinal);
    console.log('registro mandado', this.registroFinal);
  }
  formularioInvalido(): boolean {
    return (
      !this.datosRegistro.mediciones.edad ||
      !this.datosRegistro.mediciones.peso ||
      !this.datosRegistro.mediciones.altura
    );
  }

  addMedicacion() {
    this.medicacionString.split(',').forEach((medicamento) => {
      this.datosRegistro.mediciones.tipoDiabetes.medicacion.push(medicamento);
    });
  }
}
