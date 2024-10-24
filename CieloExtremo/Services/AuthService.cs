using CieloExtremo.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace CieloExtremo.Services
{    

    public class AuthService : IAuthService
    {
        private readonly AdminUser _adminUser;
        private readonly PasswordHasher<string> _passwordHasher;

        public AuthService(IOptions<AdminUser> adminUser)
        {
            _adminUser = adminUser.Value;
            _passwordHasher = new PasswordHasher<string>();
        }

        public bool ValidateUser(string username, string password)
        {
            var passwordHasher = new PasswordHasher<string>();

            // Comparar el hash de la contraseña ingresada con el almacenado
            var result = passwordHasher.VerifyHashedPassword(null, _adminUser.Password, password);

            return username == _adminUser.Username && result == PasswordVerificationResult.Success;
        }
    }
}
