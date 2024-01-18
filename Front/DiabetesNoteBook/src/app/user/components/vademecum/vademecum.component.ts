import { Component } from '@angular/core';
import { IMedicamento } from '../../interfaces/medicamento.interface';
import { VademecumService } from 'src/app/user/services/vademecum.service';

@Component({
  selector: 'app-vademecum',
  templateUrl: './vademecum.component.html',
  styleUrls: ['./vademecum.component.css'],
})
export class VademecumComponent {
  med: string =
    'lorazepam, Ibuprofeno, Paracetamol, Omeprazol, Amoxicilina, Diazepam, Trankimazin, Alprazolam, Dalsy, Nolotil,';

  arrayMed: IMedicamento[] = [];
  constructor(private vademecum: VademecumService) {}

  ngOnInit(): void {
    this.crearArrayMedicamentos();
  }

  crearArrayMedicamentos(): void {
    let nombresMedicamentos = this.med
      .split(',')
      .map((m) => m.trim())
      .filter((m) => m);
    nombresMedicamentos.forEach((nombre) => {
      this.vademecum.getMedicamentoInfo(nombre).subscribe({
        next: (res) => {
          this.arrayMed = [...this.arrayMed, res];
          console.log(this.arrayMed);
        },
        error: (err) => {
          console.log(err);
        },
      });
    });
  }
}
