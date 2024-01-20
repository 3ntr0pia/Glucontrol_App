using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.DTOs;

namespace DiabetesNoteBook.Application.Services
{
    public class NewRegisterService : INewRegister
    {

        private readonly DiabetesNoteBookContext _context;
        private readonly HashService _hashService;
        private readonly INewRegisterRepository _newRegisterRepository;

        public NewRegisterService(DiabetesNoteBookContext context, HashService hashService, INewRegisterRepository newRegisterRepository)
        {
            _context = context;
            _hashService = hashService;
            _newRegisterRepository = newRegisterRepository;
        }

        public async Task NewRegister(DTORegister userData)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var resultadoHash = _hashService.Hash(userData.Password);

                    var newUsuario = new Usuario
                    {
                        Avatar = userData.Avatar,
                        UserName = userData.UserName,
                        Email = userData.Email,
                        Password = resultadoHash.Hash,
                        Salt = resultadoHash.Salt,
                        Rol = "user"
                    };

                    // Guardar el usuario
                    await _newRegisterRepository.SaveNewRegisterUser(newUsuario);

                    // Obtener el usuario después de guardarlo
                    var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.UserName == userData.UserName);

                    var nuevaPersona = new Persona
                    {
                        Nombre = userData.Nombre,
                        PrimerApellido = userData.PrimerApellido,
                        SegundoApellido = userData.SegundoApellido,
                        Sexo = userData.Sexo,
                        Edad = userData.Edad,
                        Peso = userData.Peso,
                        Altura = userData.Altura,
                        Actividad = userData.Actividad,
                        //Medicacion = userData.Medicacion,
                        TipoDiabetes = userData.TipoDiabetes,
                        Insulina = userData.Insulina,
                        UserId = usuario.Id
                    };

                    // Guardar la persona
                    await _newRegisterRepository.SaveNewRegisterPerson(nuevaPersona);

                    transaction.Commit();
                }
                catch
                {

                    transaction.Rollback();

                }
            }
        }
    }
}