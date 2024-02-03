using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface IConfirmationRegisterRepository
    {
        Task ConfirmationRegisterTrue(Usuario confirm);
    }
}
