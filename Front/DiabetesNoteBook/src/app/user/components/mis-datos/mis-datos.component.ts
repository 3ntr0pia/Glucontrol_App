import { Component, OnInit } from '@angular/core';
import { Sexo, Actividad, TipoDiabetes } from 'src/app/enums/register.enum';
import { IUserLoginResponse } from 'src/app/interfaces/loginResponse.interface';
import { IUsuarioUpdate } from 'src/app/interfaces/usuario.interface';
import { AuthServiceService } from 'src/app/services/auth-service.service';
import { UsuarioService } from 'src/app/services/usuario.service';

@Component({
  selector: 'app-mis-datos',
  templateUrl: './mis-datos.component.html',
  styleUrls: ['./mis-datos.component.css'],
})
export class MisDatosComponent implements OnInit {
  public Sexo = Sexo;
  public Actividad = Actividad;
  public TipoDiabetes = TipoDiabetes;
  public nuevaAltura = 0;
  public nuevoPeso = 0;
  accModal: boolean = false;

  usuarioLogeado: IUserLoginResponse | null = null;
  usuario: IUsuarioUpdate = {
    id: 0,
    avatar: '',
    userName: '',
    nombre: '',
    primerApellido: '',
    segundoApellido: '',
    sexo: Sexo.hombre,
    edad: 0,
    peso: 0,
    altura: 0,
    actividad: '',
    tipoDiabetes: '',
    medicacion: '',
    insulina: false,
  };

  error: string = '';
  errorPass: string = '';
  successPass: string = '';
  nuevoAvatar: string = '';

  estadoInicialUsuario: IUsuarioUpdate = { ...this.usuario };

  abrirModalPass: boolean = false;

  pass: string = '';
  repetirPass: string = '';

  constructor(
    private usuarioService: UsuarioService,
    private authService: AuthServiceService
  ) {}
  //optencion de los datos de usuario
  ngOnInit(): void {
    this.authService.user.subscribe((user) => {
      this.usuarioLogeado = user;
      if (this.usuarioLogeado) {
        this.showUsuarioInfo(this.usuarioLogeado);
      }
    });
  }

  modalAcc() {
    if (this.accModal) {
      this.accModal = false;
    } else {
      this.accModal = true;
    }
    console.log(this.accModal);
  }
  avatarHandler(avatar: string): void {
    this.nuevoAvatar = avatar;
  }

  alturaHandler(altura: number): void {
    this.nuevaAltura = altura;
    console.log(altura);
  }
  pesoHandler(peso: number): void {
    this.nuevoPeso = peso;
  }

  showUsuarioInfo(user: IUserLoginResponse): void {
    this.authService.loginUser(user).subscribe({
      next: (res) => {
        this.usuario = {
          id: res.id,
          avatar: res.avatar,
          userName: res.userName,
          nombre: res.nombre,
          primerApellido: res.primerApellido,
          segundoApellido: res.segundoApellido,
          sexo: res.sexo,
          edad: res.edad,
          peso: res.peso,
          altura: res.altura,
          actividad: res.actividad,
          tipoDiabetes: res.tipoDiabetes,
          medicacion: res.medicacion,
          insulina: res.insulina,
        };

        this.estadoInicialUsuario = {
          id: res.id,
          avatar: res.avatar,
          userName: res.userName,
          nombre: res.nombre,
          primerApellido: res.primerApellido,
          segundoApellido: res.segundoApellido,
          sexo: res.sexo,
          edad: res.edad,
          peso: res.peso,
          altura: res.altura,
          actividad: res.actividad,
          tipoDiabetes: res.tipoDiabetes,
          medicacion: res.medicacion,
          insulina: res.insulina,
        };
        console.log('ShowUser: ', res);
        console.log('Usuario:', this.usuario);

        //Esto se puede hacer tambien con el operador spread pero no seria tan preciso
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  reestablecerFormulario(): void {
    this.usuario = { ...this.estadoInicialUsuario };
  }

  cambiarPass(): void {
    this.usuarioService
      .cambiarPass({ id: this.usuarioLogeado!.id, NewPass: this.pass })
      .subscribe({
        next: (res) => {
          console.log('Contraseña cambiada correctamente');
          this.successPass = 'Contraseña cambiada correctamente';
        },
        error: (err) => {
          this.errorPass = err.error;
          console.error('Error al cambiar la contraseña:', err);
        },
      });
  }

  actualizarUsuario(): void {
    if (this.nuevoAvatar !== '') {
      this.usuario.avatar = this.nuevoAvatar;
    }
    if (this.nuevaAltura !== 0) {
      this.usuario.altura = this.nuevaAltura;
    }

    if (this.nuevoPeso !== 0) {
      this.usuario.peso = this.nuevoPeso;
    }
    if (!this.validarFormulario(this.usuario)) {
      this.error = 'Formulario invalido';
      return;
    }
    console.log(this.usuario);

    this.usuarioService.actualizarUsuario(this.usuario).subscribe({
      next: (res) => {
        console.log('Usuario actualizado:', res);

        this.usuarioLogeado = {
          ...this.usuarioLogeado!,
          avatar: this.usuario.avatar,
          nombre: this.usuario.nombre,
          primerApellido: this.usuario.primerApellido,
          segundoApellido: this.usuario.segundoApellido,
        };

        this.authService.updateUser(this.usuarioLogeado);
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  validarFormulario(usuario: IUsuarioUpdate): boolean {
    return (
      usuario.nombre.trim() !== '' &&
      usuario.userName.trim() !== '' &&
      usuario.primerApellido.trim() !== '' &&
      usuario.segundoApellido.trim() !== '' &&
      usuario.edad > 0 &&
      usuario.actividad !== '' &&
      usuario.tipoDiabetes !== ''
    );
  }
  validarPassword(password: string): boolean {
    const patron =
      /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
    return patron.test(password);
  }
  validarUsuario(usuario: string): boolean {
    const patronUsuario = /^[A-Za-z0-9_-]{6,18}$/;
    return patronUsuario.test(usuario);
  }
  validarEmail(email: string): boolean {
    const patronEmail = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    return patronEmail.test(email);
  }

  cerrarModal(): void {
    this.pass = '';
    this.repetirPass = '';
    this.errorPass = '';
    this.successPass = '';
    this.abrirModalPass = false;
  }
}
