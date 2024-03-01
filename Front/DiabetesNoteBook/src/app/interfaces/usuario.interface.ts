import { Sexo } from 'src/app/enums/register.enum';

export interface IUsuarioUpdate {
  id?: number;
  email?: string;
  avatar: string;
  userName: string;
  nombre: string;
  primerApellido: string;
  segundoApellido: string;
  sexo: Sexo;
  edad: number;
  peso: number;
  altura: number;
  actividad: string;
  tipoDiabetes: string;
  medicacion: string;
  insulina: boolean;
}
