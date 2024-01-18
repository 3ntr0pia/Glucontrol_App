using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
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

        public MedicionesController(DiabetesNoteBookContext context, IOperationsService operationsService)
        {
            _context = context;
            _operationsService = operationsService;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> PostMediciones(DTOMediciones mediciones)
        {
            var existePersona = await _context.Personas.FirstOrDefaultAsync(x => x.Id == mediciones.Id_Persona);
            if (existePersona == null)
            {
                return NotFound("La persona a la que intenta poner la medicion no existe");
            }
            var nuevaMedicion = new Medicione()
            {
                Fecha = mediciones.Fecha,
                Regimen = mediciones.Regimen,
                PreMedicion = mediciones.PreMedicion,
                PostMedicion = mediciones.PostMedicion,
                GlucemiaCapilar = mediciones.GlucemiaCapilar,
                BolusComida = mediciones.BolusComida,
                BolusCorrector = mediciones.BolusCorrector,
                PreDeporte = mediciones.PreDeporte,
                DuranteDeporte = mediciones.DuranteDeporte,
                PostDeporte = mediciones.PostDeporte,
                Notas = mediciones.Notas,
                IdPersona = mediciones.Id_Persona,
            };
            _context.Mediciones.AddAsync(nuevaMedicion);
            await _context.SaveChangesAsync();
            return Ok("Medicion guardada con exito");
        }

    }
}
