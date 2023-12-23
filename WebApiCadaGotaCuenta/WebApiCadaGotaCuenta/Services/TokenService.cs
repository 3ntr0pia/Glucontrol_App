using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiCadaGotaCuenta.DTOs;
using WebApiCadaGotaCuenta.Models;

namespace WebApiCadaGotaCuenta.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DTOLoginResponse GenerarToken(Usuario credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, credencialesUsuario.Email),
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
                UserName = credencialesUsuario.UserName,
                Rol = credencialesUsuario.Rol
            };
        }
    }
}
