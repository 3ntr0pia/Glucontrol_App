export interface IMedicionesAzucar {
    id : number;
    fecha : Date;
    regimen : Regimen;
    preMedicion : number;
    glucemiaCapilar : number;
    bolusComida : number;
    bolusCorrector : number;
    preDeporte : number;
    duranteDeporte : number;
    postDeporte : number;
    notas : string;
    racionHc : number;
    id_Persona : number;

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

