import { Component } from '@angular/core';
import { EChartsOption } from 'echarts';
import { IMedicionesAzucar , Regimen } from 'src/app/interfaces/mediciones.interface';


@Component({
  selector: 'app-mediciones',
  templateUrl: './mediciones.component.html',
  styleUrls: ['./mediciones.component.css']
})
export class MedicionesComponent  {


  deporteRealizado: boolean = false; 
  momentoGlucemiaAntes: boolean = true;
  mediciones: IMedicionesAzucar[] = [];
  mostrarModal : boolean = false;
  mensajeModal: string = '';


 

  medicionesFromBackend: IMedicionesAzucar[] = [
    {
      id: 1,
      fecha: new Date(2024, 0, 10),
      regimen: Regimen.Desayuno,
      preMedicion: 120,
      postMedicion: 140,
      glucemiaCapilar: 95,
      bolusComida: 2,
      bolusCorrector: 1,
      preDeporte: 90,
      duranteDeporte: 85,
      postDeporte: 100,
      notas: 'Sensación de mareo leve por la mañana.',
      idPersona: 1
    },
    {
      id: 2,
      fecha: new Date(2024, 0, 11, 12, 45),
      regimen: Regimen.Comida,
      preMedicion: 100,
      postMedicion: 125,
      glucemiaCapilar: 90,
      bolusComida: 1.5,
      bolusCorrector: 1,
      preDeporte: 85,
      duranteDeporte: 80,
      postDeporte: 95,
      notas: '',
      idPersona: 1
    },
    {
      id: 3,
      fecha: new Date(2024, 0, 12, 19, 20),
      regimen: Regimen.Cena,
      preMedicion: 115,
      postMedicion: 130,
      glucemiaCapilar: 100,
      bolusComida: 2,
      bolusCorrector: 1.2,
      preDeporte: 0,
      duranteDeporte: 0,
      postDeporte: 0,
      notas: 'Cena ligera.',
      idPersona: 1
    },
    {
      id: 4,
      fecha: new Date(2024, 0, 13, 7, 15),
      regimen: Regimen.Desayuno,
      preMedicion: 130,
      postMedicion: 145,
      glucemiaCapilar: 105,
      bolusComida: 2.5,
      bolusCorrector: 0,
      preDeporte: 0,
      duranteDeporte: 0,
      postDeporte: 0,
      notas: 'Desperté con hambre.',
      idPersona: 1
    }
    
  ]


  chartOption: EChartsOption = {}; 
  constructor() {this.chartOption = {};}



  nuevaMedicion : IMedicionesAzucar = {
    id: 0,
    fecha: new Date(),
    regimen: Regimen.Desayuno,
    preMedicion: 0,
    postMedicion: 0,
    glucemiaCapilar: 0,
    bolusComida: 0,
    bolusCorrector: 0,
    preDeporte: 0,
    duranteDeporte: 0,
    postDeporte: 0,
    notas: '',
    idPersona: 0
  }
  
  
  abrirNotasModal(medicion: IMedicionesAzucar){
    if(medicion.notas == ''){
      console.log('No hay notas para mostrar');
    }else{
      this.mostrarModal = true;
      this.mensajeModal = medicion.notas;
    }
    
  }
  


  ngOnInit() {
    this.prepararDatosGrafico();
  }

  prepararDatosGrafico() {
    const fechas = this.medicionesFromBackend.map(m => 
      `${m.fecha.getDate()}/${m.fecha.getMonth() + 1}` // Formato "día/mes"
    );
    const preMediciones = this.medicionesFromBackend.map(m => m.preMedicion);
    const postMediciones = this.medicionesFromBackend.map(m => m.postMedicion);
  
    this.chartOption = {
      title: {
        text: 'Mediciones de Glucosa',
        left: 'center'
      },
      tooltip: {
        trigger: 'axis'
      },
      legend: {
        data: ['Pre Medicion', 'Post Medicion'],
        top: 'bottom'
      },
      grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
      },
      xAxis: {
        type: 'category',
        boundaryGap: false,
        data: fechas
      },
      yAxis: {
        type: 'value',
        axisLabel: {
          formatter: '{value} mg/dl'
        }
      },
      series: [
        {
          name: 'Pre Medicion',
          type: 'line',
          data: preMediciones,
          markPoint: {
            data: [
              { type: 'max', name: 'Máximo' },
              { type: 'min', name: 'Mínimo' }
            ]
          },
          markLine: {
            data: [{ type: 'average', name: 'Media' }]
          }
        },
        {
          name: 'Post Medicion',
          type: 'line',
          data: postMediciones,
          markPoint: {
            data: [
              { type: 'max', name: 'Máximo' },
              { type: 'min', name: 'Mínimo' }
            ]
          },
          markLine: {
            data: [{ type: 'average', name: 'Media' }]
          }
        }
      ]
    };
  }

}
