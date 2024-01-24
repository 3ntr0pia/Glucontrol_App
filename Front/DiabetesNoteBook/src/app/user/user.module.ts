// Modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserRoutingModule } from './user-routing.module';
import { SharedModule } from '../shared/shared.module';

//Components
import { VademecumComponent } from './components/vademecum/vademecum.component';
import { MedicionesComponent } from './components/mediciones/mediciones.component';
import { MisDatosComponent } from './components/mis-datos/mis-datos.component';
import { AyudaComponent } from './components/ayuda/ayuda.component';
import { MainComponent } from './pages/main/main.component';
import { NavbarComponent } from './global/navbar/navbar.component';

// Pipes
import { FiltroGenericosPipe } from '../pipes/filtro-genericos.pipe';
import { FiltroSinRecetaPipe } from '../pipes/filtro-sin-receta.pipe';
import { NgxEchartsModule } from 'ngx-echarts';

@NgModule({
  declarations: [
    MainComponent,
    NavbarComponent,
    MisDatosComponent,

    AyudaComponent,
    VademecumComponent,
    FiltroGenericosPipe,
    FiltroSinRecetaPipe,

    MedicionesComponent,
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    FormsModule,
    SharedModule,
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts'),
    }),
  ],
  exports: [MainComponent],
})
export class UserModule {}
