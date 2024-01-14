import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormComponent } from './components/form/form.component';
import { FormsModule } from '@angular/forms';
import { InfoComponent } from './components/info/info.component';
import { LoginMainComponent } from './pages/main/login-main.component';
import { LoginRoutingModule } from './login-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { RecordarComponent } from './components/recordar/recordar.component';


@NgModule({
  declarations: [FormComponent, InfoComponent,LoginMainComponent, RecordarComponent],
  imports: [CommonModule, FormsModule, LoginRoutingModule,HttpClientModule],
  exports: [LoginMainComponent],
})
export class LoginModule {}
