using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.Services;
using DiabetesNoteBook.Application.Services.Genereics;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicacionesController : ControllerBase
    {
        private readonly ExistUsersService _existUsersService;
        private readonly ILogger<UsersController> _logger;
        private readonly INewMedicationService _newMedicationService;
        private readonly IConsultMedication _consultMedication;
        private readonly IDeleteMedication _deleteMedication;

        public MedicacionesController(ILogger<UsersController> logger,
            INewMedicationService newMedicationService, IConsultMedication consultMedication, 
            IDeleteMedication deleteMedication, ExistUsersService existUsersService)
        {
            _logger = logger;
            _newMedicationService = newMedicationService;
            _consultMedication = consultMedication;
            _deleteMedication = deleteMedication;
            _existUsersService = existUsersService;
        }

        [HttpPost("postMedication")]
        public async Task<ActionResult> PostMedication([FromBody] DTOMedicacion userData)
        {
            try
            {

                var UserExist = await _existUsersService.UserExistById(userData.Id);

                await _newMedicationService.NewRegister(new DTOMedicacion
                {
                    Id = userData.Id,
                    medicacion = userData.medicacion,
                });

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar la medicación");
                return BadRequest("En estos momentos no se ha podido realizar la insercción de la medicación, por favor, intentelo más tarde.");
            }
        }

        [HttpGet("getMedication")]
        public async Task<ActionResult> GetMedication(int id)
        {

            try
            {

                var UserExist = await _existUsersService.UserExistById(id);

                var medicationNames = await _consultMedication.GetMedication(new DTOMedicacion
                {
                    Id = id
                });

                return Ok(medicationNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar las medicaciones");
                return BadRequest("En estos momentos no se ha podido realizar la consulta de la medicación, por favor, intentelo más tarde.");
            }
        }

        [HttpDelete("deleteMedication")]
        public async Task<ActionResult> DeleteMedication(DTODeleteMedication UserData)
        {

            try
            {

                var usuarioDBUser = await _existUsersService.UserExistById(UserData.userId);

                await _deleteMedication.DeleteMedication(new DTODeleteMedication
                {
                    userId = UserData.userId,
                    medicationId = UserData.medicationId
                });

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al celiminar la medicacion");
                return BadRequest("En estos momentos no se ha podido realizar la consulta de la medicación, por favor, intentelo más tarde.");
            }
        }
    }
}
