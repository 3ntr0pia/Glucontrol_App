using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly DiabetesNoteBookContext _context;

        public TokenService(IConfiguration configuration, DiabetesNoteBookContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<DTOLoginResponse> GenerarToken(Usuario credencialesUsuario)
        {

            var personaDB = await _context.Personas.FirstOrDefaultAsync(x => x.UserId == credencialesUsuario.Id);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, credencialesUsuario.Rol)
            };

            var clave = _configuration["ClaveJWT"];
            var claveKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));
            var signinCredentials = new SigningCredentials(claveKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new DTOLoginResponse()
            {
                Token = tokenString,
                Rol = credencialesUsuario.Rol,
                Id = credencialesUsuario.Id,
                Nombre = personaDB.Nombre,
                PrimerApellido = personaDB.PrimerApellido,
                SegundoApellido = personaDB.SegundoApellido
            };
        }
    }
}
