﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<Security> GetLoginByCredentials(UserLogin userLogin)
        {
            return await _entities
                .FirstOrDefaultAsync(
                    x => x.User == userLogin.User && x.Password == userLogin.Password
                );
        }
    }
}