using Azure;
using CapaDominio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using CapaInfraestructura.DTOs;
using CapaAplicacion.Services;
using Microsoft.AspNetCore.Http;
using CapaAplicacion.Interfaces;

namespace CapaInfraestructura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChangePasswordControllers : ControllerBase
    {
        private readonly GlucoControlContext _context;
        private readonly HashService _hashService;
        private readonly OperacionesService _operacionesService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public ChangePasswordControllers(GlucoControlContext context, HashService hashService,
            OperacionesService operacionesService, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _context = context;
            _hashService = hashService;
            _operacionesService = operacionesService;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        [HttpPut("changePassword/{id}")]
        public async Task<ActionResult> LinkChangePasswordHash([FromRoute] int id, [FromBody] DTOUsuarioChangePassword infoUsuario)
        {
            var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (usuarioDB == null)
            {
                return Unauthorized("Operación no autorizada");
            }

            var resultadoHash = _hashService.Hash(infoUsuario.Password);
            usuarioDB.Password = resultadoHash.Hash;
            usuarioDB.Salt = resultadoHash.Salt;

            await _context.SaveChangesAsync();
            await _operacionesService.AddOperacionChangePassword("ChangePassword", "ChangePassword.ChangePassword", id);

            return Ok("Password cambiado con exito");
        }

        [AllowAnonymous]
        [HttpPost("changePasswordMail")]
        public async Task<ActionResult<Response>> SendInstruction(DTOUsuarioChangePasswordMail usuario)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == usuario.Email);
                if (usuarioDB != null)
                {
                    DateTime fecha = DateTime.Now.AddHours(+1);
                    Guid miGuid = Guid.NewGuid();
                    string textoEnlace = Convert.ToBase64String(miGuid.ToByteArray());
                    textoEnlace = textoEnlace.Replace("=", "").Replace("+", "").Replace("/", "").Replace("?", "").Replace("&", "").Replace("!", "").Replace("¡", "");
                    usuarioDB.EnlaceCambioPass = textoEnlace;
                    usuarioDB.FechaEnlaceCambioPass = fecha;

                    await _operacionesService.AddOperacionChangePassword("ChangePasswordMail", "ChangePassword.ChangePasswordMail", usuarioDB.Id);
                    await _context.SaveChangesAsync();

                    var ruta = $"Para restablecer su contraseña haga click en este enlace: {_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/ChangePasswordControllers/changePasswordMail?enlace={textoEnlace}";

                    await _emailService.SendEmailAsync(new CapaAplicacion.DTOs.DTOEmail
                    {
                        Body = ruta,
                        Subject = "Restablecer contraseña",
                        ToEmail = usuario.Email
                    });

                    return Ok("Se enviaron las instrucciones a tu correo para restablecer la contraseña. Recuerda revisar la bandeja de Span.");

                }

                return BadRequest("Email no encontrado.");
            }
            catch
            {
                return BadRequest("En este momento no puedo enviar las intrucciones, intentalo más tarde por favor.");
            }
        }

        [AllowAnonymous]
        [HttpPost("changePasswordMailConEnlace/{enlace}")]
        public async Task<ActionResult<Response>> Reset([FromRoute] string enlace, [FromBody] DTOUsuarioChangePasswordMailConEnlace cambiopass)
        {
            try
            {
                DateTime fecha = DateTime.Now;

                var userTokenExiste = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.EnlaceCambioPass == enlace);

                if (userTokenExiste.EnlaceCambioPass != null && userTokenExiste.FechaEnlaceCambioPass >= fecha)
                {
                    var resultadoHash = _hashService.Hash(cambiopass.Password);
                    userTokenExiste.Password = resultadoHash.Hash;
                    userTokenExiste.Salt = resultadoHash.Salt;

                    await _context.SaveChangesAsync();
                    await _operacionesService.AddOperacionChangePassword("ChangePasswordMailConEnlace", "ChangePassword.Reset", userTokenExiste.Id);
                    return Ok("Password cambiado con exito");
                }

                return Ok("El token no existe o ha caducado.");

            }
            catch
            {
                return BadRequest("En este momento no se puede actualizar tu contraseña, intentelo más tarde por favor.");
            }
        }

        [AllowAnonymous]
        [HttpPost("confirmarEmail")]
        public async Task<ActionResult<Response>> ConfirmarEmail([FromRoute] string enlace)
        {
            try
            {

                var userTokenExiste = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.EnlaceCambioPass == enlace);

                if (userTokenExiste.EnlaceCambioPass != null)
                {

                    var linkConfirmacion = "?enlace=" + WebUtility.UrlDecode(enlace);

                    await _context.SaveChangesAsync();
                    await _operacionesService.AddOperacionChangePassword("ConfirmaciónEmail", "ChangePassword.confirmarEmail", userTokenExiste.Id);
                    return Ok("Email confirmado con éxito.");
                }

                return Ok("El token no existe.");

            }
            catch
            {
                return BadRequest("En este momento no se pconfirmar el email, intentelo más tarde por favor.");
            }
        }


    }
}
