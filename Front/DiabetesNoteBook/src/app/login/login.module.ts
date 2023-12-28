import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormComponent } from './components/form/form.component';
import { FormsModule } from '@angular/forms';
import { InfoComponent } from './components/info/info.component';
import { LoginMainComponent } from './pages/main/login-main.component';

@NgModule({
  declarations: [FormComponent, InfoComponent,LoginMainComponent],
  imports: [CommonModule, FormsModule],
  exports: [LoginMainComponent],
})
export class LoginModule {}
