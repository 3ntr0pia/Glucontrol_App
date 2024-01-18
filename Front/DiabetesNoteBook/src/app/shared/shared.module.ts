import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from './modal/modal.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { SharedRoutingModule } from './shared-routing.module';
import { CalculadoraImcComponent } from './calculadora-imc/calculadora-imc.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [ModalComponent, NotfoundComponent,CalculadoraImcComponent],
  imports: [CommonModule,SharedRoutingModule,FormsModule],
  exports: [ModalComponent,CalculadoraImcComponent],
})
export class SharedModule {}
