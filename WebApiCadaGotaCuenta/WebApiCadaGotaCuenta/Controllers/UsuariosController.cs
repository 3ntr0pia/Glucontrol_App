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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenService _tokenService;

        public UsuariosController(GlucoControlContext context, TokenService tokenService, HashService hashService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _hashService = hashService;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] DTOUsuarioRegistro usuario)
        {
            var resultadoHash = _hashService.Hash(usuario.Password);

            var newUsuario = new Usuario
            {
                UserName = usuario.UserName,
                Email = usuario.Email,
                Password = resultadoHash.Hash,
                Salt = resultadoHash.Salt,
            };

            await _context.Usuarios.AddAsync(newUsuario);
            await _context.SaveChangesAsync();

            return Ok(newUsuario);
        }

        //[AllowAnonymous]
        //[HttpPost("login")]
        //public async Task<ActionResult> LoginUsuario([FromBody] DTOUsuario usuario)
        //{
        //    var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuario.Email);
        //    if (usuarioDB == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var resultadoHash = _hashService.Hash(usuario.Password, usuarioDB.Salt);
        //    if (usuarioDB.Password == resultadoHash.Hash)
        //    {
        //        var response = _tokenService.GenerarToken(usuarioDB);
        //        return Ok(response);
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}

        //[HttpPost("linkchangepassword")]
        //public async Task<ActionResult> LinkChangePasswordHash([FromBody] DTOUsuarioLinkChangePassword usuario)
        //{
        //    var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == usuario.Email);
        //    if (usuarioDB == null)
        //    {
        //        return Unauthorized("Usuario no registrado");
        //    }

        //    Guid miGuid = Guid.NewGuid();
        //    string textoEnlace = Convert.ToBase64String(miGuid.ToByteArray());
        //    textoEnlace = textoEnlace.Replace("=", "").Replace("+", "").Replace("/", "").Replace("?", "").Replace("&", "").Replace("!", "").Replace("¡", "");
        //    usuarioDB.EnlaceCambioPass = textoEnlace;
        //    await _context.SaveChangesAsync();
        //    var ruta = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/changepassword/{textoEnlace}";
        //    return Ok(ruta);
        //}

        //[HttpGet("/changepassword/{textoEnlace}")]
        //public async Task<ActionResult> LinkChangePasswordHash(string textoEnlace)
        //{
        //    var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.EnlaceCambioPass == textoEnlace);
        //    if (usuarioDB == null)
        //    {
        //        return BadRequest("Enlace incorrecto");
        //    }

        //    return Ok("Enlace correcto");
        //}

        //[HttpPost("usuarios/changepassword")]
        //public async Task<ActionResult> LinkChangePasswordHash([FromBody] DTOUsuarioChangePassword infoUsuario)
        //{
        //    var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == infoUsuario.Email && x.EnlaceCambioPass == infoUsuario.Enlace);
        //    if (usuarioDB == null)
        //    {
        //        return Unauthorized("Operación no autorizada");
        //    }

        //    var resultadoHash = _hashService.Hash(infoUsuario.Password);
        //    usuarioDB.Password = resultadoHash.Hash;
        //    usuarioDB.Salt = resultadoHash.Salt;usuarioDB.EnlaceCambioPass = null;

        //    await _context.SaveChangesAsync();

        //    return Ok("Password cambiado con exito");
        //}

    }
}
