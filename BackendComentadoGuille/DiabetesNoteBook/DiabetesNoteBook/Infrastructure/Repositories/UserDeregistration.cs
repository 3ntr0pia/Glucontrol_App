using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class UserDeregistration : IUserDeregistration
    {
        //Llamamos a la base de datos para poder usarla
        private readonly DiabetesNoteBookContext _context;
        //Creamos el contructor
        public UserDeregistration(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        //Agregamos el metodo que esta en la interfaz junto a la tabla usuarios
        public async Task UserDeregistrationSave(Usuario delete)
        {
            //Eliminamos recursivamente sobre la tabla personas
            _context.Personas.RemoveRange(delete.Personas);
            //Eliminamos el usuario
            _context.Usuarios.Remove(delete);
            //Guardamos cambios
            await _context.SaveChangesAsync();
        }
    }
}
