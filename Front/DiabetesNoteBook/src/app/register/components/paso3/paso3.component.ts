import { Component, EventEmitter, Output } from '@angular/core';
import { IRegister } from '../../interfaces/register.interface';

@Component({
  selector: 'register-paso3',
  templateUrl: './paso3.component.html',
  styleUrls: ['./paso3.component.css']
})
export class Paso3Component {
  @Output() siguientePaso = new EventEmitter<IRegister>();

  datosRegistro! : IRegister ;
}
