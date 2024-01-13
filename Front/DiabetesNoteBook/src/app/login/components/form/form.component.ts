import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthServiceService } from 'src/app/services/auth-service.service';
import { ILogin } from '../../interfaces/login.interface';

@Component({
  selector: 'login-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class FormComponent {

  usuario : string = '';
  password : string = '';
  error : string = '';

  constructor(private authService: AuthServiceService, private router: Router ) { }

  login() {
    const datoLogin : ILogin = {
      UserName: this.usuario,
      Password: this.password
    }
    this.authService.loginUser(datoLogin).subscribe(
      (res) => {
        console.log(res);
        this.router.navigate(['/user-dashboard']);
      },
      (err) => {
        this.error = err.error;
        console.log(this.error);
      }
    );
  }

}
