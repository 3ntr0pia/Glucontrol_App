import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from './modal/modal.component';
import { CalculadoraImcComponent } from './calculadora-imc/calculadora-imc.component';
import { FormsModule } from '@angular/forms';
import { GeneradorAvatarComponent } from './generador-avatar/generador-avatar.component';

@NgModule({
  declarations: [ModalComponent, CalculadoraImcComponent,GeneradorAvatarComponent],
  imports: [CommonModule , FormsModule],
  exports: [ModalComponent,CalculadoraImcComponent,GeneradorAvatarComponent],
})
export class SharedModule {}
