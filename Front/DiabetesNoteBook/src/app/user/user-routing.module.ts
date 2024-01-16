import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from './pages/main/main.component';
import { MisDatosComponent } from './components/mis-datos/mis-datos.component';
import { NuevoRegistroComponent } from './components/nuevo-registro/nuevo-registro.component';
import { AyudaComponent } from './components/ayuda/ayuda.component';
import { RegistrosAnterioresComponent } from './components/registros-anteriores/registros-anteriores.component';


const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      { path: '', redirectTo: 'mis-datos', pathMatch: 'full' },
      { path: 'nuevo-registro', component: NuevoRegistroComponent },
      { path: 'mis-datos', component: MisDatosComponent },
      { path: 'registros-anteriores', component: RegistrosAnterioresComponent },
      { path: 'ayuda', component: AyudaComponent },
      
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
