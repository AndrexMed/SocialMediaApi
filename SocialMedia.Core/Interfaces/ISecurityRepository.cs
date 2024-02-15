using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
    public interface ISecurityRepository : IRepository<Security>
    {
        Task<SecurityAndUserDTO> GetLoginByCredentials(UserLogin userLogin);
        Task<SecurityAndUserDTO> GetUserAndProfileById(int id);
        Task<string> RecoverPassword(RecoverPassDTO recoverPassword);
    }
}