using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
	public interface IDeleteMedicionServices
	{
		Task<Medicione> ObtenerIdMedicion(int Id);
	}
}
