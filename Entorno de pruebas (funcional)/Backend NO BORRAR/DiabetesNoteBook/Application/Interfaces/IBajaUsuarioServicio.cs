using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface IBajaUsuarioServicio
    {
        Task UserDeregistration(DTOUserDeregistration delete);
    }
}
