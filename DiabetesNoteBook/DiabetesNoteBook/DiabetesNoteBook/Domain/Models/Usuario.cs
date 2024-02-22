using System;
using System.Collections.Generic;

namespace DiabetesNoteBook.Domain.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Avatar { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public bool ConfirmacionEmail { get; set; }

    public bool BajaUsuario { get; set; }

    public string? EnlaceCambioPass { get; set; }

    public DateTime? FechaEnlaceCambioPass { get; set; }

    public string Nombre { get; set; } = null!;

    public string PrimerApellido { get; set; } = null!;

    public string SegundoApellido { get; set; } = null!;

    public string Sexo { get; set; } = null!;

    public int Edad { get; set; }

    public decimal Peso { get; set; }

    public decimal Altura { get; set; }

    public string TipoDiabetes { get; set; } = null!;

    public string Actividad { get; set; } = null!;

    public bool Insulina { get; set; }

    public string? Medicacion { get; set; }

    public virtual ICollection<Medicione> Mediciones { get; set; } = new List<Medicione>();

    public virtual ICollection<Operacione> Operaciones { get; set; } = new List<Operacione>();

    public virtual ICollection<UsuarioMedicacion> UsuarioMedicacions { get; set; } = new List<UsuarioMedicacion>();
}
