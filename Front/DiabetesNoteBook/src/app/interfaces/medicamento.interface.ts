export interface IRespuestaServicio {
  totalFilas: number;
  pagina: number;
  tamanioPagina: number;
  resultados: IMedicamento[];
}
export interface IMedicamento {
  nregistro: string;
  nombre: string;
  labtitular: string;
  cpresc: string;
  estado: {
    aut: number;
  };
  comerc: boolean;
  receta: boolean;
  generico: boolean;
  conduc: boolean;
  triangulo: boolean;
  huerfano: boolean;
  biosimilar: boolean;
  nosustituible: {
    id: number;
    nombre: string;
  };
  psum: boolean;
  notas: boolean;
  materialesInf: boolean;
  ema: boolean;
  docs: IDocumento[];
  fotos: IFoto[];
  viasAdministracion: IViaAdministracion[];
  formaFarmaceutica: IFormaFarmaceutica;
  formaFarmaceuticaSimplificada: IFormaFarmaceutica;
  vtm: {
    id: number;
    nombre: string;
  };
  dosis: string;
}

export interface IDocumento {
  tipo: number;
  url: string;
  secc: boolean;
  fecha: number;
  urlHtml?: string;
}

export interface IFoto {
  tipo: string;
  url: string;
  fecha: number;
}

export interface IViaAdministracion {
  id: number;
  nombre: string;
}

export interface IFormaFarmaceutica {
  id: number;
  nombre: string;
}
