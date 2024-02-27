using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
    public interface INewUsuarioMedicacion
    {
        Task SaveNuevaUsuarioMedicacion(UsuarioMedicacion newusuariomedicacion);
    }
}
