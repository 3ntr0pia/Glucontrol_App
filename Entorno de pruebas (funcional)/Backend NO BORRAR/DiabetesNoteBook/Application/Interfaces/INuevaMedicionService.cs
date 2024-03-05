using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Interfaces
{
    public interface INuevaMedicionService
    {
        Task NuevaMedicion(DTOMediciones mediciones);
    }
}
