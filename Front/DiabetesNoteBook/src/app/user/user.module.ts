import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './pages/main/main.component';
import { NavbarComponent } from './global/navbar/navbar.component';

import { FormsModule } from '@angular/forms';
import { UserRoutingModule } from './user-routing.module';

@NgModule({
  declarations: [MainComponent, NavbarComponent],
  imports: [CommonModule, UserRoutingModule,FormsModule],
  exports: [MainComponent],
})
export class UserModule {}
