using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using DiabetesNoteBook.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    public class ChangeUserDataService : IChangeUserDataService
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly IChangeUserData _changeUserData;
        private readonly IEmailService _emailService;
        private readonly INewMedicacion _newMedicationRepository;
        private readonly INewUsuarioMedicacion _saveUsuarioMedicacionRepository;


        public ChangeUserDataService(DiabetesNoteBookContext context, IChangeUserData changeUserData, 
            IEmailService emailService, INewMedicacion newMedicationRepository, INewUsuarioMedicacion saveUsuarioMedicacionRepository)
        {
            _context = context;
            _changeUserData = changeUserData;
            _emailService = emailService;
            _newMedicationRepository = newMedicationRepository;
            _saveUsuarioMedicacionRepository = saveUsuarioMedicacionRepository;
        }

        public async Task ChangeUserData(DTOChangeUserData changeUserData)
        {

            var usuarioUpdate = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == changeUserData.Id);

            if(changeUserData.Avatar != usuarioUpdate.Avatar)
            {
                usuarioUpdate.Avatar = changeUserData.Avatar;
            }

            if (changeUserData.Nombre != usuarioUpdate.Nombre)
            {
                usuarioUpdate.Nombre = changeUserData.Nombre;
            }

            if (changeUserData.PrimerApellido != usuarioUpdate.PrimerApellido)
            {
                usuarioUpdate.PrimerApellido = changeUserData.PrimerApellido;
            }

            if (changeUserData.SegundoApellido != usuarioUpdate.SegundoApellido)
            {
                usuarioUpdate.SegundoApellido = changeUserData.SegundoApellido;
            }

            if (changeUserData.Sexo != usuarioUpdate.Sexo)
            {
                usuarioUpdate.Sexo = changeUserData.Sexo;
            }

            if (changeUserData.Edad != usuarioUpdate.Edad)
            {
                usuarioUpdate.Edad = changeUserData.Edad;
            }

            if (changeUserData.Peso != usuarioUpdate.Peso)
            {
                usuarioUpdate.Peso = changeUserData.Peso;
            }

            if (changeUserData.Edad != usuarioUpdate.Edad)
            {
                usuarioUpdate.Edad = changeUserData.Edad;
            }

            if (changeUserData.Altura != usuarioUpdate.Altura)
            {
                usuarioUpdate.Altura = changeUserData.Altura;
            }

            if (changeUserData.Actividad != usuarioUpdate.Actividad)
            {
                usuarioUpdate.Actividad = changeUserData.Actividad;
            }

            if (changeUserData.TipoDiabetes != usuarioUpdate.TipoDiabetes)
            {
                usuarioUpdate.TipoDiabetes = changeUserData.TipoDiabetes;
            }

            if (changeUserData.Insulina != usuarioUpdate.Insulina)
            {
                usuarioUpdate.Insulina = changeUserData.Insulina;
            }

            if (changeUserData.Insulina != usuarioUpdate.Insulina)
            {
                usuarioUpdate.Insulina = changeUserData.Insulina;
            }

            if (changeUserData.Email != usuarioUpdate.Email)
            {
                var emailUpdate = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == changeUserData.Email);

                if (emailUpdate == null)
                {
                    usuarioUpdate.Email = changeUserData.Email;
                    usuarioUpdate.ConfirmacionEmail = false;

                    await _emailService.SendEmailAsyncRegister(new DTOEmail
                    {
                        ToEmail = changeUserData.Email
                    });
                }

            }

            await _changeUserData.SaveChangeUserData(usuarioUpdate);

            var medicamentos = changeUserData.Medicacion.SelectMany(m => m.Split(','));

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
                    IdUsuario = usuarioUpdate.Id,
                    IdMedicacion = medicacion.IdMedicacion
                };

                _context.UsuarioMedicacions.Add(usuarioMedicacion);
                await _saveUsuarioMedicacionRepository.SaveNuevaUsuarioMedicacion(usuarioMedicacion);
            }

        }

    }
}
