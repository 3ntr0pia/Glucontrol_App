import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormComponent } from './components/form/form.component';
import { FormsModule } from '@angular/forms';
import { InfoComponent } from './components/info/info.component';
import { MainComponent } from './pages/main/main.component';

@NgModule({
  declarations: [FormComponent, InfoComponent, MainComponent],
  imports: [CommonModule, FormsModule],
  exports: [MainComponent],
})
export class LoginModule {}
