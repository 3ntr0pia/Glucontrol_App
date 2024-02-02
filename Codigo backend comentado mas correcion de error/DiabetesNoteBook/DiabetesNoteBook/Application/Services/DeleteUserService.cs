using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz.
    public class DeleteUserService : IDeleteUserService
    {
        //Llamamos a base de datos y al servicio que va a usar
        private readonly DiabetesNoteBookContext _context;
        private readonly IDeleteUser _deleteUser;
        //Creamos el constructor
        public DeleteUserService(DiabetesNoteBookContext context, IDeleteUser deleteUser)
        {
            _context = context;
            _deleteUser = deleteUser;
        }
        //Agregamos el metodo que esta en la interfaz este metodo tiene un DTODeleteUser que contiene 
        //los datos necesarios
        public async Task DeleteUser(DTODeleteUser delete)
        {
            //Para eliminar el usuario necesitamos primero localizar a la persona que tiene ese usuario
            var usuarioDB = await _context.Usuarios.Include(x => x.Personas).FirstOrDefaultAsync(x => x.Id == delete.Id);
            //llamamos al servicio encargado de eliminar este servicio borra recursivamente
            await _deleteUser.DeleteUser(usuarioDB);
        }

    }
}
