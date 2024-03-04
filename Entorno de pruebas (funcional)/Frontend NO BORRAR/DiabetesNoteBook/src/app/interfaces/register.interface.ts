import { Sexo, Actividad, TipoDiabetes } from '../enums/register.enum';

export interface IRegister {
  avatar: string;
  username: string;
  nombre: string;
  apellido: string;
  apellido2: string;
  email: string;
  password: string;
  password2: string;
  mediciones: IMediciones;
}

export interface IMediciones {
  edad: number;
  peso: number;
  altura: number;
  sexo: Sexo;
  actividad: Actividad;
  tipoDiabetes: IDiabetes;
}

export interface IDiabetes {
  tipo: TipoDiabetes;
  medicacion: string[];
  insulina: boolean;
}
