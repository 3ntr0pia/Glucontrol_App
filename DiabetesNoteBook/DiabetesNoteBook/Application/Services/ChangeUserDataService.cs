using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    public class ChangeUserDataService : IChangeUserDataService
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly IChangeUserData _changeUserData;

        public ChangeUserDataService(DiabetesNoteBookContext context, IChangeUserData changeUserData)
        {
            _context = context;
            _changeUserData = changeUserData;
        }

        public async Task ChangeUserData(DTOChangeUserData changeUserData)
        {

            var usuarioUpdate = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == changeUserData.Id);

            var personaUpdate = await _context.Personas.AsTracking().FirstOrDefaultAsync(x => x.Id == changeUserData.Id); //Ajustado a id de persona

            usuarioUpdate.Avatar = changeUserData.Avatar;
            usuarioUpdate.UserName = changeUserData.UserName;
            personaUpdate.Nombre = changeUserData.Nombre;
            personaUpdate.PrimerApellido = changeUserData.PrimerApellido;
            personaUpdate.SegundoApellido = changeUserData.SegundoApellido;
            personaUpdate.Sexo = changeUserData.Sexo;
            personaUpdate.Edad = changeUserData.Edad;
            personaUpdate.Peso = changeUserData.Peso;
            personaUpdate.Altura = changeUserData.Altura;
            personaUpdate.Actividad = changeUserData.Actividad;
            personaUpdate.Medicacion = string.Join(",", changeUserData.Medicacion);
            personaUpdate.TipoDiabetes = changeUserData.TipoDiabetes;
            personaUpdate.Insulina = changeUserData.Insulina;

            await _changeUserData.SaveChangeUserData(usuarioUpdate);

            await _changeUserData.SaveChangePersonData(personaUpdate);

        }

    }
}
