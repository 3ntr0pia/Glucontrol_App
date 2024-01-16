using System;
using System.Collections.Generic;

namespace DiabetesNoteBook.Domain.Models;

public partial class Persona
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string PrimerApellido { get; set; } = null!;

    public string SegundoApellido { get; set; } = null!;

    public string Sexo { get; set; } = null!;

    public int Edad { get; set; }

    public decimal Peso { get; set; }

    public decimal Altura { get; set; }

    public string TipoDiabetes { get; set; } = null!;

    public int UserId { get; set; }

    public string Actividad { get; set; } = null!;

    public bool Insulina { get; set; }

    public string? Medicacion { get; set; }

    public virtual ICollection<Medicione> Mediciones { get; set; } = new List<Medicione>();

    public virtual ICollection<PersonaMedicacion> PersonaMedicacions { get; set; } = new List<PersonaMedicacion>();

    public virtual Usuario User { get; set; } = null!;
}
