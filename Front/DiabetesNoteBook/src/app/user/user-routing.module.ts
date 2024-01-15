import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from './pages/main/main.component';
import { MisDatosComponent } from './components/mis-datos/mis-datos.component';


const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      { path: '', redirectTo: 'nuevo-registro', pathMatch: 'full' },
      { path: 'nuevo-registro', component: MisDatosComponent },
      { path: 'mis-datos', component: MisDatosComponent },
      { path: 'registros-anteriores', component: MisDatosComponent },
      { path: 'ayuda', component: MisDatosComponent },
      
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
