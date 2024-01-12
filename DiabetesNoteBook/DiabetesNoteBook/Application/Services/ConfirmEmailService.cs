using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;

namespace DiabetesNoteBook.Application.Services
{
    public class ConfirmEmailService : IConfirmEmailService
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly IConfirmationRegisterRepository _confirmationRegisterRepository;

        public ConfirmEmailService(DiabetesNoteBookContext context, IConfirmationRegisterRepository confirmationRegisterRepository)
        {
            _context = context;
            _confirmationRegisterRepository = confirmationRegisterRepository;

        }

        public async Task ConfirmEmail(DTOConfirmRegistrtion confirm)
        {
            var usuarioUpdate = _context.Usuarios.AsTracking().FirstOrDefault(x => x.Id == confirm.UserId);

            usuarioUpdate.ConfirmacionEmail = true;

            await _confirmationRegisterRepository.ConfirmationRegisterTrue(usuarioUpdate);
        }
    }
}