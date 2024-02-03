using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IChangeUserDataService
    {
        Task ChangeUserData(DTOChangeUserData changeUserData);
    }
}
