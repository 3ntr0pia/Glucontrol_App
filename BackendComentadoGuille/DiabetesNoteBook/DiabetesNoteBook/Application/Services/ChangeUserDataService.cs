using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class ChangeUserDataService : IChangeUserDataService
    {
        //Llamamos a los servicios necesarios para que este servicio cumpla su funcion
       //y tambien se llama a la base de datos
        
        private readonly DiabetesNoteBookContext _context;
        private readonly IChangeUserData _changeUserData;
        //Creamos el constructor
        public ChangeUserDataService(DiabetesNoteBookContext context, IChangeUserData changeUserData)
        {
            _context = context;
            _changeUserData = changeUserData;
        }
        //Ponemos el metodo que se encuentra en la interfaz el cual tiene un DTOChangeUserData que contiene
        //datos para poder hacer que este metodo cumpla su funcion
        public async Task ChangeUserData(DTOChangeUserData changeUserData)
        {
            //Buscamos en base de datos la id del usuario
            var usuarioUpdate = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == changeUserData.Id);
            //Buscamos en la tabla personas la id de la persona segun su id de usuario en otras
            //palabras busca por id de usuario y ese usuario tiene asociado una persona
            var personaUpdate = await _context.Personas.AsTracking().FirstOrDefaultAsync(x => x.UserId == changeUserData.Id);
            //Actulizamos los datos correspondientes al usuario y la persona
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
            personaUpdate.Medicacion = String.Join(",", changeUserData.Medicacion);
            personaUpdate.TipoDiabetes = changeUserData.TipoDiabetes;
            personaUpdate.Insulina = changeUserData.Insulina;
            //Llamamos al servicio encargado de guardar los cambios en la tabla usuarios.
            await _changeUserData.SaveChangeUserData(usuarioUpdate);
            //Llamamos al servicio encargado de guardar los cambios en la tabla personas.

            await _changeUserData.SaveChangePersonData(personaUpdate);

        }

    }
}
