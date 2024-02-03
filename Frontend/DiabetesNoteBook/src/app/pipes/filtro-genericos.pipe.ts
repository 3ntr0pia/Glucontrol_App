import { Pipe, PipeTransform } from '@angular/core';
import { IMedicamento } from '../interfaces/medicamento.interface';

@Pipe({
  name: 'filtroGenericos',
})
export class FiltroGenericosPipe implements PipeTransform {
  transform(medicamentos: IMedicamento[], genericos: boolean): IMedicamento[] {
    if (!genericos) {
      return medicamentos;
    }
    return medicamentos.filter((med) => med.generico);
  }
}
