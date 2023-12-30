using CapaDominio.Models;
using Microsoft.AspNetCore.Http;

namespace CapaAplicacion.Services
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

        public async Task AddOperacionLogin(string operacion, string controller, string usuario)
        {
            var userId = _context.Usuarios.FirstOrDefault(x => x.UserName == usuario);

            Operacione nuevaOperacion = new Operacione()
            {
                FechaAccion = DateTime.Now,
                Operacion = operacion,
                Controller = controller,
                Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                IdUsuario = userId.Id
            };

            await _context.Operaciones.AddAsync(nuevaOperacion);
            await _context.SaveChangesAsync();

            Task.FromResult(0);

        }

        public async Task AddOperacionChangePassword(string operacion, string controller, int id)
        {

            Operacione nuevaOperacion = new Operacione()
            {
                FechaAccion = DateTime.Now,
                Operacion = operacion,
                Controller = controller,
                Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                IdUsuario = id
            };

            await _context.Operaciones.AddAsync(nuevaOperacion);
            await _context.SaveChangesAsync();

            Task.FromResult(0);

        }

        public async Task AddOperacionRegister(string operacion, string controller, int id)
        {

            Operacione nuevaOperacion = new Operacione()
            {
                FechaAccion = DateTime.Now,
                Operacion = operacion,
                Controller = controller,
                Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                IdUsuario = id
            };

            await _context.Operaciones.AddAsync(nuevaOperacion);
            await _context.SaveChangesAsync();

            Task.FromResult(0);

        }

    }
}
