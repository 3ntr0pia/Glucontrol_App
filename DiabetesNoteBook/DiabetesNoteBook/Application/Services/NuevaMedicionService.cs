using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;

namespace DiabetesNoteBook.Application.Services
{
    public class NuevaMedicionService:INuevaMedicionService
    {
        private readonly DiabetesNoteBookContext _context;
        private readonly ISaveNuevaMedicion _saveNuevaMedicion;

        public NuevaMedicionService(DiabetesNoteBookContext context, ISaveNuevaMedicion saveNuevaMedicion)
        {
            _context = context;
            _saveNuevaMedicion = saveNuevaMedicion;
        }

        public async Task NuevaMedicion(DTOMediciones mediciones)
        {
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
                Notas = mediciones.Notas,
                IdPersona = mediciones.Id_Persona,
            };
            await _saveNuevaMedicion.SaveNuevaMecion(nuevaMedicion);


        }
    }
}
