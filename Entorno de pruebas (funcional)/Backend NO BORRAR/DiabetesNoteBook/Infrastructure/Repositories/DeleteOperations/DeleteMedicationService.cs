using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using DiabetesNoteBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    public class DeleteMedicationService : IDeleteMedication
    {

        private readonly DiabetesNoteBookContext _context;
        private readonly IDeleteUserMedication _deleteUserMedication;



        public DeleteMedicationService(DiabetesNoteBookContext context, IDeleteUserMedication deleteUserMedication )
        {
            _context = context;
            _deleteUserMedication= deleteUserMedication;
          

        }

        public async Task DeleteMedication(DTODeleteMedication userData)
        {

            var userMedication = await _context.UsuarioMedicacions.FirstOrDefaultAsync(x => x.IdUsuario == userData.userId && x.IdMedicacion == userData.medicationId);
           
            await _deleteUserMedication.DeleteUserMedications(userMedication);

        }
    }
}
