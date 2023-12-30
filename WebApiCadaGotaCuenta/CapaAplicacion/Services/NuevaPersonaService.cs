using CapaAplicacion.DTOs;
using CapaAplicacion.Interfaces;
using CapaDominio.Models;

namespace CapaAplicacion.Services
{
    public class NuevaPersonaService : INuevaPersonaService
    {
        private readonly GlucoControlContext _context;
        private readonly OperacionesService _operacionesService;

        public NuevaPersonaService(GlucoControlContext context, OperacionesService operacionesService)
        {
            _context = context;
            _operacionesService = operacionesService;
        }

        public async Task ResgitroPersona(DTOPersonaRegistro request)
        {

            var nuevaPersona = new Persona
            {
                Nombre = request.Nombre,
                PrimerApellido = request.PrimerApellido,
                SegundoApellido = request.SegundoApellido,
                Sexo = request.Sexo,
                Edad = request.Edad,
                Peso = request.Peso,
                Altura = request.Altura,
                TipoDiabetes = request.TipoDiabetes,
                UserId = request.UserId
            };

            await _context.Personas.AddAsync(nuevaPersona);
            await _context.SaveChangesAsync();
            await _operacionesService.AddOperacionRegister("NuevaPersonaService", "ResgitroPersona", request.UserId);

        }
    }
}
