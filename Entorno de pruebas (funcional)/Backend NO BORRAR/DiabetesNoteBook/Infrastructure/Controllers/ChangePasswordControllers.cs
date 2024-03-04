using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Domain.Models;
using Azure;
using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.Services;
using DiabetesNoteBook.Infrastructure.Interfaces;
using DiabetesNoteBook.Application.Services.Genereics;
using Aspose.Pdf.Operators;

namespace DiabetesNoteBook.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //En este controlador se importan los servicios necesarios para realizar las operaciones.

    public class ChangePasswordControllers : ControllerBase
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly HashService _hashService;
       
        private readonly IEmailService _emailService;
        private readonly IChangePassService _changePassService;
        private readonly IChangePassMail _changePassMail;
     
      
       
        private readonly ExistUsersService _existUsersService;
        private readonly ILogger<UsersController> _logger;

        //Se pone el constructor

        public ChangePasswordControllers(DiabetesNoteBookContext context, HashService hashService,
             IEmailService emailService, IChangePassService changePassService,
            IChangePassMail changePassMail, 
			 ExistUsersService existUsersService, ILogger<UsersController> logger)
        {
            _context = context;
            _hashService = hashService;
            
            _emailService = emailService;
            _changePassService = changePassService;
            _changePassMail = changePassMail;
           
          
           
            _existUsersService = existUsersService;
            _logger = logger;
        }
        //En este endpoint se usa para cambiar la contraseña del usuario el cual le llegan unos
        //datos que se albergan en un DTOCambioPassPorId
        [AllowAnonymous]
        [HttpPut("changePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] DTOCambioPassPorId userData)
        {
            try
            {
                var usuarioDB = await _existUsersService.UserExistById(userData.Id);

                //var usuarioDB = await _getUsuarioIdWithAsTracking.ObtenerUsuarioPorId(userData.Id);
                //Se busca en base de datos el id del usuario para el que se va ha cambiar la contraseña
                //var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == userData.Id);
                //Si el id del  usuario no existe la operacion no existe

                if (usuarioDB == null)
                {
                    return Unauthorized("Operación no autorizada");
                }
                //Se usa  el servicio _hashService para hashear la contraseña con el metodo Hash,
                //este metodo Hash se le pasa la contraseña que pone el usuario y se cifra
                //y a esta contraseña se le asigna un salt el salt es un numero aleatorio
                var resultadoHash = _hashService.Hash(userData.NewPass, usuarioDB.Salt);
                //Si la contraseña que se intenta cambiar pones la misma que hay en base de datos
                //se le muestra al usuario el mensaje que hay en Unauthorized
                if (usuarioDB.Password == resultadoHash.Hash)
                {
                    return Unauthorized("La nueva contraseña no puede ser la misma.");
                }
                //Se llama al servicio _changePassService para cambiar la contraseña este servicio
                //tiene un metodo ChangePassId al cual se le pasa un DTOCambioPassPorId el cual tiene
                //los datos necesarios para cambiar la contraseña
                await _changePassService.ChangePassId(new DTOCambioPassPorId
                {
                    Id = userData.Id,
                    NewPass = userData.NewPass
                });
                //Agregamos la operacion

               
                //Si todo va bien se devuelve un ok

                return Ok("Password cambiado con exito");
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Error al procesar al cambiar la contraseña");
                return BadRequest("En estos momentos no se ha podido realizar el cambio de contraseña, por favor, intentelo más tarde.");
            }
        }
        //Este endpoint cambia la contraseña via email el cual recibe un DTOUsuarioChangePasswordMail
        //para manejar los datos 
        [AllowAnonymous]
        [HttpPost("changePasswordMail")]
        public async Task<ActionResult<Response>> SendInstruction(DTOUsuarioChangePasswordMail usuario)
        {
            try
            {
                var usuarioDB = await _existUsersService.EmailExist(usuario.Email);

                if (usuarioDB is false)
                {
                    return Unauthorized("Este email no se encuentra registrado.");
                }

                var usuarioDBEmail = await _existUsersService.UserExistByEmail(usuario.Email);

                if (usuarioDBEmail is false)
                {
                    return Unauthorized("Usuario dado de baja con anterioridad");
                }

                if (usuarioDB != null)
                {

                    await _emailService.SendEmailAsyncChangePassword(new DTOEmail
                    {
                        ToEmail = usuario.Email
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
        //Este endpoint recibe los datos que genera el endpoint anterior, a este endpoint se le pasa un DTOUsuarioChangePasswordMailConEnlace
        //el cual tiene los datos necesarios para gestionar la respuesta
        [AllowAnonymous]
        [HttpPost("changePasswordMailConEnlace")]
        public async Task<ActionResult<Response>> Reset( [FromBody] DTOUsuarioChangePasswordMailConEnlace cambiopass)
        {
            try
            {
                var userTokenExiste = await _existUsersService.UserTokenExist(cambiopass);

                //Comprobamos en base de datos si el token generado con el anterior endpoint se ha generado
                //var userTokenExiste = await _recuperarPassConEnlaceMail.ObtenerUsuarioEmailEnlace(cambiopass.Token);
                //var userTokenExiste = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.EnlaceCambioPass == cambiopass.Token);
                //Llamamos al servicio _hashService para hashear la contraseña que el usuario pone y ha
                //esta contraseña se le asigna un salt este sal va de la mano con el token que se ha generado
                //en el endpoint anterior
                var resultadoHash = _hashService.Hash(cambiopass.NewPass, userTokenExiste.Salt);
                //Vemos si la nueva contraseña que ha introducido el usuario corresponde con la que hay en
                //base de datos si corresponde con la que hay en base de datos se le muestra el mensaje que
                //hay en Unauthorized
                if (userTokenExiste.Password == resultadoHash.Hash)
                {
                    return Unauthorized("La nueva contraseña no puede ser la misma.");
                }
                //Se genera una fecha

                DateTime fecha = DateTime.Now;
                //Si el token generado en el endpoint anterior existe y el token que se ha generado es mayoor o
                //igual a la fecha actual se genera
                if (userTokenExiste.EnlaceCambioPass != null && userTokenExiste.FechaEnlaceCambioPass >= fecha)
                {
                    //Se llama al servicio _changePassMail este servicio tiene un metodo ChangePassEnlaceMail
                    //el cual tiene un DTOUsuarioChangePasswordMailConEnlace que contiene los datos necesarios
                    //para cambiar el email
                    await _changePassMail.ChangePassEnlaceMail(new DTOUsuarioChangePasswordMailConEnlace
                    {
                        NewPass = cambiopass.NewPass,
                        Token=cambiopass.Token
                       
                    });
                    //Si todo ha ido bien devuelve un ok

                    return Ok("Password cambiado con exito");
                }
                //Se agrega la operacion

               
                //Si el token a caducado o no existe muestra este mensaje

                return Ok("El token no existe o ha caducado.");

            }
            catch(Exception ex)
            {
                //Este mensaje se muestra si ocurreo algun otro error con el servidor
                _logger.LogError(ex + "Error al procesar el reset del email.");
                return BadRequest("En este momento no se puede actualizar tu contraseña, intentelo más tarde por favor.");
            }
        }

    }
}
