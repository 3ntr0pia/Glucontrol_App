using Microsoft.EntityFrameworkCore;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class ChangePassService : IChangePassService
    {
        //Llamamos a los servicios necesarios para que este servicio cumpla su funcion
        private readonly HashService _hashService;
        private readonly DiabetesNoteBookContext _context;
        private readonly IChangePassword _changePassword;
        //Creamos el contructor
        public ChangePassService(HashService hashService, DiabetesNoteBookContext context, IChangePassword changePassword)
        {
            _hashService = hashService;
            _context = context;
            _changePassword = changePassword;
        }
        //Agregamos el metodo task que esta en la interfaz con el DTOCambioPassPorId para poder cambiar la contraseña
        //una vez logueados, este dto dispone de los datos que se necesitan junto a la base de datos
        public async Task ChangePassId(DTOCambioPassPorId userData)
        {
            //Buscamos en base de datos al usuario por su id
            var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == userData.Id);
            //Llamamos al servicio _hashService este servicio tiene un metodo Hash  al cual se le pasa la 
            //nueva contraseña que cree el usuario para que la cifre
            var resultadoHash = _hashService.Hash(userData.NewPass);
            //A la contraseña que cree el usuario se cifra con el hash y se le asigna un salt
            usuarioDB.Password = resultadoHash.Hash;
            usuarioDB.Salt = resultadoHash.Salt;
            //Llamamos al servicio _changePassword el cual tiene un metodo SaveNewPassword para guardar los cambios
            //y se le pasa los cambios que quiere que se guarde
            await _changePassword.SaveNewPassword(usuarioDB);
        }

        

    }
}
