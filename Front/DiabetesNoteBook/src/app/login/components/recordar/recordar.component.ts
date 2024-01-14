import { Component } from '@angular/core';

@Component({
  selector: 'app-recordar',
  templateUrl: './recordar.component.html',
  styleUrls: ['./recordar.component.css']
})
export class RecordarComponent {


  email : string = '';

  recordarPass() {
    console.log(this.email);
  }
}
