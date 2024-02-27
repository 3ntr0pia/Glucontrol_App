using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface IDeleteUserMedication
    {
        Task DeleteUserMedications(UsuarioMedicacion confirm);
    }
}
