import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { IRegister } from '../../interfaces/register.interface';
import { Sexo, Actividad, TipoDiabetes } from '../../interfaces/register.enum';
import { IFinalRegister } from '../../interfaces/finalregister.interface';

@Component({
  selector: 'register-paso3',
  templateUrl: './paso3.component.html',
  styleUrls: ['./paso3.component.css'],
})
export class Paso3Component {
  @Output() registrar = new EventEmitter<IFinalRegister>();

  public Sexo = Sexo;
  public Actividad = Actividad;
  public TipoDiabetes = TipoDiabetes;
  public medicacion : string = '';

  @Input() datosRegistro: IRegister = {
    avatar: '',
    username: '',
    nombre: '',
    apellido: '',
    apellido2: '',
    email: '',
    password: '',
    password2: "",
    mediciones: {
      edad: 0,
      peso: 0,
      altura: 0,
      sexo: this.Sexo.hombre,
      actividad: this.Actividad.sedentario,
      tipoDiabetes: {
        tipo: this.TipoDiabetes.tipo1,
        medicacion:"",
        insulina: false,
      },
    },
  };


  registroFinal: IFinalRegister ={
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
    medicacion: '',
    insulina: false
  }

  registroUsuario() {
    this.registroFinal ={
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
      insulina: this.datosRegistro.mediciones.tipoDiabetes.insulina
    }
    this.registrar.emit(this.registroFinal);
    console.log("registro mandado" , this.registroFinal)
  }
  formularioInvalido(): boolean {
    return (
      !this.datosRegistro.mediciones.edad ||
      !this.datosRegistro.mediciones.peso ||
      !this.datosRegistro.mediciones.altura
    );
  }

//   actualizarMedicacion():void{
//     if(this.datosRegistro.mediciones.tipoDiabetes.medicacion.length < 6){
//     const nuevaMedicacion = this.medicacion.split(',').map(m => m.trim()).filter(m => m.length > 0);
    
//     nuevaMedicacion.forEach(nombre => {
//       const existeMedicacion = this.datosRegistro.mediciones.tipoDiabetes.medicacion.find(m => m.nombre === nombre);
//       if(!existeMedicacion){
//         this.datosRegistro.mediciones.tipoDiabetes.medicacion.push({
//           nombre: nombre,
//           color: this.generarColorAleatorio(),
//           forma: this.generarFormaAleatorio(),
//           rotacion: this.generarRotacionAleatorio()
//         });
//       }
//     });
//     this.datosRegistro.mediciones.tipoDiabetes.medicacion = this.datosRegistro.mediciones.tipoDiabetes.medicacion.filter(medicamento =>
//       nuevaMedicacion.includes(medicamento.nombre)
//     );
    
//     };
//   }
//   generarColorAleatorio(){
//     const colores = ['red', 'blue', 'green', 'yellow', 'orange', 'purple', 'pink', 'black', 'white'];
//     return colores[Math.floor(Math.random() * colores.length)];
//   }

//   generarFormaAleatorio(){
//     const formas = ['pastilla', 'capsula'];
//     return formas[Math.floor(Math.random() * formas.length)];
//   }

//   generarRotacionAleatorio(){
//     const valorRotacion = ["rotate(90deg)", "rotate(180deg)", "rotate(270deg)", "rotate(360deg)", "rotate(450deg)", "rotate(540deg)", "rotate(630deg)", "rotate(720deg)", "rotate(810deg)", "rotate(900deg)", "rotate(990deg)", "rotate(1080deg)", "rotate(1170deg)", "rotate(1260deg)", "rotate(1350deg)", "rotate(1440deg)", "rotate(1530deg)", "rotate(1620deg)", "rotate(1710deg)", "rotate(1800deg)"]
//     return valorRotacion[Math.floor(Math.random() * valorRotacion.length)];

// }

}
