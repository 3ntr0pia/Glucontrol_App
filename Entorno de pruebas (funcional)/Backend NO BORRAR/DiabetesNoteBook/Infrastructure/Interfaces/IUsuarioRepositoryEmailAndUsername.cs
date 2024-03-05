using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiabetesNoteBook.Infrastructure.Interfaces
{
	public interface IUsuarioRepositoryEmailAndUsername
	{
		Task<Usuario> ObtenerUsuarioPorNombreOEmail(string nombre, string email);
	}
}
