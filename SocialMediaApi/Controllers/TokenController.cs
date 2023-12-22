using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMediaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(IConfiguration configuration,
                                    ISecurityService security,
                                        IPasswordService passwordService) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ISecurityService _securityService = security;
        private readonly IPasswordService _passwordService = passwordService;

        [HttpPost]
        public async Task<IActionResult> Authentication(UserLogin login)
        {
            var validation = await IsValidUser(login);

            if (validation.Item1) // Item1 = response bool
            {
                var token = GenerateToken(validation.Item2); // Item2 = response of security
                return Ok(new { token });
            }

            return NotFound();
        }

        private async Task<(bool, Security)> IsValidUser(UserLogin userLogin)
        {
            var user = await _securityService.GetLoginByCredentials(userLogin);

            var isValid = _passwordService.Check(user.Password, userLogin.Password);

            return (isValid, user);
        }

        private string GenerateToken(Security security)
        {
            //Header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var sigingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(sigingCredentials);

            //Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, security.UserName),
                new Claim("User", security.User),
                new Claim(ClaimTypes.Role, security.Role.ToString()),
            };

            //Payload
            var payload = new JwtPayload(_configuration["Authentication:Issuer"],
                                            _configuration["Authentication:Audience"],
                                                claims,
                                                    DateTime.Now,
                                                     DateTime.UtcNow.AddMinutes(1));

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
