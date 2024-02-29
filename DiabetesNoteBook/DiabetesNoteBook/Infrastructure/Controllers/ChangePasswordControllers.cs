using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Domain.Models;
using Azure;
using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.Services;

namespace DiabetesNoteBook.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChangePasswordControllers : ControllerBase
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly HashService _hashService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IChangePassService _changePassService;
        private readonly IOperationsService _operationsService;
        private readonly IChangePassMail _changePassMail;
        private readonly ILogger<UsersController> _logger;

        public ChangePasswordControllers(DiabetesNoteBookContext context, HashService hashService,
            IHttpContextAccessor httpContextAccessor, IEmailService emailService, IChangePassService changePassService,
            IOperationsService operationsService, IChangePassMail changePassMail, ILogger<UsersController> logger)
        {
            _context = context;
            _hashService = hashService;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _changePassService = changePassService;
            _operationsService = operationsService;
            _changePassMail = changePassMail;
            _logger = logger;

        }

        [HttpPut("changePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] DTOCambioPassPorId userData)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == userData.Id);

                if (usuarioDB == null)
                {
                    return Unauthorized("Operación no autorizada");
                }

                var resultadoHash = _hashService.Hash(userData.NewPass, usuarioDB.Salt);

                if (usuarioDB.Password == resultadoHash.Hash)
                {
                    return Unauthorized("La nueva contraseña no puede ser la misma.");
                }

                await _changePassService.ChangePassId(new DTOCambioPassPorId
                {
                    Id = userData.Id,
                    NewPass = userData.NewPass
                });

                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Cambio pass logado",
                    UserId = usuarioDB.Id
                });

                return Ok("Password cambiado con exito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar al cambiar la contraseña");
                return BadRequest("En estos momentos no se ha podido realizar el cambio de contraseña, por favor, intentelo más tarde.");
            }
        }

        [AllowAnonymous]
        [HttpPost("changePasswordMail")]
        public async Task<ActionResult<Response>> SendInstruction(DTOUsuarioChangePasswordMail usuario)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == usuario.Email);

                if (usuarioDB.Email is null)
                {
                    return Unauthorized("Este email no se encuentra registrado.");
                }

                if (usuarioDB.BajaUsuario == true)
                {
                    return Unauthorized("Usuario dado de baja con anterioridad");
                }

                if (usuarioDB != null)
                {

                    await _emailService.SendEmailAsyncChangePassword(new DTOEmail
                    {
                        ToEmail = usuario.Email
                    });

                    await _operationsService.AddOperacion(new DTOOperation
                    {
                        Operacion = "Cambio contraseña",
                        UserId = usuarioDB.Id
                    });

                    return Ok("Se enviaron las instrucciones a tu correo para restablecer la contraseña. Recuerda revisar la bandeja de Spam.");

                }

                return BadRequest("Email no encontrado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el envio de instrucciones");
                return BadRequest("En este momento no puedo enviar las intrucciones, intentalo más tarde por favor.");
            }
        }

        [AllowAnonymous]
        [HttpPost("changePasswordMailConEnlace")]
        public async Task<ActionResult<Response>> Reset([FromBody] DTOUsuarioChangePasswordMailConEnlace cambiopass)
        {
            try
            {
                var userTokenExiste = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.EnlaceCambioPass == cambiopass.Token);

                var resultadoHash = _hashService.Hash(cambiopass.NewPass, userTokenExiste.Salt);

                if (userTokenExiste.Password == resultadoHash.Hash)
                {
                    return Unauthorized("La nueva contraseña no puede ser la misma.");
                }

                DateTime fecha = DateTime.Now;

                if (userTokenExiste.EnlaceCambioPass != null && userTokenExiste.FechaEnlaceCambioPass >= fecha)
                {

                    await _changePassMail.ChangePassEnlaceMail(new DTOUsuarioChangePasswordMailConEnlace
                    {
                        NewPass = cambiopass.NewPass,
                        Token = cambiopass.Token

                    });

                    return Ok("Password cambiado con exito");
                }

                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Reseteo contraseña",
                    UserId = userTokenExiste.Id
                });

                return Ok("El token no existe o ha caducado.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el reset del email.");
                return BadRequest("En este momento no se puede actualizar tu contraseña, intentelo más tarde por favor.");
            }
        }

    }
}
