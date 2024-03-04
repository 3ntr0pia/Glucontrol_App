using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Infrastructure.Repositories.GetOperations
{
	public class UsuarioRepositoryEmailAndUsername: IUsuarioRepositoryEmailAndUsername
	{
		private readonly DiabetesNoteBookContext _context;

		public UsuarioRepositoryEmailAndUsername(DiabetesNoteBookContext context)
		{
			_context = context;
		}
		public async Task<Usuario> ObtenerUsuarioPorNombreOEmail(string nombre, string email)
		{
			return await _context.Usuarios.FirstOrDefaultAsync(x => x.UserName == nombre || x.Email == email);
		}
	}
}
