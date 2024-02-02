using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface IDeleteMedicion
    {
        Task DeleteMedicion(Medicione delete);
    }
}
