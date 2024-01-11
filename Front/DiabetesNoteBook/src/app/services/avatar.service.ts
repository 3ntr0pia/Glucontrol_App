import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AvatarService {
  constructor(private http: HttpClient) {}

  hairs: string[] = [
    'balndess',
    'classic01',
    'classic02',
    'curly',
    'elvis',
    'long',
    'ponytail',
    'slaughter',
    'stylish',
  ];
  heads: string[] = ['normal', 'thin', 'wide'];
  mouths: string[] = ['default', 'missingTooth'];
  mustaches: string[] = ['freddy', 'horshoe', 'pencilThin', 'pencilThinBeard'];
  blushes: boolean = false;
  
  eyes: string[] = ['confident', 'happy', 'normal'];
  glasses: string[] = ['none', 'sunglasses', 'round', 'smart'];
  skinColor: string[] = ['836055', 'f5d0c5', 'ffcb7e'];
  
  getRandomBoolean() {
    return Math.random() < 0.5;
  }
  
  getRandomColor() {
    return Math.random().toString(16).slice(2, 8);
  }

  getRandomInt(min: number, max: number) {
    return Math.floor(Math.random() * (max - min)) + min;
  }
  
  getRandomAvatar() {
    const randomSeed = Math.random().toString(36).substring(2, 15);
    let head = this.heads[this.getRandomInt(0, this.heads.length)];
    let hair = this.hairs[this.getRandomInt(0, this.hairs.length)];
    let mouth = this.mouths[this.getRandomInt(0, this.mouths.length)];
    let mustache = this.mustaches[this.getRandomInt(0, this.mustaches.length)];
    let eyes = this.eyes[this.getRandomInt(0, this.eyes.length)];
    let skin = this.skinColor[this.getRandomInt(0, this.skinColor.length)];
    let blush = this.blushes;
    let glasses = this.glasses;
    let backgroundColor: string = this.getRandomColor();
    let blushesProbability: number = this.getRandomInt(0, 100);
    let bodyColors: string = this.getRandomColor();
    let mustacheProbability: number = this.getRandomInt(0, 100);
    let glassesProbability: number = this.getRandomInt(0, 100);
    let hairColor: string = this.getRandomColor();

    return `https://api.dicebear.com/7.x/miniavs/svg?bodyColor=${bodyColors}&eyes=${eyes}&hair=${hair}&hairColor=${hairColor}&head=${head}&mouth=${mouth}&skin=${skin}&backgroundColor=${backgroundColor}`;
  }
}
