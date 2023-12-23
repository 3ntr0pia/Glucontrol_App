using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCadaGotaCuenta.DTOs;
using WebApiCadaGotaCuenta.Models;
using WebApiCadaGotaCuenta.Services;

namespace WebApiCadaGotaCuenta.Controllers
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

        public UsuariosController(GlucoControlContext context, TokenService tokenService, HashService hashService, OperacionesService operacionesService)
        {
            _context = context;
            _hashService = hashService;
            _tokenService = tokenService;
            _operacionesService = operacionesService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUsuario([FromBody] DTORegistroCompleto registroCompleto)
        {

            var userNameExiste = _context.Usuarios.FirstOrDefault(x => x.UserName == registroCompleto.Usuario.UserName);
            var emailNameExiste = _context.Usuarios.FirstOrDefault(x => x.Email == registroCompleto.Usuario.Email);

            if (userNameExiste != null)
            {
                return BadRequest("Usuario existente");
            }

            if (emailNameExiste != null)
            {
                return BadRequest("El email ya se encuentra registrado");
            }

            var resultadoHash = _hashService.Hash(registroCompleto.Usuario.Password);

            var newUsuario = new Usuario
            {
                UserName = registroCompleto.Usuario.UserName,
                Email = registroCompleto.Usuario.Email,
                Password = resultadoHash.Hash,
                Salt = resultadoHash.Salt,
                Rol = "user"
            };

            await _context.Usuarios.AddAsync(newUsuario);
            await _context.SaveChangesAsync();

            var nuevaPersona = new Persona
            {
                Nombre = registroCompleto.Persona.Nombre,
                PrimerApellido = registroCompleto.Persona.PrimerApellido,
                SegundoApellido = registroCompleto.Persona.SegundoApellido,
                Sexo = registroCompleto.Persona.Sexo,
                Edad = registroCompleto.Persona.Edad,
                Peso = registroCompleto.Persona.Peso,
                Altura = registroCompleto.Persona.Altura,
                TipoDiabetes = registroCompleto.Persona.TipoDiabetes,
                UserId = newUsuario.Id
            };

            int id = newUsuario.Id;

            await _context.Personas.AddAsync(nuevaPersona);
            await _context.SaveChangesAsync();
            await _operacionesService.AddOperacionRegister("Register", "Usuarios.Register", id);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LoginUsuario([FromBody] DTOLoginUsuario usuario)
        {
            var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.UserName == usuario.UserName);
            if (usuarioDB == null)
            {
                return Unauthorized("Usuario no encontrado.");
            }

            var resultadoHash = _hashService.Hash(usuario.Password, usuarioDB.Salt);
            if (usuarioDB.Password == resultadoHash.Hash)
            {
                var response = _tokenService.GenerarToken(usuarioDB);
                await _operacionesService.AddOperacionLogin("Login", "Usuarios.Login", usuario.UserName);
                return Ok(response);
            }
            else
            {
                return Unauthorized("Contraseña incorrecta.");
            }

        }

        [HttpPut("changepassword/{id}")]
        public async Task<ActionResult> LinkChangePasswordHash([FromRoute] int id,[FromBody] DTOUsuarioChangePassword infoUsuario)
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
            await _operacionesService.AddOperacionChangePassword("ChangePassword", "Usuarios.ChangePassword", id);

            return Ok("Password cambiado con exito");
        }

    }
}
