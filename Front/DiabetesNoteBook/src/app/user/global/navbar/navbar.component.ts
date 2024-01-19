import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { IUserLoginResponse } from 'src/app/interfaces/IUsuario.interface';
import { AuthServiceService } from 'src/app/services/auth-service.service';

@Component({
  selector: 'user-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {

  @Input() usuarioLogeado : IUserLoginResponse | null = null;



  constructor(private authService : AuthServiceService , private router : Router  ) { }


  
  logout(){
    this.authService.logoutUser();
    this.router.navigate(['/login']);
  }

}
