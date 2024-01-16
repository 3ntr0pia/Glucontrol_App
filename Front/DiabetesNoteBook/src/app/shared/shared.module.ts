import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from './modal/modal.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { SharedRoutingModule } from './shared-routing.module';

@NgModule({
  declarations: [ModalComponent, NotfoundComponent],
  imports: [CommonModule,SharedRoutingModule],
  exports: [ModalComponent],
})
export class SharedModule {}
