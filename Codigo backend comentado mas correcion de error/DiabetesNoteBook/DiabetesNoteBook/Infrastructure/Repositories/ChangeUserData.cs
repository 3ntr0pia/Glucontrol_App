using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class ChangeUserData : IChangeUserData
    {
        //Llamamos a la base de datos para poder usarla
        private readonly DiabetesNoteBookContext _context;
        //Creamos el contructor
        public ChangeUserData(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        //Agregamos el metodo que esta en la interfaz junto a  la tabla usuarios
        public async Task SaveChangeUserData(Usuario operation)
        {
            //Actualizamos los cambios directamente en base de datos
            _context.Usuarios.Update(operation);
            //Guardamos los cambios
            await _context.SaveChangesAsync();
        }
        //Creamos un metodo tipo task el cual recibe los datos directamente de la tabla Personas
        public async Task SaveChangePersonData(Persona operation)
        {
            //Actualizamos los cambios directamente en base de datos
            _context.Personas.Update(operation);
            //Guardamos los cambios en base de datos
            await _context.SaveChangesAsync();
        }
    }
}
