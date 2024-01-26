export interface IMedicionesAzucar {
    Fecha: Date;
    Regimen: Regimen;
    PreMedicion: number;
    GlucemiaCapilar: number;
    BolusComida: number;
    BolusCorrector: number;
    PreDeporte: number;
    DuranteDeporte: number;
    PostDeporte: number;
    Notas: string;
    Id_Persona: number;
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

