using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using DiabetesNoteBook.Infrastructure.Interfaces;
using DiabetesNoteBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Security.Claims;
using System.IO;
using System.Text;

namespace DiabetesNoteBook.Application.Services
{
	//Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
	//vinculado a una interfaz.
	public class EmailService : IEmailService
	{
		////Llamamo a lo que se va a usar para que este servicio funcione
		//private readonly IConfiguration _config;
		//private readonly IHttpContextAccessor _httpContextAccessor;
		//private readonly INewStringGuid _newStringGuid;
		//private readonly DiabetesNoteBookContext _context;
		////Creamos el constructor
		//public EmailService(IConfiguration config, IHttpContextAccessor httpContextAccessor, INewStringGuid newStringGuid, DiabetesNoteBookContext context)
		//{
		//	_config = config;
		//	_httpContextAccessor = httpContextAccessor;
		//	_newStringGuid = newStringGuid;
		//	_context = context;
		//}
		private readonly IConfiguration _config;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly INewStringGuid _newStringGuid;
		private readonly DiabetesNoteBookContext _context;
		private readonly IServiceProvider _serviceProvider;
		private readonly ICompositeViewEngine _viewEngine;
		private readonly ITempDataProvider _tempDataProvider;
		

		public EmailService(IConfiguration config, IHttpContextAccessor httpContextAccessor,
			INewStringGuid newStringGuid, DiabetesNoteBookContext context, ITempDataProvider tempDataProvider,
			ICompositeViewEngine viewEngine, IServiceProvider serviceProvider)
		{
			_config = config;
			_httpContextAccessor = httpContextAccessor;
			_newStringGuid = newStringGuid;
			_context = context;
			_tempDataProvider = tempDataProvider;
			_viewEngine = viewEngine;
			_serviceProvider = serviceProvider;
		}

		
		public async Task SendEmailAsyncRegister(DTOEmail userDataRegister)
		{

			var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == userDataRegister.ToEmail);

			Guid miGuid = Guid.NewGuid();
			string textoEnlace = Convert.ToBase64String(miGuid.ToByteArray());
			textoEnlace = textoEnlace.Replace("=", "").Replace("+", "").Replace("/", "").Replace("?", "").Replace("&", "").Replace("!", "").Replace("¡", "");
			usuarioDB.EnlaceCambioPass = textoEnlace;

			var model = new DTOEmail
			{
				RecoveryLink = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Users/validarRegistro/{usuarioDB.Id}/{usuarioDB.EnlaceCambioPass}?redirect=true",
			};

			await _newStringGuid.SaveNewStringGuid(usuarioDB);

