using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Application.Services;
using System.Text;

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
        private readonly IUserDeregistrationService _userDeregistrationService;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IChangeUserDataService _changeUserDataService;

        public UsersController(DiabetesNoteBookContext context, TokenService tokenService, HashService hashService,
            IOperationsService operationsService, INewRegister newRegisterService, 
            IEmailService emailService, IConfirmEmailService confirmEmailService, IUserDeregistrationService userDeregistrationService,
            IDeleteUserService deleteUserService, IChangeUserDataService changeUserDataService)
        {
            _context = context;
            _hashService = hashService;
            _tokenService = tokenService;
            _operationsService = operationsService;
            _emailService = emailService;
            _confirmEmailService = confirmEmailService;
            _newRegisterService = newRegisterService;
            _userDeregistrationService = userDeregistrationService;
            _deleteUserService = deleteUserService;
            _changeUserDataService = changeUserDataService;
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
            string mensaje = "<a class='btn btn-primary' href='http://localhost:4200'>Ir a login</a>";
            try
            {
                var usuarioDB = _context.Usuarios.FirstOrDefault(x => x.Id == confirmacion.UserId);

                if (usuarioDB.ConfirmacionEmail != false)//Preguntar a david lo que yo he hecho choca con esto
                {
                    mensaje="<p class='display-5 mb4'> Usuario ya validado con anterioridad.</p>";
                }

                if (usuarioDB.EnlaceCambioPass != confirmacion.Token)
                {
                    mensaje = "<p class='display-5 mb4'>Token no valido</p>";
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

                StringBuilder responseHtml = new StringBuilder();
                string bootstrap = "<link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css' integrity='sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3' crossorigin='anonymous'>";
                responseHtml.AppendLine(bootstrap);
                responseHtml.AppendLine("<div class='px-4 py-5 my-5 text-center'>");
                responseHtml.AppendLine("<div class='col-lg-6 mx-auto'>");
                responseHtml.AppendLine(mensaje);
                //responseHtml.AppendLine("<p class='display-5 mb-4'>Enlace incorrecto o ya utilizado</p>");
                responseHtml.AppendLine("<div class='d-grid gap-2 d-sm-flex justify-content-sm-center'>");
                responseHtml.AppendLine("</div></div></div>");
                return Content(responseHtml.ToString(), "text/html", Encoding.UTF8);
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

                if (usuarioDB.BajaUsuario == true)
                {
                    return Unauthorized("El usuario se encuentra dado de baja.");
                }

                var resultadoHash = _hashService.Hash(usuario.Password, usuarioDB.Salt);

                if (usuarioDB.Password == resultadoHash.Hash)
                {
                    var response = await _tokenService.GenerarToken(usuarioDB);

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

        [HttpPut("bajaUsuario")]
        public async Task<ActionResult> UserDeregistration([FromBody] DTOUserDeregistration Id)
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

                await _userDeregistrationService.UserDeregistration(new DTOUserDeregistration
                {
                    Id = Id.Id
                });

                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Baja usuario",
                    UserId = userExist.Id
                });

                return Ok();
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido dar de baja el usuario, por favor, intentelo más tarde.");
            }

        }

        [HttpDelete("elimnarUsuario")]
        public async Task<ActionResult> DeleteUser([FromBody] DTODeleteUser Id)
        {

            try
            {
                var userExist = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == Id.Id);

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
        [AllowAnonymous]
        [HttpGet("usuarioPorId/{Id}")]
        public async Task<ActionResult> UserById([FromRoute] DTOById userData)
        {

            try
            {
                var userExist = await _context.Usuarios.FindAsync(userData.Id);

                if (userExist == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Consulta usuario por id",
                    UserId = userExist.Id
                });


                return Ok(userExist);
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido consultar el usuario, por favor, intentelo más tarde.");
            }

        }

        [AllowAnonymous]
        [HttpPut("cambiardatosusuarioypersona")]
        public async Task<ActionResult> UserPUT([FromBody] DTOChangeUserData userData)
        {

            try
            {
                var usuarioUpdate = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == userData.Id);

                //if (userData.UserName == usuarioUpdate.UserName)
                //{
                //    return NotFound("El usuario ya existe.");
                //}

                await _changeUserDataService.ChangeUserData(new DTOChangeUserData
                {
                    Id = userData.Id,
                    Avatar = userData.Avatar,
                    UserName = userData.UserName,
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

                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Actualizar usuario",
                    UserId = usuarioUpdate.Id
                });

                return Ok();
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido actualizar el usuario, por favor, intentelo más tarde.");
            }

        }
        [AllowAnonymous]
        [HttpPut("cambiaremail")]
        public async Task<ActionResult> EmailPUT([FromBody] string email)
        {

            try
            {
                var emailUpdate = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == email);

                emailUpdate.Email = email;
                emailUpdate.ConfirmacionEmail = false;

                _context.Usuarios.Update(emailUpdate);
                await _context.SaveChangesAsync();

                await _emailService.SendEmailAsyncRegister(new DTOEmail
                {
                    ToEmail = email
                });

                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Cambiar email",
                    UserId = emailUpdate.Id
                });

                return Ok("Email cambiado con éxito.");
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido actualizar el email, por favor, intentelo más tarde.");
            }

        }
        [AllowAnonymous]
        [HttpPut("cambiaremail1")]
        public async Task<ActionResult> EmailPUT1([FromBody] DTOChangeEmail email)
        {
            try
            {
                var emailUpdate = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == email.EmailAntiguo);

                // Verificar si el email ha cambiado con el que hay en base de datos
                if (emailUpdate != null && emailUpdate.Email != email.NuevoEmail)
                {
                    // Enviar correo al antiguo email notificando el cambio
                    await _emailService.SendEmailAsyncEmailChanged(new DTOEmailNotification
                    {
                        ToEmail = email.EmailAntiguo,
                        NewEmail = email.NuevoEmail
                    });
                }
                else
                {
                    return BadRequest("El email no puede ser el mismo  si lo va a cambiar");
                }

                // Actualizar la información del usuario con el nuevo email
                emailUpdate.ConfirmacionEmail = false;
                emailUpdate.Email = email.NuevoEmail;

                _context.Usuarios.Update(emailUpdate);
                await _context.SaveChangesAsync();

                // Enviar correo de confirmación al nuevo email
                await _emailService.SendEmailAsyncRegister(new DTOEmail
                {
                    ToEmail = email.NuevoEmail
                });

                // Registrar la operación
                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Cambiar email",
                    UserId = emailUpdate.Id
                });

                return Ok("Email cambiado con éxito.");
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido actualizar el email, por favor, inténtelo más tarde.");
            }
        }


    }
}
