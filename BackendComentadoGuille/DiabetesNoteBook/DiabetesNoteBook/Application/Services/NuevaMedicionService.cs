using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz.
    public class NuevaMedicionService:INuevaMedicionService
    {
        //Se llama a base de datos y al servicio.
        private readonly DiabetesNoteBookContext _context;
        private readonly ISaveNuevaMedicion _saveNuevaMedicion;
        //Se crea el constructor
        public NuevaMedicionService(DiabetesNoteBookContext context, ISaveNuevaMedicion saveNuevaMedicion)
        {
            _context = context;
            _saveNuevaMedicion = saveNuevaMedicion;
        }
        //Agregamos el metodo que se encuentra en la interfaz junto con el DTOMediciones
        public async Task NuevaMedicion(DTOMediciones mediciones)
        {
            //Creamos una nueva medicion
            var nuevaMedicion = new Medicione()
            {
                Fecha = mediciones.Fecha,
                Regimen = mediciones.Regimen,
                PreMedicion = mediciones.PreMedicion,
                PostMedicion = mediciones.PostMedicion,
                GlucemiaCapilar = mediciones.GlucemiaCapilar,
                BolusComida = mediciones.BolusComida,
                BolusCorrector = mediciones.BolusCorrector,
                PreDeporte = mediciones.PreDeporte,
                DuranteDeporte = mediciones.DuranteDeporte,
                PostDeporte = mediciones.PostDeporte,
                RacionHc= mediciones.RacionHC,
                Notas = mediciones.Notas,
                IdPersona = mediciones.Id_Persona,
            };
            //Llamamos al servicio para que la guarde
            await _saveNuevaMedicion.SaveNuevaMecion(nuevaMedicion);


        }
    }
}
