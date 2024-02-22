using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Infrastructure.Repositories;

namespace DiabetesNoteBook.Application.Services
{
    public class NewRegisterService : INewRegister
    {

        private readonly DiabetesNoteBookContext _context;
        private readonly HashService _hashService;
        private readonly INewRegisterRepository _newRegisterRepository;
        private readonly INewMedicacion _newMedicationRepository;
        private readonly INewUsuarioMedicacion _saveUsuarioMedicacionRepository;

        public NewRegisterService(DiabetesNoteBookContext context, HashService hashService, INewRegisterRepository newRegisterRepository,
            INewMedicacion newMedicationRepository, INewUsuarioMedicacion saveUsuarioMedicacionRepository)
        {
            _context = context;
            _hashService = hashService;
            _newRegisterRepository = newRegisterRepository;
            _newMedicationRepository = newMedicationRepository;
            _saveUsuarioMedicacionRepository = saveUsuarioMedicacionRepository;
        }

        public async Task NewRegister(DTORegister userData)
        {



                var resultadoHash = _hashService.Hash(userData.Password);

                var newUsuario = new Usuario
                {
                    Avatar = userData.Avatar,
                    UserName = userData.UserName,
                    Email = userData.Email,
                    Password = resultadoHash.Hash,
                    Salt = resultadoHash.Salt,
                    Rol = "user",
                    Nombre = userData.Nombre,
                    PrimerApellido = userData.PrimerApellido,
                    SegundoApellido = userData.SegundoApellido,
                    Sexo = userData.Sexo,
                    Edad = userData.Edad,
                    Peso = userData.Peso,
                    Altura = userData.Altura,
                    Actividad = userData.Actividad,
                    TipoDiabetes = userData.TipoDiabetes,
                    Insulina = userData.Insulina
                };

                await _newRegisterRepository.SaveNewRegisterUser(newUsuario);

                var medicamentos = userData.Medicacion.SelectMany(m => m.Split(','));

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
                        IdUsuario = newUsuario.Id,
                        IdMedicacion = medicacion.IdMedicacion
                    };

                    _context.UsuarioMedicacions.Add(usuarioMedicacion);
                    await _saveUsuarioMedicacionRepository.SaveNuevaUsuarioMedicacion(usuarioMedicacion);
                }

        }
    }
}