using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.Services;
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
    public class MedicionesController : ControllerBase
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly IOperationsService _operationsService;
        private readonly INuevaMedicionService _medicion;
        private readonly IDeleteMedicionService _deleteMedicion;


        public MedicionesController(DiabetesNoteBookContext context, IOperationsService operationsService, INuevaMedicionService nuevaMedicion, IDeleteMedicionService deleteMedicion)
        {
            _context = context;
            _operationsService = operationsService;
            _medicion = nuevaMedicion;
            _deleteMedicion = deleteMedicion;

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> PostMediciones1(DTOMediciones mediciones)
        {
            var existePersona = await _context.Personas.FirstOrDefaultAsync(x => x.Id == mediciones.Id_Persona);
            if (existePersona == null)
            {
                return NotFound("La persona a la que intenta poner la medicion no existe");
            }

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
                Notas = mediciones.Notas,
                Id_Persona = mediciones.Id_Persona


            });
            await _operationsService.AddOperacion(new DTOOperation
            {
                Operacion = "Persona agregada",
                UserId = existePersona.Id
            });
            return Ok("Medicion guardada con exito");
        }
        [HttpDelete("eliminarmedicion")]
        public async Task<ActionResult> DeleteMedicion(DTOEliminarMedicion Id)
        {
            var medicionExist = await _context.Mediciones.FirstOrDefaultAsync(x => x.Id == Id.Id);
            if (medicionExist == null)
            {
                return BadRequest("La medicion que intenta eliminar no se encuentra");
            }
            await _deleteMedicion.DeleteMedicion(new DTOEliminarMedicion
            {
                Id = Id.Id
            });
            await _operationsService.AddOperacion(new DTOOperation
            {
                Operacion = "Eliminar medicion",
                UserId = medicionExist.Id
            });
            return Ok("Eliminacion realizada con exito");
        }
        [AllowAnonymous]
        [HttpGet("getmedicionesporidusuario/{Id}")]
        public async Task<ActionResult> GetMedicionesPorIdUsuario1([FromRoute] DTOById userData)
        {

            try
            {
                var mediciones = await _context.Mediciones.FirstOrDefaultAsync(m => m.IdPersonaNavigation.UserId == userData.Id);

                if (mediciones == null)
                {
                    return NotFound("Datos de medicion no encontrados");
                }

                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Consulta medicion por id de usuario",
                    UserId = mediciones.Id
                });


                return Ok(mediciones);
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido consultar los datos de la persona, por favor, intentelo más tarde.");
            }

        }
    }
}
