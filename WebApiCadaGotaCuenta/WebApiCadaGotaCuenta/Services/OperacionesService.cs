using WebApiCadaGotaCuenta.Models;

namespace WebApiCadaGotaCuenta.Services
{
    public class OperacionesService
    {
        private readonly GlucoControlContext _context;
        private readonly IHttpContextAccessor _accessor;

        public OperacionesService(GlucoControlContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task AddOperacion(string operacion, string controller)
        {
            Operacione nuevaOperacion = new Operacione()
            {
                FechaAccion = DateTime.Now,
                Operacion = operacion,
                Controller = controller,
                Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            await _context.Operaciones.AddAsync(nuevaOperacion);
            await _context.SaveChangesAsync();

            Task.FromResult(0);
        }
    }
}
