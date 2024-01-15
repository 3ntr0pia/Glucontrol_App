using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    public class DeleteService : IDeleteUserService
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly IDeleteUser _deleteUser;

        public DeleteService(DiabetesNoteBookContext context, IDeleteUser deleteUser)
        {
            _context = context;
            _deleteUser = deleteUser;
        }

        public async Task DeleteUserService(DTODeleteUser delete)
        {

            var usuarioDB = await _context.Usuarios.Include(x => x.Personas).FirstOrDefaultAsync(x => x.Id == delete.Id);

            await _deleteUser.DeleteUserRepository(usuarioDB);
        }
    }
}
