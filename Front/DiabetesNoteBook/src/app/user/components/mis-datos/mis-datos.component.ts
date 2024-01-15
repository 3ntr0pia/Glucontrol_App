import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-mis-datos',
  templateUrl: './mis-datos.component.html',
  styleUrls: ['./mis-datos.component.css']
})

//AfterViewIniit es un hook de ciclo de vida que se ejecuta después de que Angular haya inicializado las vistas del componente.
//Es mejor usarlo que onInit por que onInit se ejecuta antes de que Angular inicialice las vistas del componente y eso puede causar errores.
export class MisDatosComponent implements AfterViewInit {
  
  @ViewChild('graficaTest') graficaTest!: ElementRef;

 
 ngAfterViewInit() {
  this.crearGrafico();
} 


  crearGrafico() :void{
    const data = [
      { year: 2010, count: 10 },
      { year: 2011, count: 20 },
      { year: 2012, count: 15 },
      { year: 2013, count: 25 },
      { year: 2014, count: 22 },
      { year: 2015, count: 30 },
      { year: 2016, count: 28 },
    ];
  
      new Chart(this.graficaTest.nativeElement, {
        type: 'bar',
        data: {
          labels: data.map(row => row.year),
          datasets: [
            {
              label: 'Adquisiciones por año',
              data: data.map(row => row.count),
              // Puedes agregar opciones de estilo aquí
            }
          ]
        }
        // Aquí puedes agregar opciones adicionales para el gráfico
      });
    }

    
    
  }
  



