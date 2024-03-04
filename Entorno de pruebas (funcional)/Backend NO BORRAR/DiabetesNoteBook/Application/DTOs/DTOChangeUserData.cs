using DiabetesNoteBook.Application.Validators;

namespace DiabetesNoteBook.Application.DTOs
{
    public class DTOChangeUserData
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        //Para que el servicio de chageEmailservice funcione descomentar la linea 13
       // public string EmailAntiguo { get; set; } //deshabilitar al terminar
		//public string NuevoEmail { get; set; }
        public string Email { get; set; }
		public string Sexo { get; set; }
        public int Edad { get; set; }
        public decimal Peso { get; set; }
        public decimal Altura { get; set; }
        public string Actividad { get; set; }
        public string TipoDiabetes { get; set; }
        public string[] Medicacion { get; set; }
        public bool Insulina { get; set; }

    }
}
