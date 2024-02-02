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
        //Llamamos a los necesario para que este servicio cumpla con su funcion
        private readonly IConfiguration _configuration;
        private readonly DiabetesNoteBookContext _context;
        //Creamos el constructor
        public TokenService(IConfiguration configuration, DiabetesNoteBookContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        //Creamos un metodo asincrono de tipo DTOLoginResponse el cual como parametro se le pasa 
        //la tabla usuarios
        public async Task<DTOLoginResponse> GenerarToken(Usuario credencialesUsuario)
        {
            //Buscamos a la persona por su id, concretamente su usuario
            var personaDB = await _context.Personas.FirstOrDefaultAsync(x => x.UserId == credencialesUsuario.Id);
            //buscamos al usuario por id
            var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == credencialesUsuario.Id);
            //En los claims del token guardamos el rol del usuario
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, credencialesUsuario.Rol)
            };
            //Le pasamos la clave de firma para cifrar el token
            var clave = _configuration["ClaveJWT"];
            //Cifra el token
            var claveKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));
            //Algoritmo de cifrado
            var signinCredentials = new SigningCredentials(claveKey, SecurityAlgorithms.HmacSha256);
            //Caracteristicas del token
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            //Lo que devuelve ese token 
            return new DTOLoginResponse()
            {
                //Devuelve el token
                Token = tokenString,
                //Rol de usuario
                Rol = credencialesUsuario.Rol,
                //Id de usuario
                Id = credencialesUsuario.Id,
                //nombre completo de la persona
                Nombre = personaDB.Nombre,
                PrimerApellido = personaDB.PrimerApellido,
                SegundoApellido = personaDB.SegundoApellido,
                //Avatar del usuario
                Avatar = usuarioDB.Avatar
            };
        }
    }
}
