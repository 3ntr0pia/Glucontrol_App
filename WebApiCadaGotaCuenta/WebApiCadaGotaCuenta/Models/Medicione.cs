using System;
using System.Collections.Generic;

namespace WebApiCadaGotaCuenta.Models;

public partial class Medicione
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string Regimen { get; set; } = null!;

    public decimal PreMedicion { get; set; }

    public decimal PostMedicion { get; set; }

    public decimal? GlucemiaCapilar { get; set; }

    public decimal? BolusComida { get; set; }

    public decimal BolusCorrector { get; set; }

    public decimal CuerposCetonicos { get; set; }

    public string Notas { get; set; } = null!;

    public int IdPersona { get; set; }

    public virtual Persona IdPersonaNavigation { get; set; } = null!;
}
