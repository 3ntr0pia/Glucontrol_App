using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz.
    public class DeleteMedicionService:IDeleteMedicionService
    {
        //Llamamos a base de datos y servicio para poder hacer uso de ellos

        private readonly DiabetesNoteBookContext _context;
     
		//Creamos el constructor

		public DeleteMedicionService(DiabetesNoteBookContext context, IDeleteMedicionServices deleteMedicionServices)
        {
            _context = context;
           
        }
        //Agregamos el metodo que esta en la interfaz el cual tiene un DTOEliminarMedicion que contiene
        //los datos necesarios para gestionar la eliminacion
        public async Task DeleteMedicion(DTOEliminarMedicion delete)
        {
            //Buscamos la medicion por id
            
			var deleteMedicion = await _context.Mediciones.FirstOrDefaultAsync(x => x.Id == delete.Id);
            //Llamamos al servicio encargado de eliminar y le pasamos lo que se va ha eliminar
            _context.Mediciones.Remove(deleteMedicion);
            await _context.SaveChangesAsync();
         
        }
    }
}
