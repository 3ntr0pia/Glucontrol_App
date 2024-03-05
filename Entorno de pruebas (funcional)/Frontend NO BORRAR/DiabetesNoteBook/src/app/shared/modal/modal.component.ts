import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent {

  @Input() mensaje : string = '';

  @Output() cerrar = new EventEmitter<void>();

  @Input()  icono: string = '';


  cerrarModal(){
    this.cerrar.emit();
  }
  


}
