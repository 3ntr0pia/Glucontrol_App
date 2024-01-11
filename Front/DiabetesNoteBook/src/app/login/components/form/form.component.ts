import { Component } from '@angular/core';

@Component({
  selector: 'login-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class FormComponent {

  usuario : string = '';
  password : string = '';
  
  login() {
    console.log(this.usuario);
    console.log(this.password);
  }

}
