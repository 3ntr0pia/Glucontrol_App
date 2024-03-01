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

namespace DiabetesNoteBook.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MedicionesController : ControllerBase
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly ExistUsersService _existUsersService;
        private readonly ExistMedicionesService _existMedicionesService;
        private readonly IOperationsService _operationsService;
        private readonly INuevaMedicionService _medicion;
        private readonly IDeleteMedicionService _deleteMedicion;
        private readonly ILogger<UsersController> _logger;


        public MedicionesController(DiabetesNoteBookContext context, IOperationsService operationsService, 
            INuevaMedicionService nuevaMedicion, IDeleteMedicionService deleteMedicion, ILogger<UsersController> logger,
            ExistUsersService existUsersService, ExistMedicionesService existMedicionesService)
        {
            _context = context;
            _existUsersService = existUsersService;
            _operationsService = operationsService;
            _medicion = nuevaMedicion;
            _deleteMedicion = deleteMedicion;
            _logger = logger;
            _existMedicionesService = existMedicionesService;

        }

        [HttpPost]
        public async Task<ActionResult> PostMedicion(DTOMediciones mediciones)
        {
            try
            {

                var userExist = await _existUsersService.UserExistById(mediciones.Id_Usuario);

                if (userExist == null)
                {
                    return NotFound("El usuario para introducir la medición no existe.");
                }

                await _medicion.NuevaMedicion(new DTOMediciones
                {
                    Fecha = mediciones.Fecha,
                    Regimen = mediciones.Regimen,
                    PreMedicion = mediciones.PreMedicion,
                    PostMedicion = mediciones.GlucemiaCapilar,
                    GlucemiaCapilar = mediciones.GlucemiaCapilar,
                    BolusComida = mediciones.BolusComida,
                    BolusCorrector = mediciones.BolusCorrector,
                    PreDeporte = mediciones.PreDeporte,
                    DuranteDeporte = mediciones.DuranteDeporte,
                    PostDeporte = mediciones.PostDeporte,
                    RacionHC = mediciones.RacionHC,
                    Notas = mediciones.Notas,
                    Id_Usuario = mediciones.Id_Usuario
                });

                return Ok("Medicion guardada con exito ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la nueva medición");
                return BadRequest("En estos momentos no se ha podido realizar la insercción de la medición, por favor, intentelo más tarde.");
            }
        }

        [HttpDelete("eliminarmedicion")]
        public async Task<ActionResult> DeleteMedicion(DTOEliminarMedicion Id)
        {
            try
            {

                var medicionExist = await _existMedicionesService.MedicionesPorId(Id.Id);

                if (medicionExist == null)
                {
                    return BadRequest("La medicion que intenta eliminar no se encuentra");
                }

                await _deleteMedicion.DeleteMedicion(new DTOEliminarMedicion
                {
                    Id = Id.Id
                });

                return Ok("Eliminacion realizada con exito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el borrado de la medición");
                return BadRequest("En estos momentos no se ha podido realizar la ieliminación de la medición, por favor, intentelo más tarde.");
            }
        }

        [HttpGet("getmedicionesporidusuario/{Id}")]
        public async Task<ActionResult<IEnumerable<Medicione>>> GetMedicionesPorIdUsuario([FromRoute] DTOById userData)
        {

            try
            {
                var medicionExist = await _existUsersService.UserExistById(userData.Id);

                if (medicionExist == null)
                {
                    return NotFound("Datos de medicion no encontrados");
                }

                return Ok(medicionExist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la solicitud de las  mediciones");
                return BadRequest("En estos momentos no se ha podido consultar los datos de la persona, por favor, intentelo más tarde.");
            }

        }
    }
}
