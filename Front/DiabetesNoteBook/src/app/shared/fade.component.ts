import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-fade',
  template: `
    <div *ngIf="show" [@fadeInOut]>
      <ng-content></ng-content>
    </div>
  `,
  styles: [`
    div {
      transition: opacity 0.5s ease-in-out;
    }
  `],
animations: [
    trigger('fadeInOut', [
        state('void', style({ opacity: 0 })),
        state('*', style({ opacity: 1 })),
        transition(':enter', animate('500ms ease-in')),
        transition(':leave', animate('500ms ease-out'))
    ])
]
})
export class FadeComponent {
  @Input() show: boolean = false;

 
}