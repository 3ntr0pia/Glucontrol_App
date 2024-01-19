namespace WebApiCadaGotaCuenta.DTOs
{
    public class DTOMediciones
    {
        public DateTime Fecha { get; set; }
        public string Regimen { get; set; }
        public decimal PreMedicion { get; set; }
        public decimal PostMedicion { get; set; }
        public decimal GleucemiaCapilar { get; set; }
        public decimal BolusComida { get; set; }
        public decimal BolusCorrector { get; set; }
        public decimal CuerposCetonicos { get; set; }
        public string Notas { get; set; }
        public int IdPersona { get; set; }
    }
}
