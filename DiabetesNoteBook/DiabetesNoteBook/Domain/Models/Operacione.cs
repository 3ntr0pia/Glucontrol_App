using System;
using System.Collections.Generic;

namespace DiabetesNoteBook.Domain.Models;

public partial class Operacione
{
    public int Id { get; set; }

    public DateTime? FechaAccion { get; set; }

    public string? Operacion { get; set; }

    public string? Controller { get; set; }

    public string? Ip { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
