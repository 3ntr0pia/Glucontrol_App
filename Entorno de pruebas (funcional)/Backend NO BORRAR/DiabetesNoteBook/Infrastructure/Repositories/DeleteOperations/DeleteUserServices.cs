using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Infrastructure.Repositories.DeleteOperations
{
	public class DeleteUserServices:IDeleteUserServices
	{
		private readonly DiabetesNoteBookContext _context;

		public DeleteUserServices(DiabetesNoteBookContext context)
		{
			_context = context;
		}
		public async Task<Usuario> ObtenerUsuarioConRelaciones(int id)
		{
			var eliminarUsuario= await  _context.Usuarios
			.Include(x => x.Mediciones)
			.Include(x => x.UsuarioMedicacions)
			
			.FirstOrDefaultAsync(x => x.Id == id);
			return eliminarUsuario;
		}
	}
}
