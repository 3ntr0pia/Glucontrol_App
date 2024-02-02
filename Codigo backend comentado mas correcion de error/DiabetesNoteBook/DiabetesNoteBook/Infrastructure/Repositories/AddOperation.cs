using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Infrastructure.Repositories
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class AddOperation : IAddOperation
    {
        //se llama a la base de datos para poder usarla
        private readonly DiabetesNoteBookContext _context;
        //Se crea un constructor
        public AddOperation(DiabetesNoteBookContext context)
        {
            _context = context;
        }
        //Agregamos el metodo que hay en la interfaz junto con el nombre de la tabla Operaciones
        public async Task SaveAddOpertion(Operacione operation)
        {
            //Agregamos los cambios a la base de datos
            await _context.Operaciones.AddAsync(operation);
            //Guardamos cambios en base de datos
            await _context.SaveChangesAsync();
        }
    }
}
