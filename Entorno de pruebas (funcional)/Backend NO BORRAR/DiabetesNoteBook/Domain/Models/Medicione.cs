using System;
using System.Collections.Generic;

namespace DiabetesNoteBook.Domain.Models;

public partial class Medicione
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string Regimen { get; set; } = null!;

    public decimal PreMedicion { get; set; }

    public decimal? PostMedicion { get; set; }

    public decimal GlucemiaCapilar { get; set; }

    public decimal? BolusComida { get; set; }

    public decimal? BolusCorrector { get; set; }

    public decimal? PreDeporte { get; set; }

    public decimal? DuranteDeporte { get; set; }

    public decimal? PostDeporte { get; set; }

    public decimal? RacionHc { get; set; }

    public string Notas { get; set; } = null!;

    public int IdUsuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
