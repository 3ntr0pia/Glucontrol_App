import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormComponent } from './components/form/form.component';
import { FormsModule } from '@angular/forms';
import { MainComponent } from './pages/main/main.component';
import { InfoComponent } from './components/info/info.component';

@NgModule({
  declarations: [FormComponent, MainComponent, InfoComponent],
  imports: [CommonModule, FormsModule],
  exports: [MainComponent],
})
export class LoginModule {}
