using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using DiabetesNoteBook.Infrastructure.Interfaces;
using DiabetesNoteBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INewStringGuid _newStringGuid;
        private readonly DiabetesNoteBookContext _context;

        public EmailService(IConfiguration config, IHttpContextAccessor httpContextAccessor, INewStringGuid newStringGuid, DiabetesNoteBookContext context)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _newStringGuid = newStringGuid;
            _context = context;
        }

        public async Task SendEmailAsyncRegister(DTOEmail userData)
        {

            var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == userData.ToEmail);

            Guid miGuid = Guid.NewGuid();
            string textoEnlace = Convert.ToBase64String(miGuid.ToByteArray());
            textoEnlace = textoEnlace.Replace("=", "").Replace("+", "").Replace("/", "").Replace("?", "").Replace("&", "").Replace("!", "").Replace("¡", "");
            usuarioDB.EnlaceCambioPass = textoEnlace;

            await _newStringGuid.SaveNewStringGuid(usuarioDB);

            var ruta = $"Para confirmar su email haga click en este enlace: <a href='{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Users/validarRegistro/{usuarioDB.Id}/{textoEnlace}'>Confirmar Email</a>";

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(userData.ToEmail));
            email.Subject = "Confirmar Email";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = ruta
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:PassWord").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailAsyncChangePassword(DTOEmail userData)
        {

            var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == userData.ToEmail);

            DateTime fecha = DateTime.Now.AddHours(+1);
            Guid miGuid = Guid.NewGuid();
            string textoEnlace = Convert.ToBase64String(miGuid.ToByteArray());
            textoEnlace = textoEnlace.Replace("=", "").Replace("+", "").Replace("/", "").Replace("?", "").Replace("&", "").Replace("!", "").Replace("¡", "");
            usuarioDB.EnlaceCambioPass = textoEnlace;
            usuarioDB.FechaEnlaceCambioPass = fecha;

            var ruta = $"Para restablecer su contraseña haga click en este enlace: <a href='{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/ChangePasswordControllers/changePasswordMail/{textoEnlace}'>Recuperar contraseña</a>";

            await _newStringGuid.SaveNewStringGuid(usuarioDB);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(userData.ToEmail));
            email.Subject = "Recuperar contraseña";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = ruta
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:PassWord").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

        }
       
        //Metodo que controla el cambio de email
        public async Task SendEmailAsyncEmailChanged(DTOEmailNotification emailNotification)
        {
            var email = new MimeMessage();
            //Va al archivo appsetings.json y toma la informacion del email que en el se alberga
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            //Manda un correo al email proporcionado
            email.To.Add(MailboxAddress.Parse(emailNotification.ToEmail));
            //Asunto del email
            email.Subject = "Cambio de Email";

            var ruta = $"Su email ha sido cambiado a {emailNotification.NewEmail}. En caso de que usted no haya cambiado el email contacte con el administrador del sitio. Este email no admite respuesta";
            //El curpo del email
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = ruta
            };
            //Conexion con el servicio de correo electronico
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );
            //Autenticacion del correo electronico
            await smtp.AuthenticateAsync(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:PassWord").Value);
            //Realizacion del envio
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }


    }
}

