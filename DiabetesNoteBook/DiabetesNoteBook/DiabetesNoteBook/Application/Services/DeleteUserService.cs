using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    public class DeleteUserService : IDeleteUserService
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly IDeleteUser _deleteUser;

        public DeleteUserService(DiabetesNoteBookContext context, IDeleteUser deleteUser)
        {
            _context = context;
            _deleteUser = deleteUser;
        }

        public async Task DeleteUser(DTODeleteUser delete)
        {

            var usuarioDB = await _context.Usuarios
            .Include(u => u.Mediciones)
            .Include(u => u.UsuarioMedicacions)
            .Include(u => u.Operaciones)
            .FirstOrDefaultAsync(x => x.Id == delete.Id);

            await _deleteUser.DeleteUser(usuarioDB);
        }

    }
}
