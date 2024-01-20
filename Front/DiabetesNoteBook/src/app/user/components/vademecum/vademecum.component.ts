import { Component } from '@angular/core';
import { IMedicamento, IRespuestaServicio } from '../../interfaces/medicamento.interface';
import { VademecumService } from 'src/app/user/services/vademecum.service';

@Component({
  selector: 'app-vademecum',
  templateUrl: './vademecum.component.html',
  styleUrls: ['./vademecum.component.css'],
})
export class VademecumComponent {
  medicamentosFromBackend: string[] = [
    'lorazepam',
    'paracetamol',
    'ibuprofeno',
  ];

  medicamentoSeleccionado : string = '';
  medicamentosArray: IMedicamento[] = [];
  Receta: boolean = true;
  Genericos: boolean = false;
  constructor(private vademecum: VademecumService) {}

  ngOnInit(): void {}

  medicamentoChange(){
    if(this.medicamentoSeleccionado){
      this.buscarMedicamentos(this.medicamentoSeleccionado);
    }
  }

  buscarMedicamentos(nombre: string) {
    nombre = this.medicamentoSeleccionado;
    this.vademecum.getMedicamentoInfo(nombre).subscribe({
      next: (res : IRespuestaServicio) => {
        this.medicamentosArray = res.resultados;
        console.log(this.medicamentosArray);
      },
      error: (error) => {
        console.log(error);
    }
    });
    
    
  }
}
