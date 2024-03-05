using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    public class NewMedicationService : INewMedicationService
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly INewMedicacion _newMedicationRepository;
        private readonly INewUsuarioMedicacion _saveUsuarioMedicacionRepository;

        public NewMedicationService(DiabetesNoteBookContext context, INewMedicacion newMedicationRepository,
            INewUsuarioMedicacion saveUsuarioMedicacionRepository)
        {
            _context = context;
            _newMedicationRepository = newMedicationRepository;
            _saveUsuarioMedicacionRepository = saveUsuarioMedicacionRepository;
        }

        public async Task NewRegister(DTOMedicacion userData)
        {
            var medicamentos = userData.medicacion.SelectMany(m => m.Split(','));

            foreach (var medicacionNombre in medicamentos)
            {
                var nombreMedicacion = medicacionNombre.Trim();
                var medicacionExistente = await _context.Medicaciones.FirstOrDefaultAsync(m => m.Nombre == nombreMedicacion);
               
                if (medicacionExistente == null)
                {
                    var nuevaMedicacion = new Medicacione { Nombre = nombreMedicacion };
                    _context.Medicaciones.Add(nuevaMedicacion);

                    await _newMedicationRepository.SaveNewMedication(nuevaMedicacion);
                }
            }

            foreach (var medicacionNombre in medicamentos)
            {
                var nombreMedicacion = medicacionNombre.Trim();
                var medicacion = await _context.Medicaciones.FirstOrDefaultAsync(m => m.Nombre == nombreMedicacion);

                var usuarioMedicacion = new UsuarioMedicacion
                {
                    IdUsuario = userData.Id,
                    IdMedicacion = medicacion.IdMedicacion
                };

                _context.UsuarioMedicacions.Add(usuarioMedicacion);

                await _saveUsuarioMedicacionRepository.SaveNuevaUsuarioMedicacion(usuarioMedicacion);
            }
        }
    }
}
