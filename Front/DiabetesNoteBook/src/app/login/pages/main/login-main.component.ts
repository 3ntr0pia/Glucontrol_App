import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
@Component({
  selector: 'login-main',
  templateUrl: './login-main.component.html',
  styleUrls: ['./login-main.component.css'],
})
export class LoginMainComponent implements OnInit {
  title = 'login-GluControl';
  constructor(private titleService: Title) {}
  ngOnInit() {
    this.titleService.setTitle(this.title);
  }
}
