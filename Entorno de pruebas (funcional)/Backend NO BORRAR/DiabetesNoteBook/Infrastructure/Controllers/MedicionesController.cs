using Aspose.Pdf;
using Aspose.Pdf.Annotations;
using Aspose.Pdf.Operators;
using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.Services;
using DiabetesNoteBook.Application.Services.Genereics;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DiabetesNoteBook.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //En este controlador se llaman a los servicios necesarios para poder operar

    public class MedicionesController : ControllerBase
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly INuevaMedicionService _medicion;
        private readonly IDeleteMedicionService _deleteMedicion;
        private readonly IMedicionesIdUsuario _medicionesIdUsuario;
        private readonly ExistUsersService _existUsersService;
        private readonly ExistMedicionesService _existMedicionesService;
        private readonly ILogger<UsersController> _logger;
        private readonly IEmailService _emailService;
        public MedicionesController(DiabetesNoteBookContext context, INuevaMedicionService nuevaMedicion, IDeleteMedicionService deleteMedicion,
             IMedicionesIdUsuario medicionesIdUsuario, ILogger<UsersController> logger, IEmailService emailService,
            ExistUsersService existUsersService, ExistMedicionesService existMedicionesService)
        {
            _context = context;
            _existUsersService = existUsersService;
            _existMedicionesService = existMedicionesService;
            _medicion = nuevaMedicion;
            _deleteMedicion = deleteMedicion;
            _logger = logger;
            _emailService = emailService;

            _medicionesIdUsuario = medicionesIdUsuario;



        }
        //En este endpoint se realiza el agregado de las mediciones para agregar necesitan los datos que
        //hay en DTOMediciones.

        [HttpPost]
        public async Task<ActionResult> PostMediciones(DTOMediciones mediciones)
        {

            try
            {
                //var existeUsuario = await _usuarioPorId.ObtenerUsuarioPorId(id);
                //var existeUsuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == mediciones.Id_Usuario);
                //Si la persona no existe devolvemos el mensaje contenido en NotFound.
                //var existeUsuario = await _existUsersService.UserExistById(mediciones.Id_Usuario);
                var existeUsuario = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int usuarioId;
                if (int.TryParse(existeUsuario, out usuarioId))
                {
                    if (existeUsuario == null)
                    {
                        return NotFound("La persona a la que intenta poner la medicion no existe");
                    }
                    //Llamamos al servicio medicion que contiene el metodo NuevaMedicion este metodo necesita un 
                    //DTOMediciones que contiene los datos necesarios para agregar la medicion a esa persona
                    await _medicion.NuevaMedicion(new DTOMediciones
                    {
                        Fecha = mediciones.Fecha,
                        Regimen = mediciones.Regimen,
                        PreMedicion = mediciones.PreMedicion,
                        GlucemiaCapilar = mediciones.GlucemiaCapilar,
                        BolusComida = mediciones.BolusComida,
                        BolusCorrector = mediciones.BolusCorrector,
                        PreDeporte = mediciones.PreDeporte,
                        DuranteDeporte = mediciones.DuranteDeporte,
                        PostDeporte = mediciones.PostDeporte,
                        RacionHC = mediciones.RacionHC,
                        Notas = mediciones.Notas,
                        


                    });





                  
                    
                }
               
                return Ok("Medicion guardada con exito ");


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error al procesar la nueva medición");
                return BadRequest("En estos momentos no se ha podido realizar la insercción de la medición, por favor, intentelo más tarde.");
            }

        }
        ////Este endpoint permite al usuario eliminar una medicion el cual se alimenta de un DTOEliminarMedicion
        ////que contiene los datos necesarios para poder eliminar esa medicion

        [HttpDelete("eliminarmedicion")]
        public async Task<ActionResult> DeleteMedicion(DTOEliminarMedicion Id)
        {
            //Buscamos la medicion por id en base de datos
            try
            {

                var medicionExist = await _existMedicionesService.MedicionesPorId(Id.Id);

                //var medicionExist =await _getMedicionExiste.GetMedicionAsync(Id.Id);
                //var medicionExist = await _context.Mediciones.FirstOrDefaultAsync(x => x.Id == Id.Id);
                //Si la medicion no existe devolvemos el mensaje contenido en BadRequest



                if (medicionExist == null)
                {
                    return BadRequest("La medicion que intenta eliminar no se encuentra");
                }


                //Llamamos al servicio _deleteMedicion que tiene un metodo DeleteMedicion el cual
                //necesita un DTOEliminarMedicion que  contiene los datos necesarios para eliminar la medicion
                await _deleteMedicion.DeleteMedicion(new DTOEliminarMedicion
                {
                    Id = Id.Id
                });
                //Agregamos la operacion  llamando  al servicio _operationsService el cual tiene un
                //metodo AddOperacion este metodo necesita un DTOOperation el cual tiene los datos necesarios 
                //para agregar la operacion



                //Devolvemos un ok si todo va bien

                return Ok("Eliminacion realizada con exito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el borrado de la medición");
                return BadRequest("En estos momentos no se ha podido realizar la ieliminación de la medición, por favor, intentelo más tarde.");

            }

        }
        ////En este endpoint obtenemos las mediciones en base a la id del usuario para este endpoint necesita un
        ////DTOById que contiene los datos necesarios para hacer el get

        [HttpGet("getmedicionesporidusuario/{Id}")]
        public async Task<ActionResult<IEnumerable<Medicione>>> GetMedicionesPorIdUsuario([FromRoute] DTOById userData)
        {

            try
            {
                var mediciones = await _existMedicionesService.MedicionesPorUserId(userData.Id);

                //Buscamos en base de datos la id del usuario el cual tiene asociadas mediciones
                //var mediciones = await _medicionesRepository.ObtenerMedicionesPorUsuarioId(userData.Id);
                //var mediciones = await _context.Mediciones.Where(m => m.IdUsuarioNavigation.Id == userData.Id).ToListAsync();
                //Si la id del usuario que se le pasa no existe no encuentra las mediciones asociadas

                if (mediciones == null)
                {
                    return NotFound("Datos de medicion no encontrados");
                }
                //Agregamos la operacion usando el servicio _operationsService que tiene un metodo
                //AddOperacion dicho metodo necesita un DTOOperation que contiene los datos necesarios
                //para realizar la operacion.


                //Si todo va bien se devuelve un ok

                return Ok(mediciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la solicitud de las  mediciones");
                return BadRequest("En estos momentos no se ha podido consultar los datos de la persona, por favor, intentelo más tarde.");
            }

        }

        [HttpGet("descargarMedicionesPDF")]

        public async Task<IActionResult> DescargarMedicionesPDF()
        {
            // Obtener el ID del usuario actualmente autenticado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Buscar las mediciones asociadas al usuario actual
            var mediciones = await _medicionesIdUsuario.ObtenerMedicionesUsuario(userId);
            //var mediciones = await _context.Mediciones
            //           .Where(m => m.IdUsuarioNavigation.Id.ToString() == userId)
            //            .ToListAsync();
            if (mediciones == null || mediciones.Count == 0)
            {
                return BadRequest("Datos de medicion no encontrados");
            }

            // Crear un documento PDF con orientación horizontal
            Document document = new Document();
            //Margenes y tamaño del documento
            document.PageInfo.Width = Aspose.Pdf.PageSize.PageLetter.Width;
            document.PageInfo.Height = Aspose.Pdf.PageSize.PageLetter.Height;
            document.PageInfo.Margin = new MarginInfo(1, 10, 10, 10); // Ajustar márgenes


            // Agregar una nueva página al documento con orientación horizontal
            Page page = document.Pages.Add();
            //Control de margenes

            page.PageInfo.Margin.Left = 35;
            page.PageInfo.Margin.Right = 0;
            //Controla la horientacion actualmente es horizontal
            page.SetPageSize(Aspose.Pdf.PageSize.PageLetter.Width, Aspose.Pdf.PageSize.PageLetter.Height);

            // Crear una tabla para mostrar las mediciones
            Aspose.Pdf.Table table = new Aspose.Pdf.Table();

            table.VerticalAlignment = VerticalAlignment.Center;
            table.Alignment = HorizontalAlignment.Left;



            table.DefaultCellBorder = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, 0.1F);
            table.Border = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, 1F);
            table.ColumnWidths = "55 50 45 45 45 35 45 45 45 45 35 50"; // Ancho de cada columna

            page.Paragraphs.Add(table);

            // Agregar fila de encabezado a la tabla
            Aspose.Pdf.Row headerRow = table.Rows.Add();

            headerRow.Cells.Add("Fecha").Alignment = HorizontalAlignment.Center;
            headerRow.Cells.Add("Regimen").Alignment = HorizontalAlignment.Center;
            headerRow.Cells.Add("Pre Medicion").Alignment = HorizontalAlignment.Center;
            headerRow.Cells.Add("Post Medicion").Alignment = HorizontalAlignment.Center;
            headerRow.Cells.Add("Glucemia Capilar").Alignment = HorizontalAlignment.Center;
            headerRow.Cells.Add("Bolus Comida").Alignment = HorizontalAlignment.Center;
            headerRow.Cells.Add("Bolus Corrector").Alignment = HorizontalAlignment.Center;
            headerRow.Cells.Add("Pre Deporte").Alignment = HorizontalAlignment.Center;
            headerRow.Cells.Add("Durante Deporte").Alignment = HorizontalAlignment.Center;
            headerRow.Cells.Add("Post Deporte").Alignment = HorizontalAlignment.Center;
            headerRow.Cells.Add("Racion HC").Alignment = HorizontalAlignment.Center;
            headerRow.Cells.Add("Notas").Alignment = HorizontalAlignment.Center;

            // Agregar contenido de mediciones a la tabla
            foreach (var medicion in mediciones)
            {

                Aspose.Pdf.Row dataRow = table.Rows.Add();
                dataRow.Cells.Add($"{medicion.Fecha}").Alignment = HorizontalAlignment.Center;
                dataRow.Cells.Add($"{medicion.Regimen}").Alignment = HorizontalAlignment.Center;
                dataRow.Cells.Add($"{medicion.PreMedicion}").Alignment = HorizontalAlignment.Center;
                dataRow.Cells.Add($"{medicion.PostMedicion}").Alignment = HorizontalAlignment.Center;
                dataRow.Cells.Add($"{medicion.GlucemiaCapilar}").Alignment = HorizontalAlignment.Center;
                dataRow.Cells.Add($"{medicion.BolusComida}").Alignment = HorizontalAlignment.Center;
                dataRow.Cells.Add($"{medicion.BolusCorrector}").Alignment = HorizontalAlignment.Center;
                dataRow.Cells.Add($"{medicion.PreDeporte}").Alignment = HorizontalAlignment.Center;
                dataRow.Cells.Add($"{medicion.DuranteDeporte}").Alignment = HorizontalAlignment.Center;
                dataRow.Cells.Add($"{medicion.PostDeporte}").Alignment = HorizontalAlignment.Center;
                dataRow.Cells.Add($"{medicion.RacionHc}").Alignment = HorizontalAlignment.Center;
                dataRow.Cells.Add($"{medicion.Notas}").Alignment = HorizontalAlignment.Center;
            }

            // Crear un flujo de memoria para guardar el documento PDF
            MemoryStream memoryStream = new MemoryStream();

            // Guardar el documento en el flujo de memoria
            document.Save(memoryStream);

            // Convertir el documento a un arreglo de bytes
            byte[] bytes = memoryStream.ToArray();

            // Liberar los recursos de la memoria
            memoryStream.Close();
            memoryStream.Dispose();

            // Devolver el archivo PDF para descargar
            return File(bytes, "application/pdf", "mediciones.pdf");
        }

        [HttpGet("send-mediciones")]
        public async Task<IActionResult> EnviarMediciones()
        {
            

            await _emailService.SendMeasurementsByEmailAsync();
            return Ok("Mediciones enviadas por correo electrónico con éxito.");
        }
    }





    }

