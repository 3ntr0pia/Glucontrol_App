using CapaAplicacion.DTOs;
using CapaAplicacion.Interfaces;
using CapaDominio.Models;

namespace CapaAplicacion.Services
{
    public class NuevoUsuarioService : INuevoUsuarioService
    {

        private readonly GlucoControlContext _context;
        private readonly HashService _hashService;
        private readonly OperacionesService _operacionesService;

        public NuevoUsuarioService(GlucoControlContext context, HashService hashService,
            OperacionesService operacionesService)
        {
            _context = context;
            _hashService = hashService;
            _operacionesService = operacionesService;
        }

        public async Task ResgitroUsuario(DTOUsuarioRegistro request)
        {

                var resultadoHash = _hashService.Hash(request.Password);

                var newUsuario = new Usuario
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    Password = resultadoHash.Hash,
                    Salt = resultadoHash.Salt,
                    Rol = "user"
                };

                await _context.Usuarios.AddAsync(newUsuario);
                await _context.SaveChangesAsync();
                await _operacionesService.AddOperacionRegister("NuevoUsuarioService", "ResgitroUsuario", newUsuario.Id);

        }
    }
}