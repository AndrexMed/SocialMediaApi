using Microsoft.Extensions.Options;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Options;
using System.Security.Cryptography;

namespace SocialMedia.Infrastructure.Services
{
    public class PasswordService(IOptions<PasswordOptions> options) : IPasswordHasher
    {
        private readonly PasswordOptions _options = options.Value;
        public bool Check(string hash, string password)
        {
            throw new NotImplementedException();
        }

        public string Hash(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                _options.SaltSize,
                _options.Iterations
                ))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(_options.KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{_options.Iterations}.{salt}.{key}";
            }
        }
    }
}
