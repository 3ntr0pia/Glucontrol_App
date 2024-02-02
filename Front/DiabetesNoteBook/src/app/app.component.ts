import { Component } from '@angular/core';
import { environment } from './environments/environment';
import { urlImages } from './environments/config';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'GlucoControl | Diario de diabetes';

  modoNoche: boolean = false;

  cambiarModo() {
    
    this.modoNoche = !this.modoNoche;
    if (this.modoNoche) {


      urlImages.logo.next('assets/noche/logo.png');
      urlImages.logoTexto.next('assets/noche/logoTexto.png');
      document.documentElement.style.setProperty('--main--colorBackGround', '#002d52');
      document.documentElement.style.setProperty('--main--colorForeground', '#68c2f1');
    } else {
      document.documentElement.style.setProperty('--main--colorBackGround', '');
      document.documentElement.style.setProperty('--color-texto', '');
    }
  }
  
}
