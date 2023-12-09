using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
    }
}
