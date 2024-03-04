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

            var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == credencialesUsuario.Id);

            var medicationsIDs = await _context.UsuarioMedicacions
                .Where(x => x.IdUsuario == usuarioDB.Id)
                .Select(x => x.IdMedicacion)
                .ToListAsync();

            var medicationNames = await _context.Medicaciones
                .Where(m => medicationsIDs.Contains(m.IdMedicacion))
                .Select(m => m.Nombre)
                .ToListAsync();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, credencialesUsuario.Rol),
                new Claim(ClaimTypes.NameIdentifier, credencialesUsuario.Id.ToString()),
                new Claim(ClaimTypes.Email, credencialesUsuario.Email),

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
                Id = credencialesUsuario.Id,
                Token = tokenString,
                Rol = credencialesUsuario.Rol,
                Nombre = credencialesUsuario.Nombre,
                PrimerApellido = credencialesUsuario.PrimerApellido,
                SegundoApellido = credencialesUsuario.SegundoApellido,
                Avatar = usuarioDB.Avatar,
                UserName = usuarioDB.UserName,
                Sexo = usuarioDB.Sexo,
                Edad = usuarioDB.Edad,
                Peso = usuarioDB.Peso,
                Altura = usuarioDB.Altura,
                Actividad = usuarioDB.Actividad,
                TipoDiabetes = usuarioDB.TipoDiabetes,
                Medicación = medicationNames.ToArray(),
                Insulina = usuarioDB.Insulina
            };
        }
    }
}
