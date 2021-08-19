using Microsoft.Extensions.Options;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Options;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace SocialMedia.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordOptions options;

        public PasswordService(IOptions<PasswordOptions> _options)
        {
            this.options = _options.Value;
        }

        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            //using: una vez que el metodo se haya completado se libere es decir se realice un dispose
            //PBKDF2 implementation
            using (var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512))
            {
                var keyToCheck = algorithm.GetBytes(this.options.KeySize);

                return keyToCheck.SequenceEqual(key);
            }
        }

        public string Hash(string password)
        {
            //using: una vez que el metodo se haya completado se libere es decir se realice un dispose
            //PBKDF2 implementation
            using (var algorithm = new Rfc2898DeriveBytes(password, this.options.SaltSize, this.options.Iterations, HashAlgorithmName.SHA512))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(this.options.KeySize));

                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{this.options.Iterations}.{salt}.{key}";
            }
        }
    }
}

