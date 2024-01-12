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

        public ChangePasswordControllers(DiabetesNoteBookContext context, HashService hashService,
            IHttpContextAccessor httpContextAccessor, IEmailService emailService, IChangePassService changePassService,
            IOperationsService operationsService)
        {
            _context = context;
            _hashService = hashService;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _changePassService = changePassService;
            _operationsService = operationsService;
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

                await _changePassService.ChangePassId(new DTOCambioPassPorId
                {
                    Id = userData.Id,
                    NewPass = userData.NewPass
                });

                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Nuevo registro",
                    UserId = usuarioDB.Id
                });

                return Ok("Password cambiado con exito");
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido realizar el cambio de contraseña, por favor, intentelo más tarde.");
            }
        }

        //[AllowAnonymous]
        //[HttpPost("changePasswordMail")]
        //public async Task<ActionResult<Response>> SendInstruction(DTOUsuarioChangePasswordMail usuario)
        //{
        //    try
        //    {
        //        var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == usuario.Email);

        //        if (usuarioDB != null)
        //        {
        //            DateTime fecha = DateTime.Now.AddHours(+1);
        //            Guid miGuid = Guid.NewGuid();
        //            string textoEnlace = Convert.ToBase64String(miGuid.ToByteArray());
        //            textoEnlace = textoEnlace.Replace("=", "").Replace("+", "").Replace("/", "").Replace("?", "").Replace("&", "").Replace("!", "").Replace("¡", "");
        //            usuarioDB.EnlaceCambioPass = textoEnlace;
        //            usuarioDB.FechaEnlaceCambioPass = fecha;

        //            //await _operacionesService.AddOperacionChangePassword("ChangePasswordMail", "ChangePassword.ChangePasswordMail", usuarioDB.Id);
        //            await _context.SaveChangesAsync();

        //            var ruta = $"Para restablecer su contraseña haga click en este enlace: {_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/ChangePasswordControllers/changePasswordMail?enlace={textoEnlace}";

        //            //await _emailService.SendEmailAsync(new DTOEmail
        //            //{
        //            //    Body = ruta,
        //            //    Subject = "Restablecer contraseña",
        //            //    ToEmail = usuario.Email
        //            //});

        //            return Ok("Se enviaron las instrucciones a tu correo para restablecer la contraseña. Recuerda revisar la bandeja de Span.");

        //        }

        //        return BadRequest("Email no encontrado.");
        //    }
        //    catch
        //    {
        //        return BadRequest("En este momento no puedo enviar las intrucciones, intentalo más tarde por favor.");
        //    }
        //}

        //[AllowAnonymous]
        //[HttpPost("changePasswordMailConEnlace/{enlace}")]
        //public async Task<ActionResult<Response>> Reset([FromRoute] string enlace, [FromBody] DTOUsuarioChangePasswordMailConEnlace cambiopass)
        //{
        //    try
        //    {
        //        DateTime fecha = DateTime.Now;

        //        var userTokenExiste = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.EnlaceCambioPass == enlace);

        //        if (userTokenExiste.EnlaceCambioPass != null && userTokenExiste.FechaEnlaceCambioPass >= fecha)
        //        {
        //            var resultadoHash = _hashService.Hash(cambiopass.Password);
        //            userTokenExiste.Password = resultadoHash.Hash;
        //            userTokenExiste.Salt = resultadoHash.Salt;

        //            await _context.SaveChangesAsync();
        //            //await _operacionesService.AddOperacionChangePassword("ChangePasswordMailConEnlace", "ChangePassword.Reset", userTokenExiste.Id);
        //            return Ok("Password cambiado con exito");
        //        }

        //        return Ok("El token no existe o ha caducado.");

        //    }
        //    catch
        //    {
        //        return BadRequest("En este momento no se puede actualizar tu contraseña, intentelo más tarde por favor.");
        //    }
        //}

    }
}
