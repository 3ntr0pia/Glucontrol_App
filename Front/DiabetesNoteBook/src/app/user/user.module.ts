import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './pages/main/main.component';
import { NavbarComponent } from './global/navbar/navbar.component';

import { FormsModule } from '@angular/forms';
import { UserRoutingModule } from './user-routing.module';
import { MisDatosComponent } from './components/mis-datos/mis-datos.component';
import { NuevoRegistroComponent } from './components/nuevo-registro/nuevo-registro.component';
import { RegistrosAnterioresComponent } from './components/registros-anteriores/registros-anteriores.component';
import { AyudaComponent } from './components/ayuda/ayuda.component';

@NgModule({
  declarations: [MainComponent, NavbarComponent, MisDatosComponent, NuevoRegistroComponent, RegistrosAnterioresComponent, AyudaComponent],
  imports: [CommonModule, UserRoutingModule,FormsModule],
  exports: [MainComponent],
})
export class UserModule {}
