import { Component, Output } from '@angular/core';
import { Sexo, Actividad, TipoDiabetes } from '../../../enums/register.enum';
import { IRegister } from '../../../interfaces/register.interface';
import { AuthServiceService } from 'src/app/services/auth-service.service';
import { IFinalRegister } from '../../../interfaces/finalregister.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'register-main',
  templateUrl: './register-main.component.html',
  styleUrls: ['./register-main.component.css'],
})
export class RegisterMainComponent {
  public Sexo = Sexo;
  public Actividad = Actividad;
  public TipoDiabetes = TipoDiabetes;
  error: string = '';
  datosRegistro: IRegister = {
    username: '',
    avatar: '',
    nombre: '',
    apellido: '',
    apellido2: '',
    email: '',
    password: '',
    password2: '',
    mediciones: {
      edad: 0,
      peso: 0,
      altura: 0,
      sexo: this.Sexo.hombre,
      actividad: this.Actividad.sedentario,
      tipoDiabetes: {
        tipo: this.TipoDiabetes.tipo1,
        medicacion: [],
        insulina: false,
      },
    },
  };

  retrocederPaso(): void {
    if (this.paso > 1) {
      this.paso--;
    }
  }
  paso: number = 1;

  siguientePaso(info: IRegister): void {
    //this.datosRegistro = info;  SE CARGA TODO EL OBJETO;
    Object.assign(this.datosRegistro, info); // SE CARGA SOLO LA PARTE QUE SE HA MODIFICADO
    console.log(this.datosRegistro);
    if (this.paso < 3) {
      this.paso++;
    }
  }

  constructor(
    private registerService: AuthServiceService,
    private router: Router
  ) {}

  registroUsuario(datosFinales: IFinalRegister) {
    this.registerService.registerUser(datosFinales).subscribe({
      next: (data) => {
        console.log(data);
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.error = err.error.errors.email;
        console.log(err);
      },
      complete: () => console.log('Operation completed'),
    });
  }
}
