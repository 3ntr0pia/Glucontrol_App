using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapaInfraestructura.DTOs;
using CapaDominio.Models;
using CapaAplicacion.Services;
using Microsoft.EntityFrameworkCore;
using CapaAplicacion.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CapaInfraestructura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly GlucoControlContext _context;
        private readonly HashService _hashService;
        private readonly TokenService _tokenService;
        private readonly OperacionesService _operacionesService;
        private readonly INuevoUsuarioService _nuevoUsuarioService;
        private readonly INuevaPersonaService _nuevaPersonaService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IConfirmarEmailService _confirmarEmailService;

        public UsuariosController(GlucoControlContext context, TokenService tokenService, HashService hashService,
            OperacionesService operacionesService, INuevoUsuarioService nuevoUsuarioService, INuevaPersonaService nuevaPersonaService,
            IHttpContextAccessor httpContextAccessor, IEmailService emailService, IConfirmarEmailService confirmarEmailService)
        {
            _context = context;
            _hashService = hashService;
            _tokenService = tokenService;
            _operacionesService = operacionesService;
            _nuevoUsuarioService = nuevoUsuarioService;
            _nuevaPersonaService = nuevaPersonaService;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _confirmarEmailService = confirmarEmailService;
        }

        [AllowAnonymous]
        [HttpPost("registro")]
        public async Task<ActionResult> RegisterUsuario([FromBody] DTORegistroCompleto registroCompleto)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var userNameExiste = _context.Usuarios.FirstOrDefault(x => x.UserName == registroCompleto.Usuario.UserName);

                if (userNameExiste != null)
                {
                    return BadRequest("Usuario existente");
                }

                var emailNameExiste = _context.Usuarios.FirstOrDefault(x => x.Email == registroCompleto.Usuario.Email);

                if (emailNameExiste != null)
                {
                    return BadRequest("El email ya se encuentra registrado");
                }

                // Esperar a que se complete la operación de registro de usuario
                await _nuevoUsuarioService.ResgitroUsuario(new CapaAplicacion.DTOs.DTOUsuarioRegistro
                {
                    UserName = registroCompleto.Usuario.UserName,
                    Email = registroCompleto.Usuario.Email,
                    Password = registroCompleto.Usuario.Password
                });

                // Recuperar el usuario después de que la operación de registro se haya completado
                var usuario = _context.Usuarios.FirstOrDefault(x => x.UserName == registroCompleto.Usuario.UserName);

                // Verificar si el usuario se ha creado correctamente
                if (usuario == null)
                {
                    return BadRequest("Error al registrar el usuario");
                }

                // Esperar a que se complete la operación de registro de persona
                await _nuevaPersonaService.ResgitroPersona(new CapaAplicacion.DTOs.DTOPersonaRegistro
                {
                    Nombre = registroCompleto.Persona.Nombre,
                    PrimerApellido = registroCompleto.Persona.PrimerApellido,
                    SegundoApellido = registroCompleto.Persona.SegundoApellido,
                    Sexo = registroCompleto.Persona.Sexo,
                    Edad = registroCompleto.Persona.Edad,
                    Peso = registroCompleto.Persona.Peso,
                    Altura = registroCompleto.Persona.Altura,
                    TipoDiabetes = registroCompleto.Persona.TipoDiabetes,
                    UserId = usuario.Id
                });

                var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == registroCompleto.Usuario.Email);

                Guid miGuid = Guid.NewGuid();
                string textoEnlace = Convert.ToBase64String(miGuid.ToByteArray());
                textoEnlace = textoEnlace.Replace("=", "").Replace("+", "").Replace("/", "").Replace("?", "").Replace("&", "").Replace("!", "").Replace("¡", "");
                usuarioDB.EnlaceCambioPass = textoEnlace;

                await _context.SaveChangesAsync();
                await _operacionesService.AddOperacionChangePassword("ChangePasswordMail", "ChangePassword.ChangePasswordMail", usuarioDB.Id);

                var ruta = $"Para confirmar su email haga click en este enlace: <a href='{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Usuarios/validarRegistro/{usuarioDB.Id}/{textoEnlace}'>Confirmar Email</a>";

                await _emailService.SendEmailAsync(new CapaAplicacion.DTOs.DTOEmail
                {
                    Body = ruta,
                    Subject = "Confirmar Email",
                    ToEmail = usuario.Email,
                    IsHtml = true  
                });

                // Confirmar la transacción si ambas operaciones se han completado correctamente
                transaction.Commit();

                return Ok();
            }
            catch
            {
                // Revertir la transacción en caso de error
                transaction.Rollback();
                return BadRequest("En estos momentos no se ha podido realizar le registro, por favor, intentelo más tarde.");
            }
        }

        [AllowAnonymous]
        [HttpGet("validarRegistro/{UserId}/{Token}")]
        public async Task<ActionResult> ValidarRegistro([FromRoute] DTOComprobarRegistro confirmacion)
        {

            try
            {
                var estaConfirmado = _context.Usuarios.FirstOrDefault(x => x.Id == confirmacion.UserId);

                if (estaConfirmado?.ConfirmacionEmail == true)
                {
                    return BadRequest("Usuario ya validado con anterioridad.");
                }

                await _confirmarEmailService.ConfirmacionEmail(new CapaAplicacion.DTOs.DTOComprobarRegistro
                {
                    UserId = confirmacion.UserId
                });

                return Ok();
                

            }
            catch
            {

                return BadRequest("En estos momentos no se ha podido validar el registro, por favor, intentelo de nuevo más tarde.");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LoginUsuario([FromBody] DTOLoginUsuario usuario)
        {

            try
            {

                var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.UserName == usuario.UserName);

                if (usuarioDB == null)
                {
                    return Unauthorized("Usuario no encontrado.");
                }

                if (usuarioDB.ConfirmacionEmail != true)
                {
                    return Unauthorized("Usuario no confirmado, por favor acceda a su correo y valida su registro.");
                }

                var resultadoHash = _hashService.Hash(usuario.Password, usuarioDB.Salt);

                if (usuarioDB.Password == resultadoHash.Hash)
                {
                    var response = _tokenService.GenerarToken(usuarioDB);
                    await _operacionesService.AddOperacionLogin("Login", "UsuariosController", usuario.UserName);
                    return Ok(response);
                }
                else
                {
                    return Unauthorized("Contraseña incorrecta.");
                }

            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido realizar el login, por favor, intentelo más tarde.");
            }

        }

    }
}
