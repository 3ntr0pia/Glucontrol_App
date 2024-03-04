using DiabetesNoteBook.Domain.Models;
using System.Security.Claims;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
	public interface IMedicionesIdUsuario
	{
		Task<List<Medicione>> ObtenerMedicionesUsuario(string userId);
	}
}
