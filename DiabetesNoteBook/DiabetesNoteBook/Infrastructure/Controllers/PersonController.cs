using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PersonController : ControllerBase
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly IOperationsService _operationsService;

        public PersonController(DiabetesNoteBookContext context, IOperationsService operationsService)
        {
            _context = context;
            _operationsService = operationsService;
        }

        [AllowAnonymous]
        [HttpGet("personaPorId/{Id}")]
        public async Task<ActionResult> UserById([FromRoute] DTOById userData)
        {

            try
            {
                var personExist = await _context.Personas.FirstOrDefaultAsync(x => x.UserId == userData.Id);

                if (personExist == null)
                {
                    return NotFound("Datos de persona no encontrados");
                }

                await _operationsService.AddOperacion(new DTOOperation
                {
                    Operacion = "Consulta usuario por id",
                    UserId = personExist.Id
                });


                return Ok(personExist);
            }
            catch
            {
                return BadRequest("En estos momentos no se ha podido consultar los datos de la persona, por favor, intentelo más tarde.");
            }

        }
    }

}
