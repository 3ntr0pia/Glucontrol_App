import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'; 
import { Observable } from 'rxjs/internal/Observable';
@Injectable({
  providedIn: 'root'
})
export class AvatarService {

  private baseUrl = 'https://api.dicebear.com/';

  constructor(private http: HttpClient ) { }

  getAvatar(seed: string, sprites: string): string {
    return `${this.baseUrl}${sprites}/${seed}.svg`;
  }
}




