import { Pipe, PipeTransform } from '@angular/core';
import { IMedicamento } from '../interfaces/medicamento.interface';

@Pipe({
  name: 'filtroSinReceta',
})
export class FiltroSinRecetaPipe implements PipeTransform {
  transform(medicamentos: IMedicamento[], Receta: boolean): IMedicamento[] {
    if (Receta) {
      return medicamentos;
    }
    return medicamentos.filter((med) => med.receta === Receta);
  }
}
