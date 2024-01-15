using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IUserDeregistrationService
    {
        Task UserDeregistration(DTOUserDeregistration delete);
    }
}
