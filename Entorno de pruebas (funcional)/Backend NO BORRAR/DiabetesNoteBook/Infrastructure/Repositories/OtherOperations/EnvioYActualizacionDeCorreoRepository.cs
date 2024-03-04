using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Infrastructure.Repositories.OtherOperations
{
	public class EnvioYActualizacionDeCorreoRepository: IActualizacionYEnvioDeCorreoElectronico
	{
		private readonly DiabetesNoteBookContext _context;
		private readonly IEmailService _emailService;

		public EnvioYActualizacionDeCorreoRepository(DiabetesNoteBookContext context, IEmailService emailService)
		{
			_context = context;
			_emailService = emailService;
		}
       
        public async Task<bool> ActualizarEmailUsuario(int userId, string nuevoEmail)
        {
            var usuarioActualizado = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == userId);

            if (usuarioActualizado != null)
            {
                if (usuarioActualizado.Email != nuevoEmail)
                {
                    usuarioActualizado.ConfirmacionEmail = false;
                    usuarioActualizado.Email = nuevoEmail;
                    _context.Usuarios.Update(usuarioActualizado);
                    await _context.SaveChangesAsync();
                    	

                    await _emailService.SendEmailAsyncRegister(new DTOEmail
                    {
                        ToEmail = nuevoEmail
                    });
                }
                else
                {
                    // No hay cambios en el email, pero aún así se envía el correo electrónico de confirmación
                    //await _emailService.SendEmailAsyncRegister(new DTOEmail
                    //{
                    //    ToEmail = nuevoEmail
                    //});
                }
                return true;
            }

            return false;
        }


        public async Task<bool> EnviarCorreoElectronico(string nuevoEmail)
		{
			await _emailService.SendEmailAsyncRegister(new DTOEmail
			{
				ToEmail = nuevoEmail
			});
			return true;
		}
	}
}
