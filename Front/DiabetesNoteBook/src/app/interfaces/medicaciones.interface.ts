export interface IMedicacion {
  Id: number;
  medicacion: string[];
}

export interface IElminarMedicacion {
  userId: number;
  medicationId: number;
}
