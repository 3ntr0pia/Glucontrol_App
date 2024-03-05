using DiabetesNoteBook.Application.DTOs;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Infrastructure.Interfaces;
using System.Security.Claims;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz.
    public class NuevaMedicionService:INuevaMedicionService
    {
        //Se llama a base de datos y al servicio.

        private readonly DiabetesNoteBookContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		//Se crea el constructor

		public NuevaMedicionService(DiabetesNoteBookContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _httpContextAccessor = accessor;
        }
        //Agregamos el metodo que se encuentra en la interfaz junto con el DTOMediciones

        public async Task NuevaMedicion(DTOMediciones mediciones)
        {
            //Creamos una nueva medicion
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int id;
            if (int.TryParse(userId, out id))
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
                    RacionHc = mediciones.RacionHC,
                    Notas = mediciones.Notas,
                    IdUsuario = id

                };
                //Llamamos al servicio para que la guarde
                await _context.Mediciones.AddAsync(nuevaMedicion);
                //Guardamos los cambios

                await _context.SaveChangesAsync();
            }
          




        }
    }
}