			var ruta = await RenderViewToStringAsync("ViewRegisterEmail", model);

			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
			email.To.Add(MailboxAddress.Parse(userDataRegister.ToEmail));
			email.Subject = "Confirmar Email";
			email.Body = new TextPart(TextFormat.Html)
			{
				Text = await RenderViewToStringAsync("ViewRegisterEmail", model)
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

			var model = new DTOEmail
			{
				RecoveryLink = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/shared/recover-pass/{textoEnlace}?redirect=true",
			};

			await _newStringGuid.SaveNewStringGuid(usuarioDB);

			var ruta = await RenderViewToStringAsync("RecoverPassword", model);

			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
			email.To.Add(MailboxAddress.Parse(userData.ToEmail));
			email.Subject = "Recuperar contraseña";
			email.Body = new TextPart(TextFormat.Html)
			{
				Text = await RenderViewToStringAsync("RecoverPassword", model)
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
			//Decimos de quien es el correo electronico
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
        //---------------------------------------------------------

        //public async Task SendMeasurementsByEmailAsync(string userEmail)
        //{
        //    // Busca el usuario en la base de datos
        //    var user = await _context.Usuarios.Include(u => u.Mediciones)
        //                                       .FirstOrDefaultAsync(u => u.Email == userEmail);

        //    if (user == null || user.Mediciones == null || !user.Mediciones.Any())
        //    {
        //        // Manejar el caso en el que no se encuentren mediciones o el usuario no existe
        //        return;
        //    }

        //    // Construye el cuerpo del correo con las mediciones
        //    var measurementsBody = new StringBuilder();
        //    measurementsBody.AppendLine("Aquí están tus mediciones:");

        //    foreach (var measurement in user.Mediciones)
        //    {
        //        measurementsBody.AppendLine($"Fecha: {measurement.Fecha}, Valor: {measurement.Regimen}, Pre.Medicion:{measurement.PreMedicion}, Post.Medicion: {measurement.PostMedicion}, Glucemia Capilar:{measurement.GlucemiaCapilar}, Bolus Comida: {measurement.BolusComida}, Bolus Corrector: {measurement.BolusCorrector}, Pre.Deporte:{measurement.PreDeporte}, Durante Deporte:{measurement.DuranteDeporte}, Post.Deporte:{measurement.PostDeporte}, RacionHC:{measurement.RacionHc}, Notas:{measurement.Notas}");
        //        // Agrega más propiedades de mediciones si es necesario
        //    }

        //    // Construye el mensaje de correo
        //    var email = new MimeMessage();
        //    email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
        //    email.To.Add(MailboxAddress.Parse(userEmail));
        //    email.Subject = "Tus Mediciones";
        //    email.Body = new TextPart(TextFormat.Html)
        //    {
        //        Text = measurementsBody.ToString()
        //    };

        //    // Envía el correo electrónico
        //    using var smtp = new SmtpClient();
        //    await smtp.ConnectAsync(
        //        _config.GetSection("Email:Host").Value,
        //        Convert.ToInt32(_config.GetSection("Email:Port").Value),
        //        SecureSocketOptions.StartTls
        //    );

        //    await smtp.AuthenticateAsync(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:PassWord").Value);
        //    await smtp.SendAsync(email);
        //    await smtp.DisconnectAsync(true);
        //}

        //     public async Task SendMeasurementsByEmailAsync(string userEmail)
        //     {
        //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //         int userIdInt;
        //         if (int.TryParse(userId, out userIdInt))
        //         {
        //            // Busca el usuario en la base de datos
        //         var user = await _context.Usuarios.Include(u => u.Mediciones)
        //                                            .FirstOrDefaultAsync(u => u.Id == userIdInt);



        //// Construye el cuerpo del correo con las mediciones usando la vista HTML
        //var model = user.Mediciones.Select(measurement => new DTOMediciones
        //         {
        //             Fecha = measurement.Fecha,
        //             Regimen = measurement.Regimen,
        //             PreMedicion = measurement.PreMedicion,
        //             PostMedicion = (decimal)measurement.PostMedicion,
        //             GlucemiaCapilar = measurement.GlucemiaCapilar,
        //             BolusComida = (decimal)measurement.BolusComida,
        //             BolusCorrector = (decimal)measurement.BolusCorrector,
        //             PreDeporte = (decimal)measurement.PreDeporte,
        //             DuranteDeporte = (decimal)measurement.DuranteDeporte,
        //             PostDeporte = (decimal)measurement.PostDeporte,
        //             RacionHC = (decimal)measurement.RacionHc,
        //             Notas = measurement.Notas
        //         }).ToList();

        //         var measurementsBody = await RenderViewToStringAsync("ViewMediciones", model);

        //         // Construye el mensaje de correo
        //         var email = new MimeMessage();
        //         email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
        //         email.To.Add(MailboxAddress.Parse(userEmail));
        //         email.Subject = "Tus Mediciones";
        //         email.Body = new TextPart(TextFormat.Html)
        //         {
        //             Text = measurementsBody
        //         };

        //         // Envía el correo electrónico
        //         using var smtp = new SmtpClient();
        //         await smtp.ConnectAsync(
        //             _config.GetSection("Email:Host").Value,
        //             Convert.ToInt32(_config.GetSection("Email:Port").Value),
        //             SecureSocketOptions.StartTls
        //         );

        //         await smtp.AuthenticateAsync(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:PassWord").Value);
        //         await smtp.SendAsync(email);
        //         await smtp.DisconnectAsync(true); 
        //         }

        //     }
        public async Task SendMeasurementsByEmailAsync()
        {
            //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmailClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            // Busca el usuario en la base de datos
            var user = await _context.Usuarios.Include(u => u.Mediciones)
                                                   .FirstOrDefaultAsync(u => u.Email == userEmailClaim);



                // Construye el cuerpo del correo con las mediciones usando la vista HTML
                var model = user.Mediciones.Select(measurement => new DTOMediciones
                {
                    Fecha = measurement.Fecha,
                    Regimen = measurement.Regimen,
                    PreMedicion = measurement.PreMedicion,
                    PostMedicion = (decimal)measurement.PostMedicion,
                    GlucemiaCapilar = measurement.GlucemiaCapilar,
                    BolusComida = (decimal)measurement.BolusComida,
                    BolusCorrector = (decimal)measurement.BolusCorrector,
                    PreDeporte = (decimal)measurement.PreDeporte,
                    DuranteDeporte = (decimal)measurement.DuranteDeporte,
                    PostDeporte = (decimal)measurement.PostDeporte,
                    RacionHC = (decimal)measurement.RacionHc,
                    Notas = measurement.Notas
                }).ToList();

                var measurementsBody = await RenderViewToStringAsync("ViewMediciones", model);

                // Construye el mensaje de correo
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
                email.To.Add(MailboxAddress.Parse(userEmailClaim));
                email.Subject = "Tus Mediciones";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = measurementsBody
                };

                // Envía el correo electrónico
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


        private async Task<string> RenderViewToStringAsync(string viewName, object model)
		{
			var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
			var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

			using (var sw = new StringWriter())
			{
				var viewResult = _viewEngine.FindView(actionContext, viewName, false);

				if (viewResult.View == null)
				{
					throw new ArgumentNullException($"{viewName} does not match any available view");
				}

				var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
				{
					Model = model
				};

				var viewContext = new ViewContext(
					actionContext,
					viewResult.View,
					viewDictionary,
					new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
					sw,
					new HtmlHelperOptions()
				);

				await viewResult.View.RenderAsync(viewContext);

				return sw.ToString();
			}


		}
	}
}

