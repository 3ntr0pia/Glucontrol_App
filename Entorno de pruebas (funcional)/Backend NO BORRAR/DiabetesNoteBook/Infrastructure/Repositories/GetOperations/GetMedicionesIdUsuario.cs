using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DiabetesNoteBook.Infrastructure.Repositories.GetOperations
{
	public class GetMedicionesIdUsuario: IMedicionesIdUsuario
	{
		private readonly DiabetesNoteBookContext _context;

		public GetMedicionesIdUsuario(DiabetesNoteBookContext context)
		{
			_context = context;
		}
		public async Task<List<Medicione>> ObtenerMedicionesUsuario(string userId)
		{
			var mediciones= await _context.Mediciones
	   .Where(m => m.IdUsuarioNavigation.Id.ToString() == userId)
	   .ToListAsync();
			return mediciones;
		}


	}
}
