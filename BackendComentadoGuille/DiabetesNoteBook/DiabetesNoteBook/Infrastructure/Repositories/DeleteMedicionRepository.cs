using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{ 
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class DeleteMedicionRepository:IDeleteMedicion
    {
        //Llamamos a la base de datos para poder usarla
        private readonly DiabetesNoteBookContext _context;
        //Creamos el constructor
        public DeleteMedicionRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        //Agregamos el metodo que esta en la interfaz junto a la tabla Mediciones
        public async Task DeleteMedicion(Medicione delete)
        {
            //Eliminamos los datos directamente de la base de datos
            _context.Mediciones.Remove(delete);
            //Guardamos los cambios
            await _context.SaveChangesAsync();
        }
    }
}
