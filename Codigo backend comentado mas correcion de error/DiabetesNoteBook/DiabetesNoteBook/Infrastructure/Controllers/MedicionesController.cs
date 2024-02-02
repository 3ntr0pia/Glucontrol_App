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
    //En este controlador se llaman a los servicios necesarios para poder operar
    public class MedicionesController : ControllerBase
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly IOperationsService _operationsService;
        private readonly INuevaMedicionService _medicion;
        private readonly IDeleteMedicionService _deleteMedicion;

        //Se realiza el constructor
        public MedicionesController(DiabetesNoteBookContext context, IOperationsService operationsService, INuevaMedicionService nuevaMedicion, IDeleteMedicionService deleteMedicion)
        {
            _context = context;
            _operationsService = operationsService;
            _medicion = nuevaMedicion;
            _deleteMedicion = deleteMedicion;

        }
        [HttpPost]
        [AllowAnonymous]
        //En este endpoint se realiza el agregado de las mediciones para agregar necesitan los datos que
        //hay en DTOMediciones.
        public async Task<ActionResult> PostMediciones(DTOMediciones mediciones)
        {
            //Buscamos si la persona existe en base de datos ya que las mediciones estan asociadas a una persona
            var existePersona = await _context.Personas.FirstOrDefaultAsync(x => x.Id == mediciones.Id_Persona);
            //Si la persona no existe devolvemos el mensaje contenido en NotFound.
            if (existePersona == null)
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
                RacionHC= mediciones.RacionHC,
                Notas = mediciones.Notas,
                Id_Persona = mediciones.Id_Persona


            });
            //Se agrega la operacion llamando al servicio  _operationsService el cual tiene un metodo
            //_operationsService dicho metodo se alimenta de un DTOOperation que contiene los datos necesarios para 
            //agregar la operacion
            await _operationsService.AddOperacion(new DTOOperation
            {
                Operacion = "Persona agregada",
                UserId = existePersona.Id
            });
            //Si todo va bien devuel un ok.
            return Ok("Medicion guardada con exito ");
        }
        //Este endpoint permite al usuario eliminar una medicion el cual se alimenta de un DTOEliminarMedicion
        //que contiene los datos necesarios para poder eliminar esa medicion
        [HttpDelete("eliminarmedicion")]
        public async Task<ActionResult> DeleteMedicion(DTOEliminarMedicion Id)
        {
            //Buscamos la medicion por id en base de datos
            var medicionExist = await _context.Mediciones.FirstOrDefaultAsync(x => x.Id == Id.Id);
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
            await _operationsService.AddOperacion(new DTOOperation
            {
                Operacion = "Eliminar medicion",
                UserId = medicionExist.Id
            });
            //Devolvemos un ok si todo va bien
            return Ok("Eliminacion realizada con exito");
        }
        [AllowAnonymous]
        //En este endpoint obtenemos las mediciones en base a la id del usuario para este endpoint necesita un
        //DTOById que contiene los datos necesarios para hacer el get
        [HttpGet("getmedicionesporidusuario/{Id}")]
        public async Task<ActionResult<IEnumerable<Medicione>>> GetMedicionesPorIdUsuario([FromRoute] DTOById userData)
        {

            try
            {
                //Buscamos en base de datos la id del usuario el cual tiene asociadas mediciones
                var mediciones = await _context.Mediciones.Where(m => m.IdPersonaNavigation.UserId == userData.Id).ToListAsync();
                //Si la id del usuario que se le pasa no existe no encuentra las mediciones asociadas
                if (mediciones == null)
                {
                    return NotFound("Datos de medicion no encontrados");
                }
                //Agregamos la operacion usando el servicio _operationsService que tiene un metodo
                //AddOperacion dicho metodo necesita un DTOOperation que contiene los datos necesarios
                //para realizar la operacion.
                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Consulta medicion por id de usuario",
                    UserId = userData.Id
                });

                //Si todo va bien se devuelve un ok
                return Ok(mediciones);
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido consultar los datos de la persona, por favor, intentelo más tarde.");
            }

        }
    }
}
