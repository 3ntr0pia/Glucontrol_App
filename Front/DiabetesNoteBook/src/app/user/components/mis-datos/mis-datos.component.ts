import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Chart } from 'chart.js';
import { UsuarioService } from '../../services/usuario.service';
import { IUsuarioResponse } from '../../interfaces/usuario.interface';
import { AuthServiceService } from 'src/app/services/auth-service.service';
import { IUserLoginResponse } from 'src/app/interfaces/IUsuario.interface';

@Component({
  selector: 'app-mis-datos',
  templateUrl: './mis-datos.component.html',
  styleUrls: ['./mis-datos.component.css']
})


export class MisDatosComponent implements OnInit {

  usuarioLogeado: IUserLoginResponse | null = null;
  usuario !: IUsuarioResponse;

  nuevoAvatar : string = "";

  constructor(private usuarioService : UsuarioService, private authService : AuthServiceService  ) { }



    ngOnInit(): void {
      //Esta es la forma nueva de suscribirse a un observable (si no sale tachado el subscribe es por que no se usa)
      this.authService.user.subscribe({
        next: (user: IUserLoginResponse | null) => {
          this.usuarioLogeado = user;
          if (this.usuarioLogeado) {
            this.getUsuarioInfo(this.usuarioLogeado.idUsuario);
          }
        },
        error: (err) => {
          console.error(err);
        }
      });
    }

    setAvatar(avatar: string): void {
      this.nuevoAvatar = avatar;
    }

  getUsuarioInfo(usuarioId : number) : void {
    
    this.usuarioService.getUsuarioInfo(usuarioId).subscribe({
      next: (usuario: IUsuarioResponse) => {
        this.usuario = usuario;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  
  
  
}


