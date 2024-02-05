using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //Se llama al servicio necesario para realizar las operaciones oportunas.
    public class PersonController : ControllerBase
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly IOperationsService _operationsService;
        //Se realiza el constructor
        public PersonController(DiabetesNoteBookContext context, IOperationsService operationsService)
        {
            _context = context;
            _operationsService = operationsService;
        }

        [AllowAnonymous]
        [HttpGet("personaPorId/{Id}")]
        //En este endpoint se realiza una busqueda por la id del usuario dicho endpoint necesita un
        //DTOById que contiene los datos necesarios para realizar el get.
        public async Task<ActionResult> UserById([FromRoute] DTOById userData)
        {

            try
            {
                //Buscamos si la persona existe en base a la id de usuario que tenga
                var personExist = await _context.Personas.FirstOrDefaultAsync(x => x.UserId == userData.Id);
                //Convertimos el campo medicacion a un array seprado por coma  la medicacion esta asociada a una persona
               personExist.Medicacion= String.Join(",", personExist.Medicacion);
                //Si la id que tenga no existe decimos al usuario que no existe
                if (personExist == null)
                {
                    return NotFound("Datos de persona no encontrados");
                }
                //Agregamos la operacion
                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Consulta usuario por id",
                    UserId = personExist.Id
                });

                //Si todo va bien se devuelve un ok
                return Ok(personExist);
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido consultar los datos de la persona, por favor, intentelo más tarde.");
            }

        }
    }

}
