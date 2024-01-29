using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCadaGotaCuenta.DTOs;
using WebApiCadaGotaCuenta.Models;

namespace WebApiCadaGotaCuenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly GlucoControlContext _controlContext;

        public PersonasController(GlucoControlContext controlContext)
        {
            _controlContext = controlContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> GetPersonas()
        {
            var personas = await _controlContext.Personas.ToListAsync();
            return Ok(personas);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Persona>>> PostMediciones(DTOPersonas personas)
        {
            var existeUsuario = await _controlContext.Usuarios.FindAsync(personas.UserId);
            if(existeUsuario == null)
            {
                return BadRequest("El usuario no existe");
            };
            var nuevaPersona = new Persona()
            {
                Nombre= personas.Nombre,
                PrimerApellido=personas.PrimerApellido,
                SegundoApellido=personas.SegundoApellido,
                Sexo=personas.Sexo,
                Edad=personas.Edad,
                Peso=personas.Peso,
                Altura=personas.Altura,
                TipoDiabetes=personas.TipoDiabetes,
                UserId=personas.UserId,

            };
            await _controlContext.Personas.AddAsync(nuevaPersona);
            await _controlContext.SaveChangesAsync();
            return Ok("Persona agregada con exito");

        }
        [HttpPut("{Id}")]
        public async Task<ActionResult> PutPersona([FromRoute] int Id, [FromForm] DTOPersonas personas)
        {
            var existePersona = await _controlContext.Personas.FindAsync(Id);
            if (existePersona == null)
            {
                return BadRequest("La persona no existe");
            }
            var existeUsuario = await _controlContext.Personas.FindAsync(personas.UserId);
            if (existeUsuario == null)
            {
                return BadRequest("El usuario no existe");
            }
            var personaUpdate = await _controlContext.Personas.AsTracking().FirstOrDefaultAsync(persona => persona.Id == Id);
            personaUpdate.Nombre=personas.Nombre;
            personaUpdate.PrimerApellido=personas.PrimerApellido;
            personaUpdate.SegundoApellido=personas.SegundoApellido;
            personaUpdate.Sexo=personas.Sexo;
            personaUpdate.Edad=personas.Edad;
            personaUpdate.Peso=personas.Peso;
            personaUpdate.Altura=personas.Altura;
            personaUpdate.TipoDiabetes=personas.TipoDiabetes;

            _controlContext.Update(personaUpdate);
            await _controlContext.SaveChangesAsync();
            return Ok("Persona actualizada con exito");
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteMedicion(int Id)
        {
            var existePersona = await _controlContext.Personas.FirstOrDefaultAsync(x => x.Id == Id);
            if (existePersona == null)
            {
                return BadRequest("La persona no existe");
            }
            _controlContext.Remove(existePersona);
            await _controlContext.SaveChangesAsync();
            return Ok("Persona Eliminada Con Exito");
        }

    }
}
