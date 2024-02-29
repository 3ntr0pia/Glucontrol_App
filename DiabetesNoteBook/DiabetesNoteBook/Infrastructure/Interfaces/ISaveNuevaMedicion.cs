using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface ISaveNuevaMedicion
    {
        Task SaveNuevaMecion(Medicione newmedicion);
    }
}
