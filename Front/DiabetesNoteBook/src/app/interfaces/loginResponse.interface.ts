import { Sexo } from 'src/app/enums/register.enum';
export interface ILogin {
  UserName: string;
  Password: string;
}

export interface IUserLoginResponse {
  UserName: string;
  Password: string;
  email: string;
  token: string;
  rol: string;
  id: number;
  avatar: string;
  nombre: string;
  primerApellido: string;
  segundoApellido: string;
  userName: string;
  sexo: Sexo;
  edad: number;
  peso: number;
  altura: number;
  actividad: string;
  tipoDiabetes: string;
  medicacion: string;
  insulina: boolean;
}

export interface IUserLogout {
  email: string;
}

export interface IUserChangePassword {
  email: string;
  oldPassword?: string;
  newPassword: string;
}
export interface IRecover {
  token: string;
  newPass: string;
}
