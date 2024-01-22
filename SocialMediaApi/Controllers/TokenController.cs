using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMediaApi.Controllers
{
    public class TokenController(IConfiguration configuration,
                                    ISecurityService security,
                                        IPasswordService passwordService) : ApiController
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
                return Ok(new { token, validation.Item2 });
            }

            return NotFound("User or password is invalid.");
        }

        private async Task<(bool, SecurityAndUserDTO)> IsValidUser(UserLogin userLogin)
        {
            var user = await _securityService.GetLoginByCredentials(userLogin);

            if (user != null)
            {
                var isValid = _passwordService.Check(user.Password, userLogin.Password);

                if (!isValid)
                    return (false, null!);
                else
                    user.Password = "";
                return (isValid, user);
            }
            return (false, null!);
        }

        private string GenerateToken(SecurityAndUserDTO security)
        {
            //Header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var sigingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(sigingCredentials);

            //Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, security.UserId.ToString()),
                new Claim(ClaimTypes.Name, $"{security.FirstName} {security.LastName}"),
                new Claim("User", security.User),
                new Claim(ClaimTypes.Role, security.Role.ToString()),
            };

            //Payload
            var payload = new JwtPayload(_configuration["Authentication:Issuer"],
                                            _configuration["Authentication:Audience"],
                                                claims,
                                                    DateTime.Now,
                                                     DateTime.UtcNow.AddMinutes(10));

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize]
        [HttpGet("Get-Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var currentUser = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            if (currentUser != 0)
            {
                var userSecurity = await _securityService.GetUserProfileById(currentUser);

                if (userSecurity != null)
                {
                    return Ok(userSecurity);
                }
            }
            return NotFound();
        }

    }
}