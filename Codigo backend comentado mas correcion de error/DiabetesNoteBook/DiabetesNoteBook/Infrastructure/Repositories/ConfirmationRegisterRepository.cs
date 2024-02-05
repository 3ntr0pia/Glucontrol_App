using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class ConfirmationRegisterRepository : IConfirmationRegisterRepository
    {
        //Llamamos a base da datos para poder usarla
        private readonly DiabetesNoteBookContext _context;
        //Creamos el contructor
        public ConfirmationRegisterRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        //Agregamos el metodo que esta en la interfaz junto a  la tabla Usuarios
        public async Task ConfirmationRegisterTrue(Usuario confirm)
        {
            //Actualizamos los cambios en base de datos
            _context.Usuarios.Update(confirm);
            //Guardamos los cambios
            await _context.SaveChangesAsync();

        }
    }
}
