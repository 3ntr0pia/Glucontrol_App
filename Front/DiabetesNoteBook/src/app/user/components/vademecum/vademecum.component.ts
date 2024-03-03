import { Component } from '@angular/core';
import {
  IMedicamento,
  IRespuestaServicio,
} from '../../../interfaces/medicamento.interface';
import { VademecumService } from 'src/app/services/vademecum.service';
import { UsuarioService } from 'src/app/services/usuario.service';
import { AuthServiceService } from 'src/app/services/auth-service.service';
import { Sexo } from 'src/app/enums/register.enum';
import { IUsuarioUpdate } from 'src/app/interfaces/usuario.interface';
import { IUserLoginResponse } from 'src/app/interfaces/loginResponse.interface';
import { MedicacionService } from 'src/app/services/medicacion.service';

@Component({
  selector: 'app-vademecum',
  templateUrl: './vademecum.component.html',
  styleUrls: ['./vademecum.component.css'],
})
export class VademecumComponent {
  medicamentosFromBackend: string[] = [];
  usuario: IUsuarioUpdate = {
    avatar: '',
    userName: '',
    nombre: '',
    primerApellido: '',
    segundoApellido: '',
    sexo: Sexo.hombre,
    edad: 0,
    peso: 0,
    altura: 0,
    actividad: '',
    tipoDiabetes: '',
    medicacion: '',
    insulina: false,
  };

  userId!: number;

  medicamentoSeleccionado: string = '';
  medicamentosArray: IMedicamento[] = [];
  Receta: boolean = true;
  Genericos: boolean = false;
  nuevoMedicamento: string = '';
  accModal: boolean = false;
  error: string = '';

  constructor(
    private vademecum: VademecumService,
    private userService: UsuarioService,
    private authService: AuthServiceService,
    private medicamentoService: MedicacionService
  ) {}

  ngOnInit(): void {
    this.getUserID(this.userId);
    console.log(this.userId);

    // this.getUserData(this.usuario);
    console.log(this.usuario);
  }

  modalAcc() {
    if (this.accModal) {
      this.accModal = false;
    } else {
      this.accModal = true;
    }
    console.log(this.accModal);
  }

  medicamentoChange() {
    if (this.medicamentoSeleccionado) {
      this.buscarMedicamentos(this.medicamentoSeleccionado);
    }
  }

  buscarMedicamentos(nombre: string) {
    nombre = this.medicamentoSeleccionado;
    this.vademecum.getMedicamentoInfo(nombre).subscribe({
      next: (res: IRespuestaServicio) => {
        this.medicamentosArray = res.resultados;
        console.log(this.medicamentosArray);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
  getUserID(userId: number) {
    this.authService.user.subscribe({
      next: (user) => {
        userId != user?.id;
      },
    });
  }
  getMedicamento(userId: number) {
    this.medicamentoService.getMedicaciones(userId).subscribe({});
  }

  // getUserData(user:unknown) {
  //   this.authService.loginUser(user).subscribe({
  //     next: (res) => {
  //       this.usuario = {
  //         id: res.id,
  //         avatar: res.avatar,
  //         userName: res.userName,
  //         nombre: res.nombre,
  //         primerApellido: res.primerApellido,
  //         segundoApellido: res.segundoApellido,
  //         sexo: res.sexo,
  //         edad: res.edad,
  //         peso: res.peso,
  //         altura: res.altura,
  //         actividad: res.actividad,
  //         tipoDiabetes: res.tipoDiabetes,
  //         medicacion: res.medicacion,
  //         insulina: res.insulina,
  //       };
  //       this.medicamentosFromBackend = this.usuario.medicacion.split(',');
  //       console.log(this.usuario);
  //     },
  //     error: (err) => {
  //       console.log(err);
  //     },
  //   });
  // }

  addMedicamento() {
    const nuevoMedicamentoLowerCase = this.nuevoMedicamento.toLocaleLowerCase();

    if (this.nuevoMedicamento === '') {
      this.error = 'El campo no puede estar vacío.';
      return;
    }
    if (!this.medicamentosFromBackend.includes(nuevoMedicamentoLowerCase)) {
      this.medicamentosFromBackend.push(nuevoMedicamentoLowerCase);
      this.nuevoMedicamento = '';
      this.usuario.medicacion = this.medicamentosFromBackend.join(',');

      this.userService.actualizarUsuario(this.usuario).subscribe({
        next: (res) => {
          this.error = '';
        },
        error: (err) => {
          console.log(err);
        },
      });
    } else {
      this.error = 'El medicamento ya existe en la lista.';
    }
  }

  eliminarMedicamento(medicamentoAEliminar: string) {
    const medicamentoAEliminarLowerCase =
      medicamentoAEliminar.toLocaleLowerCase();
    const existeMedicamento = this.medicamentosFromBackend.includes(
      medicamentoAEliminarLowerCase
    );

    if (!existeMedicamento) {
      this.error = 'El medicamento no existe en la lista.';
      return;
    }

    this.medicamentosFromBackend = this.medicamentosFromBackend.filter(
      (med) => med.toLocaleLowerCase() !== medicamentoAEliminarLowerCase
    );

    this.usuario.medicacion = this.medicamentosFromBackend.join(',');
    this.userService.actualizarUsuario(this.usuario).subscribe({
      next: (res) => {
        console.log('Medicamento eliminado:', medicamentoAEliminar);
        this.nuevoMedicamento = '';
        this.error = '';
      },
      error: (err) => {
        console.error('Error al actualizar el usuario:', err);
      },
    });
  }
}
