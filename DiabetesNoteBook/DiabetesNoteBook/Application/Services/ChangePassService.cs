using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Services
{
    public class ChangePassService : IChangePassService
    {
        private readonly HashService _hashService;
        private readonly DiabetesNoteBookContext _context;
        private readonly IChangePassword _changePassword;

        public ChangePassService(HashService hashService, DiabetesNoteBookContext context, IChangePassword changePassword)
        {
            _hashService = hashService;
            _context = context;
            _changePassword = changePassword;
        }

        public async Task ChangePassId(DTOCambioPassPorId userData)
        {
            var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == userData.Id);

            var resultadoHash = _hashService.Hash(userData.NewPass);
            usuarioDB.Password = resultadoHash.Hash;
            usuarioDB.Salt = resultadoHash.Salt;

            await _changePassword.SaveNewPassword(usuarioDB);
        }
    }
}
