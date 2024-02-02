using System;
using System.Collections.Generic;

namespace DiabetesNoteBook.Domain.Models;

public partial class Medicacione
{
    public int IdMedicacion { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<PersonaMedicacion> PersonaMedicacions { get; set; } = new List<PersonaMedicacion>();
}
