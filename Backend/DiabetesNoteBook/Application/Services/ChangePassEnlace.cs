using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    public class ChangePassEnlace : IChangePassMail
    {
        private readonly HashService _hashService;
        private readonly DiabetesNoteBookContext _context;
        private readonly IChangePasswordMail _changePasswordMail;

        public ChangePassEnlace(HashService hashService, DiabetesNoteBookContext context, IChangePasswordMail changePasswordMail)
        {
            _hashService = hashService;
            _context = context;
            _changePasswordMail = changePasswordMail;
        }
        public async Task ChangePassEnlaceMail(DTOUsuarioChangePasswordMailConEnlace userData)
        {
            var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.EnlaceCambioPass == userData.Token);

            var resultadoHash = _hashService.Hash(userData.NewPass);
            usuarioDB.Password = resultadoHash.Hash;
            usuarioDB.Salt = resultadoHash.Salt;

            await _changePasswordMail.SaveNewPasswordMail(usuarioDB);
        }
    }
}
