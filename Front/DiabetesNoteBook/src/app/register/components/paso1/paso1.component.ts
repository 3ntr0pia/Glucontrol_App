import { Component, EventEmitter, Output, Input } from '@angular/core';
import { IRegister } from '../../interfaces/register.interface';

@Component({
  selector: 'register-paso1',
  templateUrl: './paso1.component.html',
  styleUrls: ['./paso1.component.css']
})
export class Paso1Component {

  @Output() siguientePaso = new EventEmitter<IRegister>();

  @Input() datosRegistro!: IRegister;


}
