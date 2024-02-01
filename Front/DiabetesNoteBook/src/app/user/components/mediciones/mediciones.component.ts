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
    racionHc : 0,
    id_Persona: 0,
  };
  chartOption: EChartsOption = {};
  elementoPagina: IMedicionesAzucar[] = [];
  paginaActual: number = 1;
  numeroTotalDePaginas: number = 0;
  elementosPorPagina: number = 4;
  accModal : boolean = false;
  constructor(
    private medicionesService: MedicionesService,
    private authService: AuthServiceService,
    private usuarioService: UsuarioService,
    
  ) {
    //Poner aqui cualquier cosa hace que se ejecute al inicio, a diferencia de ngOnInit que se ejecuta cuando se carga la vista
    this.chartOption = {};
    
  }

  modalAcc(){
    if(this.accModal){
      this.accModal = false;
    }else{
      this.accModal = true;
    }
    console.log(this.accModal);
  }

  ngOnInit() {
    this.getMediciones(this.authService.userValue!.id);
    this.getPersonaID();
    this.nuevaMedicion.fecha = new Date();
    this.elementoPagina.reverse();
     console.log(this.elementoPagina);
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
    let fechas = this.mediciones.map((m) => {
      let fecha = new Date(m.fecha);
      return `${fecha.getHours()}:${fecha.getMinutes().toString().padStart(2, '0')}:${fecha.getSeconds().toString().padStart(2, '0')}`;
    });
    let preMediciones = this.mediciones.map((m) => m.preMedicion);
    let glucemiasCapilares = this.mediciones.map((m) => m.glucemiaCapilar);
    let medidasGenerales = this.mediciones.map(m => m.preMedicion !== 0 ? m.preMedicion : m.glucemiaCapilar);

    fechas = fechas.reverse();
    preMediciones = preMediciones.reverse();
    glucemiasCapilares = glucemiasCapilares.reverse();
    medidasGenerales = medidasGenerales.reverse();

    
    this.chartOption = {
      aria: {
        description: 'Una descripción detallada de tu gráfico de mediciones de glucosa.',
        decal: {
          show: true
        },
        enabled: true,
      },
      title: {
        text: 'Mediciones de Glucosa',
        left: 'center',
        textStyle: {
          fontSize: 20,
        }
      },
      tooltip: {
        trigger: 'axis',
      },
      legend: {
        data: [
          { name: 'Pre Medicion', icon: 'diamond' },  
          { name: 'Post Medicion', icon: 'diamond' },  
          { name: 'Mediciones', icon: 'circle'},
          { name: 'Hiperglucemia', icon: 'rect' },  
          { name: 'Hipoglucemia', icon: 'rect' }, 
        ],
        bottom: 0,
        selected : {
          'Pre Medicion' : false,
          'Post Medicion' : false,
        }
        
      },
      grid: {
        left: '10%',
        right: '10%',
        bottom: '10%',
        containLabel: true,
      },
      xAxis: {
        type: 'category',
        boundaryGap: true,
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
          name: 'Mediciones',
          type: 'line',
          data: medidasGenerales,
          markPoint: {
            data: [
              { type: 'max', name: 'Máximo' },
              { type: 'min', name: 'Mínimo' },
            ],
          },
          
        },
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
          
        },
       
        {
          name: 'Hiperglucemia',
          type: 'line',
          data: [], 
          markArea: {
            silent: true, 
            itemStyle: {
              color: 'rgba(255, 0, 0, 0.2)',
               
            },
            data: [[
              { yAxis: 210 }, 
              { yAxis: 180 }, 
            ]]
          }
        },
        {
          name: 'Hipoglucemia',
          type: 'line',
          data: [], 
          markArea: {
            silent: true,
            itemStyle: {
              color: 'rgba(255, 0, 0, 0.2)', 
            },
            data: [[
              { yAxis: 0 }, 
              { yAxis: 70 }, 
            ]]
          }
        },
      ],
    };

}

  getMediciones(userId: number) {
    this.medicionesService.getMediciones(userId).subscribe({
      next: (mediciones) => {
        console.log('Datos recibidos del servidor:', mediciones);
        if(mediciones.length == 0){
          this.elementoPagina=[]
        }
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
        this.getMediciones(this.authService.userValue!.id);
        this.prepararDatosGrafico();
        this.calcularTotalDePaginas();
        this.cambiarPagina(this.paginaActual);
        
        // this.verificarLimitesEnMediciones(); 
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  deleteMedicion(idMedicion: number) {
    this.medicionesService.deleteMediciones(idMedicion).subscribe({
      next: (res) => {
        console.log(res);
        this.getMediciones(this.authService.userValue!.id);
        

        this.calcularTotalDePaginas();
        if (this.paginaActual > this.numeroTotalDePaginas) {
          this.paginaActual = this.numeroTotalDePaginas;
        }
        this.cambiarPagina(this.paginaActual);
  


        
        this.prepararDatosGrafico();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

}
