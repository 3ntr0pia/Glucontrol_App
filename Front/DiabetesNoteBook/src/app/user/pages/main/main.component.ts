import { Component, OnInit } from '@angular/core';
import { IUserLoginResponse } from 'src/app/interfaces/IUsuario.interface';
import { AuthServiceService } from 'src/app/services/auth-service.service';

@Component({
  selector: 'user-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit{
  
  usuarioLogeado : IUserLoginResponse | null = null;

  constructor(private authService : AuthServiceService ) { }


  ngOnInit(): void {
    this.recibirUsuarioLogeado();
  }
  
  recibirUsuarioLogeado(){
    this.authService.user.subscribe((user: IUserLoginResponse | null) => {
      this.usuarioLogeado = user;
      console.log(this.usuarioLogeado);
    });
  }



}
