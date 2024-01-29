using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface INewRegisterRepository
    {
        Task SaveNewRegisterUser(Usuario newUsuario);
        Task SaveNewRegisterPerson(Persona nuevaPersona);
    }
}
