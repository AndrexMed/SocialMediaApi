using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
    public interface ISecurityService
    {
        Task<SecurityAndUserDTO> GetLoginByCredentials(UserLogin userLogin);
        Task RegisterUserAndSecurity(Security security, User user);
        Task<SecurityAndUserDTO> GetUserProfileById(int id);
    }
}