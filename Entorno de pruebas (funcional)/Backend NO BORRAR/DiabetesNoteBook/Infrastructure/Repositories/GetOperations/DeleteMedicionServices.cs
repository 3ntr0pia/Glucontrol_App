using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.Services;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Infrastructure.Repositories.GetOperations
{
	public class DeleteMedicionServices: IDeleteMedicionServices
	{
		private readonly DiabetesNoteBookContext _context;
	

		public DeleteMedicionServices(DiabetesNoteBookContext context)
		{
			_context = context;
			
		}
		public async Task<Medicione> ObtenerIdMedicion(int Id)
		{
			var obtenerMedicion = await _context.Mediciones.FirstOrDefaultAsync(x => x.Id == Id);
			
			return obtenerMedicion;
		}
	}
}
