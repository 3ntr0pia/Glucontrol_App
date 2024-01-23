export interface IMediciones {
    id: number;
    fecha: Date;
    regimen: string;
    preMedicion: number;
    postMedicion: number;
    glucemiaCapilar: number;
    bolusComida: number;
    bolusCorrector: number;
    preDeporte: number;
    duranteDeporte: number;
    postDeporte: number;
    notas: string;
    idPersona: number;
}