﻿using System;
using System.Collections.Generic;

namespace WebApiCadaGotaCuenta.Models;

public partial class Persona
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string PrimerApellido { get; set; } = null!;

    public string SegundoApellido { get; set; } = null!;

    public string Sexo { get; set; } = null!;

    public int Edad { get; set; }

    public int Peso { get; set; }

    public decimal Altura { get; set; }

    public string TipoDiabetes { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<Medicione> Mediciones { get; set; } = new List<Medicione>();

    public virtual Usuario User { get; set; } = null!;
}