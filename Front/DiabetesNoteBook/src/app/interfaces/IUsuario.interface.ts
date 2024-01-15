export interface IUserLoginResponse {
    idUsuario: number;
    avatar: string;
    nombre : string;
    apellido : string;
    apellido2 : string;
  }
  
  export interface IUserLogout {
    email: string;
  }
  
  export interface IUserChangePassword {
    email: string;
    oldPassword?: string;
    newPassword: string;
  }
  