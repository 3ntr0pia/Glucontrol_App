import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormComponent } from './components/form/form.component';
import { FormsModule } from '@angular/forms';
import { InfoComponent } from './components/info/info.component';
import { LoginMainComponent } from './pages/main/login-main.component';
import { LoginRoutingModule } from './login-routing.module';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [FormComponent, InfoComponent,LoginMainComponent],
  imports: [CommonModule, FormsModule, LoginRoutingModule,HttpClientModule],
  exports: [LoginMainComponent],
})
export class LoginModule {}
