using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMediaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authentication(UserLogin login)
        {
            if (IsValidUser()) 
            {
                var token = GenerateToken();
                return Ok(token);
            }

            return NotFound();
        }

        private bool IsValidUser()
        {
            return true;
        }

        private string GenerateToken()
        {
            //Header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var sigingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(sigingCredentials);

            //Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "Giovanni"),
                new Claim(ClaimTypes.Email, "andres@mail.com"),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            //Payload
            var payload = new JwtPayload(_configuration["Authentication:Issuer"],
                                            _configuration["Authentication:Audience"],
                                                claims,
                                                    DateTime.Now,
                                                     DateTime.UtcNow.AddMinutes(2));

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
