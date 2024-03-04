using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface IAddNuevaMedicacion
    {
        Task AddNuevaMedicacion(Medicacione medicacion);
    }
}
