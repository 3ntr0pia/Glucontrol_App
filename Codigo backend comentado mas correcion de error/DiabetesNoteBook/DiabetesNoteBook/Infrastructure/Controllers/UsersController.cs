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
    //NOTA: TODAS LAS OPERACIONES QUE SEAN POST,PUT Y DELETE EL GUARDAR ACTUALIZAR O ELIMINAR ESTAS 
    //FUNCIONES SE ENCARGAN LOS SERVICIOS ASIGNADOS. POR EJEMPLO: await _newRegisterService.NewRegister(new DTORegister
    //TENEMOS ESETE SERVICIO DE REGISTRO ESTE SERVICIO ES EL QUE SE ENCARGA DE GUARDAR LOS CAMBIOS
    //POR LO TANTO NO ES NECESARIO HACER EL GUARDADO EN EL ENDPOINT YA QUE SE ENCARGA EL SERVICIO DE HACERLO.
    //Se llaman a los servicios necesarios para este controlador, los servicios no se ponen directamente
    //para promover la reutilizacion de dichos servicios, hay servicios que no al no recibir muchisimos
    //cambios se pone directamente el servicio.
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
        //Se realiza el contructor
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
        //Este endpoint su funcion es de que el usuario se pueda regristrar en la aplicacion
        //este endpoint tiene un DTO llamado DTORegister que contiene los datos necesarios para
        //que el usuario se pueda registrar dichos datos vienen del body. Para el acceso al DTO
        //le hemos llamado userData para poder acceder a estos datos
        [HttpPost("registro")]
        public async Task<ActionResult> UserRegistration([FromBody] DTORegister userData)
        {

            try
            {
                //Buscamos en la base de datos si el nombre de usuario que se intenta registrar existe en base de datos
                var usuarioDBUser = _context.Usuarios.FirstOrDefault(x => x.UserName == userData.UserName);
                //Si dicho nombre de usuario existe, al usuario le sale el mensaje contenido en el BadRequest.
                if (usuarioDBUser != null)
                {
                    return BadRequest("Usuario existente");
                }
                //Buscamos en base de datos el email del usuario por si un usuario se intenta registrar con
                //un email que ya se ha registrado en base de datos
                var usuarioDBEmail = _context.Usuarios.FirstOrDefault(x => x.Email == userData.Email);
                //Si el usuario pone un email que se encuentra en la base de datos le sale el mensaje
                //contenido en el BadRequest.
                if (usuarioDBEmail != null)
                {
                    return BadRequest("El email ya se encuentra registrado");
                }
                //llegados a este punto el nombre de usuario y email no existe y por lo tanto se procede
                //al registro llamando al servicio _newRegisterService, este servicio tiene un metodo el cual
                //se encarga de registrar al usuario dicho servicio precisa de los datos del usuario que se 
                //encuentra en DTORegister debido que asi es como lo marca el metodo NewRegister del servicio
                //de _newRegisterService
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
                //cuando el usuario se registra hay un servicio que manda un email para que confirme la
                //creacion de la cuenta si el usuario no confirma su cuenta no puede hacer login hasta que
                //no confirme su email esto se hace para evitar accesos no deseados.
                //Este servicio de enviar el email tiene un metodo SendEmailAsyncRegister que precisa de un
                //DTOEmail que contiene el email.
                await _emailService.SendEmailAsyncRegister(new DTOEmail
                {
                    ToEmail = userData.Email
                });
                //Buscamos al usuario por su email.
                var usuarioDBId = _context.Usuarios.FirstOrDefault(x => x.Email == userData.Email);
                //Agregamos la operacion que se ha realizado llamado al servicio _operationsService
                //dicho servicio tiene un metodo AddOperacion que tiene un DTOOperation que contiene
                //los datos que se va ha proporcionar sobre que operacion se ha realizado.
                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Nuevo registro",
                    UserId = usuarioDBId.Id
                });
                //Si todo ha ido bien se devuelve un ok
                return Ok();
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido realizar le registro, por favor, intentelo más tarde.");
            }
        }

        [AllowAnonymous]
        [HttpGet("validarRegistro/{UserId}/{Token}")]
        //Como hemos comentado en el controlador anterior si el usuario no confirma su email este
        //no podra loguearse este endpoint se encarga de hacer esta funcion de ver si se ha
        //confirmado o no, si el token que tiene el usuario es valido...
        //Este endpoint tiene un DTOConfirmRegistrtion que contiene los datos necesarios para dicha
        //comprobacion
        public async Task<ActionResult> ConfirmRegistration([FromRoute] DTOConfirmRegistrtion confirmacion)
        {
            //Ponemos una variable de tipo string que esta va ha ser un boton que va a reedirigir a
            //http://localhost:4200 esto llevara al usuario al front que tenemos en angular concretamente
            //al login para que se loguee el usuario.
            string mensaje = "<a class='btn btn-primary' href='http://localhost:4200'>Ir a login</a>";
            try
            {
               //Buscamos al usuario en base a su id para controlar si el usuario se ha validado o no.
                var usuarioDB = _context.Usuarios.FirstOrDefault(x => x.Id == confirmacion.UserId);
                //Si el usuario vuelve a confirmar su email le saldra el mensaje contenido en la variable
                //mensaje.
                if (usuarioDB.ConfirmacionEmail != false)
                {
                    mensaje="<p class='display-5 mb4'> Usuario ya validado con anterioridad.</p>";
                }
                //Si el token ha sido alterado o ha caducado le saldra el mensaje contenido en la variable
                //mensaje.
                if (usuarioDB.EnlaceCambioPass != confirmacion.Token)
                {
                    mensaje = "<p class='display-5 mb4'>Token no valido</p>";
                }
                //Llamamos al servicio _confirmEmailService para confirmar el email del usuario dicho servicio
                //tiene un metodo llamado ConfirmEmail que necesita los datos contenidos en DTOConfirmRegistrtion
                await _confirmEmailService.ConfirmEmail(new DTOConfirmRegistrtion
                {
                    UserId = confirmacion.UserId
                });
                //llegados a este punto el email se ha confirmado con exito por lo tanto agregamos la operacion haciendo
                //uso del servicio _operationsService cuyo servicio tiene un metodo AddOperacion que precisa de un
                //DTOOperation que tiene los datos necesarios para agregar el tipo de operacion 
                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Confirmar email",
                    UserId = usuarioDB.Id
                });
                //Con estas lineas que hay a continuacion construimos un mini front el cual tiene un boton este boton
                //esta en la variable mensaje el cual ya se ha explicado
                StringBuilder responseHtml = new StringBuilder();
                string bootstrap = "<link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css' integrity='sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3' crossorigin='anonymous'>";
                responseHtml.AppendLine(bootstrap);
                responseHtml.AppendLine("<div class='px-4 py-5 my-5 text-center'>");
                responseHtml.AppendLine("<div class='col-lg-6 mx-auto'>");
                responseHtml.AppendLine(mensaje);
                //responseHtml.AppendLine("<p class='display-5 mb-4'>Enlace incorrecto o ya utilizado</p>");
                responseHtml.AppendLine("<div class='d-grid gap-2 d-sm-flex justify-content-sm-center'>");
                responseHtml.AppendLine("</div></div></div>");
                //Para que se muestre correctamente este mini front lo tenemos que devolver de esta manera.
                return Content(responseHtml.ToString(), "text/html", Encoding.UTF8);
            }
            catch
            {

                return BadRequest("En estos momentos no se ha podido validar el registro, por favor, intentelo de nuevo más tarde.");
            }
        }

        [AllowAnonymous]
        //Este endpoint se encarga de hacer que el usuario se pueda loguear para que un usuario se 
        //pueda loguear este endpoint requiere un DTOLoginUsuario que contiene los datos necesarios
        //para hacer login esos datos se pasan por el body de la peticion
        [HttpPost("login")]
        public async Task<ActionResult> UserLogin([FromBody] DTOLoginUsuario usuario)
        {

            try
            {
                //Al hacer login se nor pide un nombre de usuario y contraseña primero comprobamos si el
                //nombre de usuario existe.
                var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.UserName == usuario.UserName);
                //Si el nombre de usuario no existe devolvemos el mensaje almacenado en Unauthorized
                if (usuarioDB == null)
                {
                    return Unauthorized("Usuario no encontrado.");
                }
                //Si el usuario existe pero no ha confirmado su email devolvemos el mensaje contenido en
                //Unauthorized
                if (usuarioDB.ConfirmacionEmail != true)
                {
                    return Unauthorized("Usuario no confirmado, por favor acceda a su correo y valida su registro.");
                }
                //Si el usuario ha solicidado darse de baja de la aplicacion he intenta loguearse se le
                //avisara al usuario con el mensaje contenido en Unauthorized.
                if (usuarioDB.BajaUsuario == true)
                {
                    return Unauthorized("El usuario se encuentra dado de baja.");
                }
                //Esta variable almacena la llamada al servicio _hashService este servicio tiene un metodo
                //llamado hash al cual se le pasa la contraseña de usuario para que la cifre y se le asigna un
                //salt que corresponde a esa contraseña
                var resultadoHash = _hashService.Hash(usuario.Password, usuarioDB.Salt);
                //Se comprueba si la contraseña que intruce el usuario corresponde con el hash que tiene asociado
                //esa contraseña en base de datos.
                if (usuarioDB.Password == resultadoHash.Hash)
                {
                    //Si la contraseña es correcta se le devuelve el token al usuario
                    var response = await _tokenService.GenerarToken(usuarioDB);
                    //Se agrega el tipo de operacion llamando al servicio _operationsService que tiene un metodo
                    //AddOperacion al cual se le pasa un DTOOperation que contiene los datos necesarios para
                    //poder agregar esa operacion
                    await _operationsService.AddOperacion(new DTOOperation
                    {
                        Operacion = "Login",
                        UserId = usuarioDB.Id
                    });
                    //Si todo ha ido bien se le devuelve el token
                    return Ok(response);
                }
                else
                {
                    //Si el usuario se equivoca en la contraseña se le devuelve este error
                    return Unauthorized("Contraseña incorrecta.");
                }

            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido realizar el login, por favor, intentelo más tarde.");
            }

        }

        [HttpPut("bajaUsuario")]
        //Este endpoint se encarga de dar de baja ha un usuario el cual necesita un DTOUserDeregistration
        //que contiene los datos necesarios para dar de baja ese usuario.
        public async Task<ActionResult> UserDeregistration([FromBody] DTOUserDeregistration Id)
        {

            try
            {
                //Buscamos si el usuario existe en base de datos esta busqueda se realiza en base a su id
                var userExist = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == Id.Id);
                //Si se intenta dar de baja a un usuario que no existe sale el mensaje contenido en
                //Unauthorized
                if (userExist == null)
                {
                    return Unauthorized("Usuario no encontrado");
                }
                //Si el usuario se intenta dar de baja nuevamente sale el mensaje contenido en Unauthorized
                if (userExist.BajaUsuario == true)
                {
                    return Unauthorized("Usuario dado de baja con anterioridad");
                }
                //Si todo va bien el usuario se da de baja correctamente, para ello se llama al servicio
                //_userDeregistrationService que tiene un metodo UserDeregistration dicho metodo necesita
                //un DTOUserDeregistration que contiene los datos necesarios para procesar la baja de usuario
                await _userDeregistrationService.UserDeregistration(new DTOUserDeregistration
                {
                    Id = Id.Id
                });
                //Se agrega la operacion usando el servicio _operationsService usando el metodo AddOperacion
                //el cual necesita un DTOOperation que contiene los datos necesarios para agregar la operacion
                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Baja usuario",
                    UserId = userExist.Id
                });
                //Si todo ha ido bien se devuelve un ok.
                return Ok();
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido dar de baja el usuario, por favor, intentelo más tarde.");
            }

        }
        //Este endpoint se encarga de eliminar un usuario este endpoint necesita un DTODeleteUser que contiene
        //los datos necesarios para eliminar el usuario.
        [HttpDelete("elimnarUsuario")]
        public async Task<ActionResult> DeleteUser([FromBody] DTODeleteUser Id)
        {

            try
            {
                //Buscamos en base de datos si existe el usuario en base de datos en base a su id.
                var userExist = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == Id.Id);
                //Si el usuario no existe al administrador del sitio le sale el mensaje contenido en
                //Unauthorized.
                if (userExist == null)
                {
                    return Unauthorized("Usuario no encontrado");
                }
                //Si el usuario no se ha dado de baja no se puede eliminar por lo tanto al administrador se le
                //comunica que el usuario no se ha dado de baja por lo tanto necesita darse de baja.
                if (userExist.BajaUsuario == false)
                {
                    return Unauthorized("El usuario no se encuentra dado de baja, por favor, solicita la baja primero.");
                }
                //Para eliminar al usuario llamamos al servicio _deleteUserService el cual tiene un metodo
                //DeleteUser este metodo necesita un DTODeleteUser que contiene los datos necesarios para
                //eliminar a un usuario.
                await _deleteUserService.DeleteUser(new DTODeleteUser
                {
                    Id = Id.Id
                });
                //Una vez que se ha eliminado el usuario se agrega una operacion llamando al servicio
                //_operationsService que contiene un metodo AddOperacion y contiene un DTOOperation el
                //cual tiene los datos necesarios para agregar la operacion.
                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Borrar usuario",
                    UserId = userExist.Id
                });

                //Si todo ha ido bien devolvemos un ok.
                return Ok();
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido eliminar el usuario, por favor, intentelo más tarde.");
            }

        }
        [AllowAnonymous]
        //En este endpoint se realiza una peticion get la cual obtiene el usuario por su id, este endpoint
        //contiene un DTOById que el dato que tiene se le pasa por ruta
        [HttpGet("usuarioPorId/{Id}")]
        public async Task<ActionResult> UserById([FromRoute] DTOById userData)
        {

            try
            {
                //Buscamos en base de datos si el usuario existe en base a su id
                var userExist = await _context.Usuarios.FindAsync(userData.Id);
                //Si el usuario no existe mostramos el mensaje contenido en NotFound.
                if (userExist == null)
                {
                    return NotFound("Usuario no encontrado");
                }
                //Si se ha realizado la consulta con exito agregamos la operacion llamando al servicio
                //_operationsService cuyo servicio tiene un metodo AddOperacion este metodo se le pasa un 
                //DTOOperation que contiene los datos necesarios para agregar la operacion.
                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Consulta usuario por id",
                    UserId = userExist.Id
                });

                //Si todo ha ido bien devolvemos el usuario.
                return Ok(userExist);
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido consultar el usuario, por favor, intentelo más tarde.");
            }

        }

        [AllowAnonymous]
        //Este endpoint se encarga de que el usuario pueda cambiar sus datos si este se ha equivocado o quiere
        //actualizar algun dato a este endpoint se le pasa un DTOChangeUserData que contiene los datos
        //que se necesita para actualizar los datos del usuario estos datos se le pasa por el body de la peticion
        [HttpPut("cambiardatosusuarioypersona")]
        public async Task<ActionResult> UserPUT([FromBody] DTOChangeUserData userData)
        {

            try
            {
                //Se busca en base de datos si el usuario existe en base a su id
                var usuarioUpdate = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == userData.Id);
                //Si el usuario no existe se  le muestra al usuario el mensaje contenido en el BadRequest
                if (usuarioUpdate == null)
                {
                    return BadRequest("El usuario ha actualizar no existe");
                }
                //Se llama al servicio _changeUserDataService encargado de actualizar el usuario
                //dicho servicio tiene un metodo ChangeUserData el cual se le pasa un DTOChangeUserData
                //este dto contiene los datos necesarios para actualizar el usuario.
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
                //Si todo ha ido bien se agrega la operacion llamando al servicio _operationsService
                //este servicio tiene un metodo AddOperacion que se le pasa un DTOOperation el cual
                //contiene los datos necesarios para agregar la operacion
                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Actualizar usuario",
                    UserId = usuarioUpdate.Id
                });
                //Si todo ha ido bien devuelve un ok.
                return Ok();
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido actualizar el usuario, por favor, intentelo más tarde.");
            }

        }
        //[AllowAnonymous]
        //[HttpPut("cambiaremail")]
        //public async Task<ActionResult> EmailPUT([FromBody] string email)
        //{

        //    try
        //    {
        //        var emailUpdate = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == email);

        //        emailUpdate.Email = email;
        //        emailUpdate.ConfirmacionEmail = false;

        //        _context.Usuarios.Update(emailUpdate);
        //        await _context.SaveChangesAsync();

        //        await _emailService.SendEmailAsyncRegister(new DTOEmail
        //        {
        //            ToEmail = email
        //        });

        //        await _operationsService.AddOperacion(new DTOOperation
        //        {
        //            Operacion = "Cambiar email",
        //            UserId = emailUpdate.Id
        //        });

        //        return Ok("Email cambiado con éxito.");
        //    }
        //    catch
        //    {
        //        return BadRequest("En estos momentos no se ha podido actualizar el email, por favor, intentelo más tarde.");
        //    }

        //}
        [AllowAnonymous]
        [HttpPut("cambiaremail")]
        //Este endpoint se encarga de que el usuario pueda cambiar su email por otro, dicho
        //endpoint necesita un DTOChangeEmail que contiene los datos necesarios para poder cambiar el email
        public async Task<ActionResult> EmailPUT([FromBody] DTOChangeEmail email)
        {
            try
            {
                //Buscamos en base de datos el email que tiene el usuario
                var emailUpdate = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == email.EmailAntiguo);

                // Verificar si el email ha cambiado con el que hay en base de datos o el email no existe
                if (emailUpdate != null && emailUpdate.Email != email.NuevoEmail)
                {
                    //Este servicio _emailService se encarga de actualizar el email cuyo servicio tiene un
                    //metodo SendEmailAsyncEmailChanged que se encarga de actualizar el email el cual tiene
                    //un DTOEmailNotification que tiene los datos necesarios para cambiar el email.
                    //Este servicio hace varias cosas cuando actualizas el email envia una notificacion al
                    //correo antiguo y al nuevo envia el correo de notificacion.
                    await _emailService.SendEmailAsyncEmailChanged(new DTOEmailNotification
                    {
                        ToEmail = email.EmailAntiguo,
                        NewEmail = email.NuevoEmail
                    });
                }
                else
                {
                    //Si hay algun error muestra este mensaje
                    return BadRequest("El email no puede ser el mismo  si lo va a cambiar");
                }

                //Cambiamos el antiguo email a que ese email no esta confirmado
                emailUpdate.ConfirmacionEmail = false;
                //Asignamos al Email antigual el nuevo email
                emailUpdate.Email = email.NuevoEmail;
                //Actualizamos el email
                _context.Usuarios.Update(emailUpdate);
                //Guardamos los cambios
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
                //Si todo ha ido bien devolvemos un ok
                return Ok("Email cambiado con éxito.");
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido actualizar el email, por favor, inténtelo más tarde.");
            }
        }


    }
}
