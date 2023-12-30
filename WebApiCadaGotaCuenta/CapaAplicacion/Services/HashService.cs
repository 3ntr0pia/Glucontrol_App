using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using CapaAplicacion.Classes;

namespace CapaAplicacion.Services
{
    public class HashService
    {
        public ResultadoHash Hash(string password)
        {

            var salt = new byte[16];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            return Hash(password, salt);
        }


        public ResultadoHash Hash(string password, byte[] salt)
        {

            var claveDerivada = KeyDerivation.Pbkdf2(password: password,
                salt: salt, prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32);

            var hash = Convert.ToBase64String(claveDerivada);

            return new ResultadoHash()
            {
                Hash = hash,
                Salt = salt
            };

        }
    }
}
