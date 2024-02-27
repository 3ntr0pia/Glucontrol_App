import { Component, EventEmitter, Output } from '@angular/core';
import { AvatarService } from 'src/app/services/avatar.service';

@Component({
  selector: 'app-generador-avatar',
  templateUrl: './generador-avatar.component.html',
  styleUrls: ['./generador-avatar.component.css']
})
export class GeneradorAvatarComponent {

  @Output() avatarGenerado : EventEmitter<string> = new EventEmitter<string>();
 
  avatar : string = "";
  defaultAvatar : string ="assets/avatar.png";

  constructor(private avatarService: AvatarService) {}

  generarAvatar() {
    console.log('GenerarAvatar llamado');
    this.avatar = this.avatarService.getRandomAvatar();
    this.avatarGenerado.emit(this.avatar);
  }
}
