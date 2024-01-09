import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterMainComponent } from './pages/main/register-main.component';
import { RegisterRoutingModule } from './register-routing.module';
import { Paso1Component } from './components/paso1/paso1.component';
import { FormsModule } from '@angular/forms';
import { Paso2Component } from './components/paso2/paso2.component';
import { Paso3Component } from './components/paso3/paso3.component';

@NgModule({
  declarations: [RegisterMainComponent, Paso1Component, Paso2Component, Paso3Component],
  imports: [CommonModule, RegisterRoutingModule, FormsModule],
  exports: [RegisterMainComponent],
})
export class RegisterModule {}
