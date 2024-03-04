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

		public DeleteUserService(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        //Agregamos el metodo que esta en la interfaz este metodo tiene un DTODeleteUser que contiene 
        //los datos necesarios
        public async Task DeleteUser(DTODeleteUser delete)
        {
            //Para eliminar el usuario necesitamos primero localizar a la persona que tiene ese usuario
            //var usuarioDB = await _deleteUserServices.ObtenerUsuarioConRelaciones(delete.Id);
            var usuarioDB = await _context.Usuarios.Include(x => x.Mediciones).Include(x=>x.UsuarioMedicacions).FirstOrDefaultAsync(x => x.Id == delete.Id);
            //llamamos al servicio encargado de eliminar este servicio borra recursivamente
           
            _context.Mediciones.RemoveRange(usuarioDB.Mediciones);
            _context.UsuarioMedicacions.RemoveRange(usuarioDB.UsuarioMedicacions);
            //Eliminamos el usuario

            _context.Usuarios.Remove(usuarioDB);
            //Guardamos los cambios

            await _context.SaveChangesAsync();
            
        }

    }
}
