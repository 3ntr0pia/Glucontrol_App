import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { config } from 'rxjs';
import { urlImages } from 'src/app/environments/config';
import { environment } from 'src/app/environments/environment';

import { ILogin, IRecover } from 'src/app/interfaces/loginResponse.interface';
import { AuthServiceService } from 'src/app/services/auth-service.service';
import { RecordarPassService } from 'src/app/services/recordar-pass.service';

@Component({
  selector: 'login-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css'],
})
export class FormComponent {
  usuario: string = '';
  password: string = '';
  mail: string = '';
  error: string = '';
  
  

  constructor(
    private authService: AuthServiceService,
    private router: Router,
    private recordarService: RecordarPassService

  ) {}

  recordar: boolean = false;
  mostrarModal: boolean = false;
  mensajeModal: string = '';
  accModal: boolean = false;
  recuperarPass: IRecover = {
    token: '',
    newPass: '',
  };

  verOlvidado() {
    this.recordar = true;
  }


  modalAcc() {
    if (this.accModal) {
      this.accModal = false;
    } else {
      this.accModal = true;
    }
    console.log(this.accModal);
  }
  
  login() {
    const datoLogin: ILogin = {
      UserName: this.usuario,
      Password: this.password,
    };

    this.authService.loginUser(datoLogin).subscribe({
      next: (res) => {
        this.router.navigate(['/user-dashboard']);
      },
      error: (err) => {
        this.error = err.error;
        console.log(this.error);
      },
    });
  }

  recordarPassword() {
    this.recordar = false;
    this.mensajeModal =
      'Tu solicitud de recuperación de contraseña ha sido enviada. Por favor, revisa tu correo electrónico.';
    this.recordarService.recordarPassLogin(this.mail).subscribe({
      next: (res) => {
        console.log('Ha salido todo bien', res);
      },
      error: (err) => {
        console.log(err);
      },
    });
    this.mostrarModal = true;
  }

  formularioInvalido() {
    const regexEmail = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!regexEmail.test(this.mail)) {
      return true;
    }
    return false;
  }
}
