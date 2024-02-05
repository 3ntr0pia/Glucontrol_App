using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class DeleteUserRepository : IDeleteUser
    {
        //Llamamos a la base de datos para poder usarla
        private readonly DiabetesNoteBookContext _context;
        //Creamos el constructor
        public DeleteUserRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        //Agregamos el metodo que esta en la interfaz junto a  la tabla Usuarios
        public async Task DeleteUser(Usuario delete)
        {
            //Hacemos un borrado recursiobo sobre la tabla personas
            _context.Personas.RemoveRange(delete.Personas);
            //Eliminamos el usuario
            _context.Usuarios.Remove(delete);
            //Guardamos los cambios
            await _context.SaveChangesAsync();
        }
    }
}
