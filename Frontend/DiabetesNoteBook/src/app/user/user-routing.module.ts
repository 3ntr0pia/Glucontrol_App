import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from './pages/main/main.component';
import { MisDatosComponent } from './components/mis-datos/mis-datos.component';
import { AyudaComponent } from './components/ayuda/ayuda.component';
import { VademecumComponent } from './components/vademecum/vademecum.component';
import { MedicionesComponent } from './components/mediciones/mediciones.component';
import { AuthGuard } from '../guard/auth.guard';



const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'mis-datos', pathMatch: 'full' },
      { path: 'mis-datos', component: MisDatosComponent , title: 'Mis datos | Glucontrol '},
      { path : 'mediciones', component: MedicionesComponent , title: 'Mediciones | Glucontrol '},
      { path: 'vademecum', component: VademecumComponent , title: 'Vademecum | Glucontrol '},
      { path: 'ayuda', component: AyudaComponent , title: 'Ayuda | Glucontrol '},
      
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
