using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class ChangePassword : IChangePassword
    {
        //Llamamos a la base de datos para poder usarla
        private readonly DiabetesNoteBookContext _context;
        //Creamos el constructor
        public ChangePassword(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        //Agregamos el metodo que esta en la interfaz junto con el nombre de la tabla
        public async Task SaveNewPassword(Usuario operation)
        {
            //Actualizamos los datos en la tabla Usuarios
            _context.Usuarios.Update(operation);
            //Guardamos los cambios en base de datos
            await _context.SaveChangesAsync();
        }
    }
}
