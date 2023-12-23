using System;
using System.Collections.Generic;

namespace WebApiCadaGotaCuenta.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string? EnlaceCambioPass { get; set; }

    public virtual ICollection<Operacione> Operaciones { get; set; } = new List<Operacione>();

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
