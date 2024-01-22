using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        private readonly SocialMediaContext _context;
        public SecurityRepository(SocialMediaContext context) : base(context)
        {
            _context = context;
        }

        private IQueryable<SecurityAndUserDTO> GetUserAndSecurityQuery()
        {
            return from user in _context.Users
                   join security in _context.Security on user.Id equals security.UserId
                   select new SecurityAndUserDTO
                   {
                       UserId = user.Id,
                       FirstName = user.FirstName,
                       LastName = user.LastName,
                       Email = user.Email,
                       DateOfBirth = user.DateOfBirth,
                       Telephone = user.Telephone,
                       IsActive = user.IsActive,

                       SecurityId = security.Id,
                       User = security.User,
                       Role = security.Role,
                       Password = security.Password,
                   };
        }

        public async Task<SecurityAndUserDTO> GetLoginByCredentials(UserLogin userLogin)
        {
            var userSecurity = await GetUserAndSecurityQuery()
                .Where(s => s.User == userLogin.User)
                .FirstOrDefaultAsync();

            return userSecurity ?? throw new Exception("No se encontró el usuario.");
        }

        public async Task<SecurityAndUserDTO> GetUserAndProfileById(int id)
        {
            var userSecurity = await GetUserAndSecurityQuery()
                .Where(s => s.UserId == id)
                .FirstOrDefaultAsync();

            return userSecurity ?? throw new Exception("No se encontró el usuario.");
        }

    }
}