using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
    public interface ISecurityService
    {
        Task<Security> GetLoginByCredentials(UserLogin userLogin);
        Task RegisterUserAndSecurity(Security security, User user);
    }
}