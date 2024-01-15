using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Application.Services;
using DiabetesNoteBook.Application.Filters;

namespace DiabetesNoteBook.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly HashService _hashService;
        private readonly TokenService _tokenService;
        private readonly IOperationsService _operationsService;
        private readonly INewRegister _newRegisterService;
        private readonly IEmailService _emailService;
        private readonly IConfirmEmailService _confirmEmailService;
        private readonly FiltroExcepcion _filtroDeExcepcion;
        private readonly IDeleteUserService _deleteUserService;

        public UsersController(DiabetesNoteBookContext context, TokenService tokenService, HashService hashService,
            IOperationsService operationsService, INewRegister newRegisterService,
            IEmailService emailService, IConfirmEmailService confirmEmailService, FiltroExcepcion filtroDeExcepcion, IDeleteUserService deleteUserService)
        {
            _context = context;
            _hashService = hashService;
            _tokenService = tokenService;
            _operationsService = operationsService;
            _emailService = emailService;
            _confirmEmailService = confirmEmailService;
            _newRegisterService = newRegisterService;
            _filtroDeExcepcion = filtroDeExcepcion;
            _deleteUserService = deleteUserService;
        }

        [AllowAnonymous]
        [HttpPost("registro")]
        public async Task<ActionResult> UserRegistration([FromBody] DTORegister userData)
        {

            try
            {
                var usuarioDBUser = _context.Usuarios.FirstOrDefault(x => x.UserName == userData.UserName);

                if (usuarioDBUser != null)
                {
                    return BadRequest("Usuario existente");
                }

                var usuarioDBEmail = _context.Usuarios.FirstOrDefault(x => x.Email == userData.Email);

                if (usuarioDBEmail != null)
                {
                    return BadRequest("El email ya se encuentra registrado");
                }

                await _newRegisterService.NewRegister(new DTORegister
                {
                    Avatar = userData.Avatar,
                    UserName = userData.UserName,
                    Email = userData.Email,
                    Password = userData.Password,
                    Nombre = userData.Nombre,
                    PrimerApellido = userData.PrimerApellido,
                    SegundoApellido = userData.SegundoApellido,
                    Sexo = userData.Sexo,
                    Edad = userData.Edad,
                    Peso = userData.Peso,
                    Altura = userData.Altura,
                    Actividad = userData.Actividad,
                    Medicacion = userData.Medicacion,
                    TipoDiabetes = userData.TipoDiabetes,
                    Insulina = userData.Insulina
                });

                await _emailService.SendEmailAsyncRegister(new DTOEmail
                {
                    ToEmail = userData.Email
                });

                var usuarioDBId = _context.Usuarios.FirstOrDefault(x => x.Email == userData.Email);

                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Nuevo registro",
                    UserId = usuarioDBId.Id
                });

                return Ok();
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido realizar le registro, por favor, intentelo más tarde.");
            }
        }

        [AllowAnonymous]
        [HttpGet("validarRegistro/{UserId}/{Token}")]
        public async Task<ActionResult> ConfirmRegistration([FromRoute] DTOConfirmRegistrtion confirmacion)
        {

            try
            {
                var usuarioDB = _context.Usuarios.FirstOrDefault(x => x.Id == confirmacion.UserId);

                if (usuarioDB.ConfirmacionEmail != false)
                {
                    return BadRequest("Usuario ya validado con anterioridad.");
                }

                if (usuarioDB.EnlaceCambioPass != confirmacion.Token)
                {
                    return BadRequest("Token no valido.");
                }

                await _confirmEmailService.ConfirmEmail(new DTOConfirmRegistrtion
                {
                    UserId = confirmacion.UserId
                });

                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Confirmar email",
                    UserId = usuarioDB.Id
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
        public async Task<ActionResult> UserLogin([FromBody] DTOLoginUsuario usuario)
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

                    await _operationsService.AddOperacion(new DTOOperation
                    {
                        Operacion = "Login",
                        UserId = usuarioDB.Id
                    });

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

        [HttpDelete("eliminarUsuario")]
        public async Task<ActionResult> UserDelete([FromBody] DTODeleteUser Id)
        {

            try
            {

                var userExist = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == Id.Id);

                if (userExist == null)
                {
                    return Unauthorized("Usuario no encontrado");
                }

                if (userExist.BajaUsuario == true)
                {
                    return Unauthorized("Usuario dado de baja con anterioridad");
                }

                await _deleteUserService.DeleteUserService(new DTODeleteUser
                {
                    Id = Id.Id
                });

                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Borrar usuario",
                    UserId = userExist.Id
                });

                return Ok();
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido eliminar el usuario, por favor, intentelo más tarde.");
            }

        }

    }
}
