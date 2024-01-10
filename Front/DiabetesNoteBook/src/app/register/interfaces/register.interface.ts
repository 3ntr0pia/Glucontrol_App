import { Sexo, Actividad, TipoDiabetes } from './register.enum';

export interface IRegister{
    avatar: string;
    nombre: string;
    apellido: string;
    apellido2: string;
    email: string;
    password: string;
    password2: string;
    mediciones : IMediciones;
}

export interface IMediciones{
    edad : number;
    peso : number;
    altura : number;
    sexo : Sexo;
    actividad : Actividad;
    tipoDiabetes : IDiabetes;  
}


export interface IDiabetes{
    tipo : TipoDiabetes;
    fecha_diagnostico : Date;
    medicacion: IPill[];
    insulina: boolean;
}

export interface IPill {
    nombre : string;
    color : string;
    forma: string;
}