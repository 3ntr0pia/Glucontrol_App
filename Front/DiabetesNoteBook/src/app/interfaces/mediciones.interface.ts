export interface IArrayMedicionesAzucar {
  arrayMediciones: IMedicionesAzucar[];
}
export interface IMedicionesAzucar {
  id: number;
  fecha: Date;
  regimen: string;
  preMedicion: number;
  postMedicion: number;
  glucemiaCapilar: number;
  bolusComida: number;
  bolusCorrector: number;
  preDeporte: number;
  duranteDeporte: number;
  postDeporte: number;
  racionHc: number;
  notas: string;
  idUsuario: number;
}

export enum Regimen {
  Desayuno = 'Desayuno',
  Comida = 'Comida',
  Merienda = 'Merienda',
  Cena = 'Cena',
}

export enum Comida {
  antes = 'antes',
  despues = 'despues',
}
