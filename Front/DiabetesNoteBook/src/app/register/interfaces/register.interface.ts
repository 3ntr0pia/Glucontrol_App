export interface IRegister{
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

export enum Actividad{
    sedentario = "Sedentario",
    ligero = "Ligero",
    moderado = "Moderado",
    intenso = "Intenso"
}

export enum Sexo{
    hombre = "Hombre",
    mujer = "Mujer"
}
export enum TipoDiabetes{
    tipo1 = "Tipo 1",
    tipo2 = "Tipo 2",
    gestacional = "Gestacional"
}

export interface IDiabetes{
    tipo : TipoDiabetes;
    fecha_diagnostico : Date;
    medicacion: string[];
    insulina: boolean;
}