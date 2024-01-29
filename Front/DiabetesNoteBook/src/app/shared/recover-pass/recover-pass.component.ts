import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IRecover } from 'src/app/interfaces/loginResponse.interface';
import { RecordarPassService } from 'src/app/services/recordar-pass.service';

@Component({
  selector: 'app-recover-pass',
  templateUrl: './recover-pass.component.html',
  styleUrls: ['./recover-pass.component.css'],
})
export class RecoverPassComponent {
  constructor(
    private recover: RecordarPassService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {
    this.activatedRoute.params.subscribe((params) => {
      this.token = params['token'];
      console.log(this.token);
    });
  }
  token: string = '';
  newPass: string = '';
  error: string = '';
  recuperarPass() {
    const datoLogin: IRecover = {
      token: this.token,
      newPass: this.newPass,
    };
    console.log(datoLogin);
    this.recover.recordarPass(datoLogin).subscribe({
      next: (res) => {
        this.router.navigate(['/login']);
        console.log(res);
      },
      error: (err) => {
        this.error = err.error;
        console.log(this.error);
      },
    });
  }
}
