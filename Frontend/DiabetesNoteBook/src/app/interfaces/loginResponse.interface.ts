export interface ILogin {
  UserName: string;
  Password: string;
}

export interface IUserLoginResponse {
  token: string;
  rol: string;
  id: number;
  idPersona: number;
  avatar: string;
  userId: string;
  nombre: string;
  primerApellido: string;
  segundoApellido: string;
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
