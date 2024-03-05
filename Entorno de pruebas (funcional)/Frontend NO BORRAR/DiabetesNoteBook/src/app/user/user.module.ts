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
import { SharedModule } from '../shared/shared.module';
import { VademecumComponent } from './components/vademecum/vademecum.component';
import { FiltroGenericosPipe } from '../pipes/filtro-genericos.pipe';
import { FiltroSinRecetaPipe } from '../pipes/filtro-sin-receta.pipe';

@NgModule({
  declarations: [
    MainComponent,
    NavbarComponent,
    MisDatosComponent,
    NuevoRegistroComponent,
    RegistrosAnterioresComponent,
    AyudaComponent,
    VademecumComponent,
    FiltroGenericosPipe,
    FiltroSinRecetaPipe,
  ],
  imports: [CommonModule, UserRoutingModule, FormsModule, SharedModule],
  exports: [MainComponent],
})
export class UserModule {}
