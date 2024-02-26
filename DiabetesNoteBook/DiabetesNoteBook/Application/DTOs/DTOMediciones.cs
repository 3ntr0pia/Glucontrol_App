namespace DiabetesNoteBook.Application.DTOs
{
    public class DTOMediciones
    {
        public DateTime Fecha { get; set; }
        public string Regimen { get; set; }
        public decimal PreMedicion { get; set; }
        public decimal PostMedicion { get; set; }
        public decimal GlucemiaCapilar { get; set; }
        public decimal BolusComida { get; set; }
        public decimal BolusCorrector { get; set; }
        public decimal PreDeporte { get; set; }
        public decimal DuranteDeporte { get; set; }
        public decimal PostDeporte { get; set; }
        public decimal RacionHC { get; set; }
        public string Notas { get; set; }
        public int Id_Usuario { get; set; }
    }
}
