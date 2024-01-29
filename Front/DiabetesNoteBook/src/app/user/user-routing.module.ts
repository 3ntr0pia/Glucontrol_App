import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from './pages/main/main.component';
import { MisDatosComponent } from './components/mis-datos/mis-datos.component';
import { AyudaComponent } from './components/ayuda/ayuda.component';
import { VademecumComponent } from './components/vademecum/vademecum.component';
import { MedicionesComponent } from './components/mediciones/mediciones.component';



const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      { path: '', redirectTo: 'mis-datos', pathMatch: 'full' },
      { path: 'mis-datos', component: MisDatosComponent },
      { path : 'mediciones', component: MedicionesComponent },
      { path: 'vademecum', component: VademecumComponent },
      { path: 'ayuda', component: AyudaComponent },
      
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
