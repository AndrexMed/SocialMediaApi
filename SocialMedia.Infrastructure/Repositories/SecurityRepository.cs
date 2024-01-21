using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        public SecurityRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<SecurityAndUserDTO> GetLoginByCredentials(UserLogin userLogin)
        {
            var user = await _entities.FirstOrDefaultAsync(x => x.User == userLogin.User);

            if (user == null)
            {
                return null!;
            }

            return new SecurityAndUserDTO
            {
                UserId = user.UserId,
                FirstName = "Test",
                LastName = "Test 2",
                Email = "Email",
                DateOfBirth = new DateOnly(),
                Telephone = "123456",
                IsActive = true,

                SecurityId = user.Id,
                User = user.User,
                Role = user.Role,
                Password = user.Password,
            };
        }
    }
}