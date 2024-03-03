export interface IMedicionesAzucar {
  id: number;
  avatar: string;
  userName: string;
  email: string;
  password: string;
  salt: string;
  rol: string;
  confirmacionEmail: boolean;
  bajaUsuario: boolean;
  enlaceCambioPass: string;
  fechaEnlaceCambioPass: null;
  nombre: string;
  primerApellido: string;
  segundoApellido: string;
  sexo: string;
  edad: number;
  peso: number;
  altura: number;
  tipoDiabetes: string;
  actividad: string;
  insulina: true;
  mediciones: [];
  operaciones: [];
  usuarioMedicacions: [];
  id_Usuario: number;
  fecha: Date;
  regimen: Regimen;
  preMedicion: number;
  glucemiaCapilar: number;
  bolusComida: number;
  bolusCorrector: number;
  preDeporte: number;
  duranteDeporte: number;
  postDeporte: number;
  notas: string;
  racionHc: number;
}
/*
  "id": 1,
  "avatar": "https://api.dicebear.com/7.x/miniavs/svg?blushes=default&blushesProbability=99&bodyColor=3d287b&eyes=confident&glasses=normal&glassesProbability=24&hair=elvis&hairColor=af4055&head=wide&mouth=missingTooth&mustache=&mustacheProbability=11&skin=ffcb7e&backgroundColor=379f79",
  "userName": "Xaby72",
  "email": "xaby19@gmail.com",
  "password": "cfe/feV9QkBUmFkchVW1DCi5bmNy395ZSHSQu4ea84U=",
  "salt": "abldSNyHwlN7UBXcIz7b4Q==",
  "rol": "user",
  "confirmacionEmail": true,
  "bajaUsuario": false,
  "enlaceCambioPass": "AViMAOfl50SVAaFVq4lw4g",
  "fechaEnlaceCambioPass": null,
  "nombre": "Javier",
  "primerApellido": "Ortiz",
  "segundoApellido": "Ortiz",
  "sexo": "Hombre",
  "edad": 42,
  "peso": 75,
  "altura": 169,
  "tipoDiabetes": "Tipo 1",
  "actividad": "Ligero",
  "insulina": true,
  "mediciones": [],
  "operaciones": [],
  "usuarioMedicacions": []
*/
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
