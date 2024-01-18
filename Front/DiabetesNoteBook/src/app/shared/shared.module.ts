import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from './modal/modal.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { SharedRoutingModule } from './shared-routing.module';
import { CalculadoraImcComponent } from './calculadora-imc/calculadora-imc.component';
import { FormsModule } from '@angular/forms';
import { GeneradorAvatarComponent } from './generador-avatar/generador-avatar.component';

@NgModule({
  declarations: [ModalComponent, NotfoundComponent,CalculadoraImcComponent, GeneradorAvatarComponent],
  imports: [CommonModule,SharedRoutingModule,FormsModule],
  exports: [ModalComponent,CalculadoraImcComponent,GeneradorAvatarComponent],
})
export class SharedModule {}
