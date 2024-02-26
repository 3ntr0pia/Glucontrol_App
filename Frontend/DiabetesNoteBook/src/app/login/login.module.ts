import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormComponent } from './components/form/form.component';

import { InfoComponent } from './components/info/info.component';
import { LoginMainComponent } from './pages/main/login-main.component';
import { LoginRoutingModule } from './login-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '../shared/shared.module';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [FormComponent, InfoComponent, LoginMainComponent],
  imports: [
    CommonModule,
    FormsModule,
    LoginRoutingModule,
    HttpClientModule,
    SharedModule,
  ],
  exports: [LoginMainComponent],
})
export class LoginModule {}
