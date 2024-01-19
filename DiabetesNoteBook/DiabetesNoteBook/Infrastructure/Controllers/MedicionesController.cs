using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.Services;
using DiabetesNoteBook.Domain.Models;
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

        public MedicionesController(DiabetesNoteBookContext context, IOperationsService operationsService,INuevaMedicionService nuevaMedicion)
        {
            _context = context;
            _operationsService = operationsService;
            _medicion = nuevaMedicion;
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
                Id_Persona=mediciones.Id_Persona
               
               
            });


          
            return Ok("Medicion guardada con exito");
        }


    }
}
