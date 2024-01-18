import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from './modal/modal.component';
import { CalculadoraImcComponent } from './calculadora-imc/calculadora-imc.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [ModalComponent, CalculadoraImcComponent],
  imports: [CommonModule , FormsModule],
  exports: [ModalComponent,CalculadoraImcComponent],
})
export class SharedModule {}
