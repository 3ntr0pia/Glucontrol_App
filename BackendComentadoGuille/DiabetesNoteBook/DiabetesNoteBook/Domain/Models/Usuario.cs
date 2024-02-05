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

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
