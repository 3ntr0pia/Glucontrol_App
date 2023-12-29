import { Component, EventEmitter, Output } from '@angular/core';
import { IRegister } from '../../interfaces/register.interface';

@Component({
  selector: 'register-paso2',
  templateUrl: './paso2.component.html',
  styleUrls: ['./paso2.component.css']
})
export class Paso2Component {
  @Output() siguientePaso = new EventEmitter<IRegister>();

  datosRegistro! : IRegister ;
}
