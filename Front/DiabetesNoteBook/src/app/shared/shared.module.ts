import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from './modal/modal.component';
import { CalculadoraImcComponent } from './calculadora-imc/calculadora-imc.component';
import { FormsModule } from '@angular/forms';
import { GeneradorAvatarComponent } from './generador-avatar/generador-avatar.component';
import { RecoverPassComponent } from './recover-pass/recover-pass.component';
import { SharedRoutingModule } from './shared-routing.module';
import { LoginRoutingModule } from '../login/login-routing.module';
import { LazyLoadImageDirective } from './directives/lazy-load-image.directive';

@NgModule({
  declarations: [
    ModalComponent,
    CalculadoraImcComponent,
    GeneradorAvatarComponent,
    RecoverPassComponent, LazyLoadImageDirective,
  ],
  imports: [CommonModule, FormsModule, SharedRoutingModule, LoginRoutingModule],
  exports: [
    ModalComponent,
    CalculadoraImcComponent,
    GeneradorAvatarComponent,
    RecoverPassComponent,
    LazyLoadImageDirective
  ],
})
export class SharedModule {}
