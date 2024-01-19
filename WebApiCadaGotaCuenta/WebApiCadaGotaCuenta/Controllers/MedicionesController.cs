using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCadaGotaCuenta.DTOs;
using WebApiCadaGotaCuenta.Models;

namespace WebApiCadaGotaCuenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicionesController : ControllerBase
    {
        private readonly GlucoControlContext _controlContext;

        public MedicionesController(GlucoControlContext controlContext)
        {
            _controlContext = controlContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicione>>> GetMediciones()
        {
            var mediciones= await _controlContext.Mediciones.ToListAsync();
            return Ok(mediciones);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Medicione>>> PostMediciones(DTOMediciones mediciones)
        {
            var existePersona = _controlContext.Mediciones.FindAsync(mediciones.IdPersona);
            if (existePersona == null)
            {
                return BadRequest("La persona no existe");
            }
            var nuevaMedicion = new Medicione()
            {
                Fecha = mediciones.Fecha,
                Regimen = mediciones.Regimen,
                PreMedicion = mediciones.PreMedicion,
                PostMedicion = mediciones.PostMedicion,
                GlucemiaCapilar = mediciones.GleucemiaCapilar,
                BolusComida = mediciones.BolusComida,
                BolusCorrector = mediciones.BolusCorrector,
                CuerposCetonicos = mediciones.CuerposCetonicos,
                Notas = mediciones.Notas,
                IdPersona = mediciones.IdPersona
            };
           await _controlContext.Mediciones.AddAsync(nuevaMedicion);
           await _controlContext.SaveChangesAsync();
            return Ok("Medicion agregada con exito");

        }
        [HttpPut("{Id}")]
        public async Task<ActionResult> PutMedicion([FromRoute]int Id, [FromForm] DTOMediciones mediciones)
        {
            var existeMedicion = await _controlContext.Mediciones.FindAsync(Id);
            if(existeMedicion == null)
            {
                return BadRequest("La medicion no existe");
            }
            var existePersona = await _controlContext.Mediciones.FindAsync(mediciones.IdPersona);
            if(existePersona == null)
            {
                return BadRequest("La persona no existe");
            }
            var medicionUpdate = await _controlContext.Mediciones.AsTracking().FirstOrDefaultAsync(medicion => medicion.Id == Id);
            medicionUpdate.Fecha= mediciones.Fecha;
            medicionUpdate.Regimen= mediciones.Regimen;
            medicionUpdate.PreMedicion= mediciones.PreMedicion;
            medicionUpdate.PostMedicion=mediciones.PostMedicion;
            medicionUpdate.GlucemiaCapilar = mediciones.GleucemiaCapilar;
            medicionUpdate.BolusComida = mediciones.BolusComida;
            medicionUpdate.BolusCorrector = mediciones.BolusCorrector;
            medicionUpdate.CuerposCetonicos = mediciones.CuerposCetonicos;
            medicionUpdate.Notas=mediciones.Notas;
            medicionUpdate.IdPersona= mediciones.IdPersona;
            _controlContext.Update(medicionUpdate);
            await _controlContext.SaveChangesAsync();
            return Ok("Medicion Actualizada con exito");
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteMedicion(int Id)
        {
            var existeMedicion= await _controlContext.Mediciones.FirstOrDefaultAsync(x => x.Id==Id);
            if (existeMedicion == null)
            {
                return BadRequest("La medicion no existe");
            }
           _controlContext.Remove(existeMedicion);
            await _controlContext.SaveChangesAsync();
            return Ok("Medicion Eliminada Con Exito");
        }

    }
}
