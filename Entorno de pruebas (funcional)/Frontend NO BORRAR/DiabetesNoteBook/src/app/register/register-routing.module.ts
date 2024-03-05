import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegisterMainComponent } from './pages/main/register-main.component';

const routes: Routes = [
  {
    path: '',
    component: RegisterMainComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegisterRoutingModule { }
