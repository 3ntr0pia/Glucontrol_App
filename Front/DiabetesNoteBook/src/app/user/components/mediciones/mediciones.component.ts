import { Component } from '@angular/core';
import { EChartsOption } from 'echarts';
import { IUserLoginResponse } from 'src/app/interfaces/loginResponse.interface';
import {
  IMedicionesAzucar,
  Regimen,
} from 'src/app/interfaces/mediciones.interface';
import { AuthServiceService } from 'src/app/services/auth-service.service';
import { MedicionesService } from 'src/app/services/mediciones.service';
import { UsuarioService } from 'src/app/services/usuario.service';

@Component({
  selector: 'app-mediciones',
  templateUrl: './mediciones.component.html',
  styleUrls: ['./mediciones.component.css'],
})
export class MedicionesComponent {
  deporteRealizado: boolean = false;
  momentoGlucemia: boolean = true;
  bolo : boolean = false;
  mediciones: IMedicionesAzucar[] = [];
  mostrarModal: boolean = false;
  mensajeModal: string = '';
  usuarioLogeadoPersonaId: number = 0;
  nuevaMedicion: IMedicionesAzucar = {
    id: 0,
    fecha: new Date(),
    regimen: Regimen.Desayuno,
    preMedicion: 0,
    glucemiaCapilar: 0,
    bolusComida: 0,
    bolusCorrector: 0,
    preDeporte: 0,
    duranteDeporte: 0,
    postDeporte: 0,
    notas: '',
    id_Persona: 0,
  };
  chartOption: EChartsOption = {};
  elementoPagina: any[] = [];
  paginaActual: number = 1;
  numeroTotalDePaginas: number = 0;
  elementosPorPagina: number = 4;

  constructor(
    private medicionesService: MedicionesService,
    private authService: AuthServiceService,
    private usuarioService: UsuarioService
  ) {
    //Poner aqui cualquier cosa hace que se ejecute al inicio, a diferencia de ngOnInit que se ejecuta cuando se carga la vista
    this.chartOption = {};
    
  }

  ngOnInit() {
    this.getMediciones(this.authService.userValue!.id);
    this.getPersonaID();
    this.nuevaMedicion.fecha = new Date();
    
  }



  get fechaInput(): string {
    return this.convertirFechaAString(this.nuevaMedicion.fecha);
  }

  private convertirFechaAString(fecha: Date): string {
    const año = fecha.getFullYear().toString();
    const mes = this.ponerCero(fecha.getMonth() + 1); 
    const dia = this.ponerCero(fecha.getDate());
    const hora = this.ponerCero(fecha.getHours());
    const minuto = this.ponerCero(fecha.getMinutes());

    return `${año}-${mes}-${dia}T${hora}:${minuto}`;
  }

  private ponerCero(numero: number): string {
    return (numero < 10 ? '0' : '') + numero;
  }

  onFechaChange(newValue: string) {
    this.nuevaMedicion.fecha = new Date(newValue);
  }

  getPersonaID() {
    this.usuarioService
      .getUsuarioYPersonaInfo(this.authService.userValue!.id)
      .subscribe({
        next: (res) => {
          this.usuarioLogeadoPersonaId = res[1].id as number;
          this.nuevaMedicion.id_Persona = this.usuarioLogeadoPersonaId;
        },
        error: (error) => console.error(error),
      });
  }

  calcularTotalDePaginas() {
    this.numeroTotalDePaginas = Math.ceil(
      this.mediciones.length / this.elementosPorPagina
    );
  }

  cambiarPagina(nuevaPagina: number): void {
    if (nuevaPagina >= 1 && nuevaPagina <= this.numeroTotalDePaginas) {
      this.paginaActual = nuevaPagina;
      const inicio = (this.paginaActual - 1) * this.elementosPorPagina;
      const fin = inicio + this.elementosPorPagina;
      this.elementoPagina = this.mediciones.slice(inicio, fin);
    }
  }

  abrirNotasModal(medicion: IMedicionesAzucar) {
    if (medicion.notas == '') {
      console.log('No hay notas para mostrar');
    } else {
      this.mostrarModal = true;
      this.mensajeModal = medicion.notas;
    }
  }

  prepararDatosGrafico() {
    const fechas = this.mediciones.map((m) => {
      const fecha = new Date(m.fecha);
      return `${fecha.getDate()}/${fecha.getMonth() + 1}`;
    });
    const preMediciones = this.mediciones.map((m) => m.preMedicion);
    const glucemiasCapilares = this.mediciones.map((m) => m.glucemiaCapilar);

    this.chartOption = {
      title: {
        text: 'Mediciones de Glucosa',
        left: 'center',
      },
      tooltip: {
        trigger: 'axis',
      },
      legend: {
        data: ['Pre Medicion', 'Post Medicion'],
        top: 'bottom',
      },
      grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true,
      },
      xAxis: {
        type: 'category',
        boundaryGap: false,
        data: fechas,
      },
      yAxis: {
        type: 'value',
        axisLabel: {
          formatter: '{value} mg/dl',
        },
      },
      series: [
        {
          name: 'Pre Medicion',
          type: 'line',
          data: preMediciones,
          markPoint: {
            data: [
              { type: 'max', name: 'Máximo' },
              { type: 'min', name: 'Mínimo' },
            ],
          },
          markLine: {
            data: [{ type: 'average', name: 'Media' }],
          },
        },
        {
          name: 'Post Medicion',
          type: 'line',
          data: glucemiasCapilares,
          markPoint: {
            data: [
              { type: 'max', name: 'Máximo' },
              { type: 'min', name: 'Mínimo' },
            ],
          },
          markLine: {
            data: [{ type: 'average', name: 'Media' }],
          },
        },
      ],
    };
  }

  getMediciones(userId: number) {
    this.medicionesService.getMediciones(userId).subscribe({
      next: (mediciones) => {
        console.log('Datos recibidos del servidor:', mediciones);
        this.mediciones = mediciones.reverse();
        this.prepararDatosGrafico();
        this.calcularTotalDePaginas();
        this.cambiarPagina(this.paginaActual);
      },
      error: (error) => console.error(error),
    });
  }

  postMedicion() {
    this.medicionesService.postMediciones(this.nuevaMedicion).subscribe({
      next: (res) => {
        console.log('Datos recibidos del servidor');
        this.getMediciones(this.authService.userValue!.id);
        this.prepararDatosGrafico();
        this.calcularTotalDePaginas();
        this.cambiarPagina(this.paginaActual);
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  deleteMedicion(idMedicion: number) {
    this.medicionesService.deleteMediciones(idMedicion).subscribe({
      next: (res) => {
        console.log(res)
        this.getMediciones(this.authService.userValue!.id);
        this.prepararDatosGrafico();
        this.calcularTotalDePaginas();
        this.cambiarPagina(this.paginaActual);
      },
      error: (error) => {
        console.error(error);
      },
    });
  }
}
