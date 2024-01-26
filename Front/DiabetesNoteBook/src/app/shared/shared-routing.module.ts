import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NotfoundComponent } from './notfound/notfound.component';
import { RecoverPassComponent } from './recover-pass/recover-pass.component';

const routes: Routes = [
  {
    path: '**',
    component: NotfoundComponent
  },
  {
    path: 'recover-pass',
    component: RecoverPassComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SharedRoutingModule { }
