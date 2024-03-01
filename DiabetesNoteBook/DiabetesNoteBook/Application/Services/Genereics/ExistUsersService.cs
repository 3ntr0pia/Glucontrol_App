using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services.Genereics
{
    public class ExistUsersService
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly ILogger<UsersController> _logger;

        public ExistUsersService(DiabetesNoteBookContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> UserNameExist(string userName)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.UserName == userName);

                return usuarioDB != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta de nombre de usuario existente");
                throw new Exception("Error al procesar la solicitud");
            }
        }

        public async Task<bool> EmailExist(string email)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == email);

                return usuarioDB != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta de email existente");
                throw new Exception("Error al procesar la solicitud");
            }
        }

        public async Task<Usuario> UserExistById(int id)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                return usuarioDB;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta de id de usuario existente");
                throw new Exception("Error al procesar la solicitud");
            }
        }

        public async Task<Usuario> UserExistByUserName(string userName)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.UserName == userName);

                return usuarioDB;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta de id de usuario existente");
                throw new Exception("Error al procesar la solicitud");
            }
        }

        public async Task<bool> UserExistByEmail(string email)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.ConfirmacionEmail == true);

                return usuarioDB != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta de email de usuario existente");
                throw new Exception("Error al procesar la solicitud");
            }
        }

        public async Task<Usuario> UserTokenExist(DTOUsuarioChangePasswordMailConEnlace userName)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.EnlaceCambioPass == userName.Token);

                return usuarioDB;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta de id de usuario existente");
                throw new Exception("Error al procesar la solicitud");
            }
        }
    }
}
