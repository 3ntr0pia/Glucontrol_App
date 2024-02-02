using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class SaveNuevaMedicionRepository:ISaveNuevaMedicion
    {
        //Llamamos a la  base de datos para poder usarla
        private readonly DiabetesNoteBookContext _context;
        //Creamos el contructor
        public SaveNuevaMedicionRepository(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        //Agregamos el metodo que esta en la interfaz junto a  la tabla Mediciones
        public  async Task SaveNuevaMecion(Medicione newmedicion)
        {
            //Agregamos los datos directamente a la tabla mediciones
            await _context.Mediciones.AddAsync(newmedicion);
            //Guardamos los cambios
            await _context.SaveChangesAsync();
        }

        
    }
}
