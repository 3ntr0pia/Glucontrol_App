using DiabetesNoteBook.Domain.Models;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
	public interface IDeleteUserServices
	{
		Task<Usuario> ObtenerUsuarioConRelaciones(int id);
	}
}
