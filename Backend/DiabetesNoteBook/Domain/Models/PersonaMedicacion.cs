using System;
using System.Collections.Generic;

namespace DiabetesNoteBook.Domain.Models;

public partial class PersonaMedicacion
{
    public int Id { get; set; }

    public int IdPersona { get; set; }

    public int IdMedicacion { get; set; }

    public virtual Medicacione IdMedicacionNavigation { get; set; } = null!;

    public virtual Persona IdPersonaNavigation { get; set; } = null!;
}
