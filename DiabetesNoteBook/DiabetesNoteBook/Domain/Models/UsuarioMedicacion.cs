using System;
using System.Collections.Generic;

namespace DiabetesNoteBook.Domain.Models;

public partial class UsuarioMedicacion
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdMedicacion { get; set; }

    public virtual Medicacione IdMedicacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
