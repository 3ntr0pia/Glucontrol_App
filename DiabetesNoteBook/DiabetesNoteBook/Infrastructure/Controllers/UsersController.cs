using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Application.Services;
using DiabetesNoteBook.Application.Services.Genereics;

namespace DiabetesNoteBook.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ExistUsersService _existUsersService;
        private readonly HashService _hashService;
        private readonly TokenService _tokenService;
        private readonly INewRegister _newRegisterService;
        private readonly IEmailService _emailService;
        private readonly IConfirmEmailService _confirmEmailService;
        private readonly IUserDeregistrationService _userDeregistrationService;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IChangeUserDataService _changeUserDataService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(TokenService tokenService, HashService hashService,INewRegister newRegisterService, 
            IEmailService emailService, IConfirmEmailService confirmEmailService, IUserDeregistrationService userDeregistrationService,
            IDeleteUserService deleteUserService, IChangeUserDataService changeUserDataService, ILogger<UsersController> logger, ExistUsersService existUsersService)
        {
            _existUsersService = existUsersService;
            _hashService = hashService;
            _tokenService = tokenService;
            _emailService = emailService;
            _confirmEmailService = confirmEmailService;
            _newRegisterService = newRegisterService;
            _userDeregistrationService = userDeregistrationService;
            _deleteUserService = deleteUserService;
            _changeUserDataService = changeUserDataService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("registro")]
        public async Task<ActionResult> UserRegistration([FromBody] DTORegister userData)
        {
            
            try
            {

                var usuarioDBUser = await _existUsersService.UserNameExist(userData.UserName);

                if (usuarioDBUser is true)
                {
                    return BadRequest("El usuario ya se encuentra registrado");
                }

                var usuarioDBEmail = await _existUsersService.EmailExist(userData.Email);

                if (usuarioDBEmail is true)
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

                return Ok();
            }
            catch (Exception ex)
			{
				_logger.LogError(ex, "Error al procesar el registro");
				return BadRequest("En estos momentos no se ha podido realizar le registro, por favor, intentelo más tarde.");
            }
        }

        [AllowAnonymous]
        [HttpGet("validarRegistro/{UserId}/{Token}")]
        public async Task<ActionResult> ConfirmRegistration([FromRoute] DTOConfirmRegistrtion confirmacion)
        {
            
            try
            {

                var usuarioDBUser = await _existUsersService.UserExistById(confirmacion.UserId);

                if (usuarioDBUser.ConfirmacionEmail != false)
                {
                    return BadRequest ("Usuario ya validado con anterioridad");
                }

                if (usuarioDBUser.EnlaceCambioPass != confirmacion.Token)
                {
                    return BadRequest ("Token no valido");
                }

                await _confirmEmailService.ConfirmEmail(new DTOConfirmRegistrtion
                {
                    UserId = confirmacion.UserId
                });

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar de confirmación");
                return BadRequest("En estos momentos no se ha podido validar el registro, por favor, intentelo de nuevo más tarde.");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> UserLogin([FromBody] DTOLoginUsuario usuario)
        {

            try
            {

                var userExist = await _existUsersService.UserNameExist(usuario.UserName);

                if (userExist == null)
                {
                    return Unauthorized("Usuario no encontrado.");
                }

                var userConfirm = await _existUsersService.UserExistByUserName(usuario.UserName);

                if (userConfirm.ConfirmacionEmail != true)
                {
                    return Unauthorized("Usuario no confirmado, por favor acceda a su correo y valida su registro.");
                }

                if (userConfirm.BajaUsuario == true)
                {
                    return Unauthorized("El usuario se encuentra dado de baja.");
                }

                var resultadoHash = _hashService.Hash(usuario.Password, userConfirm.Salt);

                if (userConfirm.Password == resultadoHash.Hash)
                {
                    var response = await _tokenService.GenerarToken(userConfirm);

                    return Ok(response);
                }
                else
                {
                    return Unauthorized("Contraseña incorrecta y/o contraseña incorrectos.");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar de logado");
                return BadRequest("En estos momentos no se ha podido realizar el login, por favor, intentelo más tarde.");
            }

        }

        [HttpPut("bajaUsuario")]
        public async Task<ActionResult> UserDeregistration([FromBody] DTOUserDeregistration Id)
        {

            try
            {

                var usuarioDBUser = await _existUsersService.UserExistById(Id.Id);

                if (usuarioDBUser == null)
                {
                    return Unauthorized("Usuario no encontrado");
                }

                if (usuarioDBUser.BajaUsuario == true)
                {
                    return Unauthorized("Usuario dado de baja con anterioridad");
                }

                await _userDeregistrationService.UserDeregistration(new DTOUserDeregistration
                {
                    Id = Id.Id
                });

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar de baja");
                return BadRequest("En estos momentos no se ha podido dar de baja el usuario, por favor, intentelo más tarde.");
            }

        }

        [HttpDelete("elimnarUsuario")]
        public async Task<ActionResult> DeleteUser([FromBody] DTODeleteUser Id)
        {

            try
            {
                var userExist = await _existUsersService.UserExistById(Id.Id);

                if (userExist == null)
                {
                    return Unauthorized("Usuario no encontrado");
                }

                if (userExist.BajaUsuario == false)
                {
                    return Unauthorized("El usuario no se encuentra dado de baja, por favor, solicita la baja primero.");
                }

                await _deleteUserService.DeleteUser(new DTODeleteUser
                {
                    Id = Id.Id
                });

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar de eliminación de usuario");
                return BadRequest("En estos momentos no se ha podido eliminar el usuario, por favor, intentelo más tarde.");
            }
        }

        [AllowAnonymous]
        [HttpPut("cambiardatosusuario")]
        public async Task<ActionResult> UserPUT([FromBody] DTOChangeUserData userData)
        {

            try
            {
                var userExist = await _existUsersService.UserExistById(userData.Id);

                await _changeUserDataService.ChangeUserData(new DTOChangeUserData
                {
                    Id = userData.Id,
                    Avatar = userData.Avatar,
                    Nombre = userData.Nombre,
                    PrimerApellido = userData.PrimerApellido,
                    SegundoApellido = userData.SegundoApellido,
                    Sexo = userData.Sexo,
                    Edad = userData.Edad,
                    Peso = userData.Peso,
                    Altura = userData.Altura,
                    Actividad = userData.Actividad,
                    TipoDiabetes = userData.TipoDiabetes,
                    Insulina = userData.Insulina,
                    Email = userData.Email
                });

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar de eliminación actualización de usuario");
                return BadRequest("En estos momentos no se ha podido actualizar el usuario, por favor, intentelo más tarde.");
            }

        }

    }
}
