import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription } from 'rxjs';
import { urlImages } from 'src/app/environments/config';
@Component({
  selector: 'login-main',
  templateUrl: './login-main.component.html',
  styleUrls: ['./login-main.component.css'],
})
export class LoginMainComponent implements OnInit {
  title = 'login-GluControl';

  private subscription: Subscription;
  logo : string = ''
  logoTexto : string = ''
  constructor(private titleService: Title) {

    this.subscription = urlImages.logo.subscribe((newLogo) => {
      this.logo = newLogo;
    });
    this.subscription = urlImages.logoTexto.subscribe((newLogoTexto) => {
      this.logoTexto = newLogoTexto;
    });
  }
  ngOnInit() {
    this.titleService.setTitle(this.title);
  }
}
