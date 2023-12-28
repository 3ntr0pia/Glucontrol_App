import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterMainComponent } from './pages/main/register-main.component';
import { RegisterRoutingModule } from './register-routing.module';



@NgModule({
  declarations: [RegisterMainComponent],
  imports: [CommonModule, RegisterRoutingModule],
  exports: [RegisterMainComponent],
})
export class RegisterModule {}
