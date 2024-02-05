using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz.
    public class UserDeregistrationService : IUserDeregistrationService
    {
        //Llamamos a la base de datos y servicios necesarios
        private readonly DiabetesNoteBookContext _context;
        private readonly IUserDeregistration _userDeregistration;
        //Creamos el constructor
        public UserDeregistrationService(DiabetesNoteBookContext context, IUserDeregistration userDeregistration)
        {
            _context = context;
            _userDeregistration = userDeregistration;
        }
        //Agregamos el metodo que esta en la interfaz junto a su DTOUserDeregistration
        public async Task UserDeregistration(DTOUserDeregistration delete)
        {
            //Buscamos al usuario por su id  y que persona tiene asociada
            var usuarioDB = await _context.Usuarios.Include(x => x.Personas).FirstOrDefaultAsync(x => x.Id == delete.Id);
            //Eliminamos de manera recursiva esa persona junto al usuario
            await _userDeregistration.UserDeregistrationSave(usuarioDB);
        }
    }
}
