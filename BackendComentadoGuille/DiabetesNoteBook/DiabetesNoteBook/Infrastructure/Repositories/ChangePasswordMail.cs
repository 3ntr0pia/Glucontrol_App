using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class ChangePasswordMail : IChangePasswordMail
    {
        //Llamamos a la base de datos
        private readonly DiabetesNoteBookContext _context;
        //Creamos el contructor
        public ChangePasswordMail(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        //Agregamos el metodo que esta en la interfaz junto a  la tabla Usuarios
        public async Task SaveNewPasswordMail(Usuario operation)
        {
            //Actualizamos los cambios directamente en la tabla Usuarios
            _context.Usuarios.Update(operation);
            //Guardamos los cambios en base de datos
            await _context.SaveChangesAsync();
        }
    }
}
