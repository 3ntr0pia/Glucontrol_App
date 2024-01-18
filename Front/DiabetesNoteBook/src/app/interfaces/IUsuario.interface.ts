export interface IUserLoginResponse {
    token: string;
    rol : string;
    idUsuario: number;
    avatar: string;
    nombre : string;
    primerApellido : string;
    segundoApellido : string;
  }
  
  export interface IUserLogout {
    email: string;
  }
  
  export interface IUserChangePassword {
    email: string;
    oldPassword?: string;
    newPassword: string;
  }
  