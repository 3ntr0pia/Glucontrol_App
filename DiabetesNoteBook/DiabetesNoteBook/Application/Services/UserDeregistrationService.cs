using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    public class UserDeregistrationService : IUserDeregistrationService
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly IUserDeregistration _userDeregistration;

        public UserDeregistrationService(DiabetesNoteBookContext context, IUserDeregistration userDeregistration)
        {
            _context = context;
            _userDeregistration = userDeregistration;
        }

        public async Task UserDeregistration(DTOUserDeregistration delete)
        {

            var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == delete.Id);

            usuarioDB.BajaUsuario = true;

            await _userDeregistration.UserDeregistrationSave(usuarioDB);
        }
    }
}
