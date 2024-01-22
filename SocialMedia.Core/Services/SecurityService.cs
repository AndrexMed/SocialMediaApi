using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Services
{
    public class SecurityService(IUnitOfWork unitOfWork) : ISecurityService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<SecurityAndUserDTO> GetLoginByCredentials(UserLogin userLogin)
        {
            return await _unitOfWork.SecurityRepository.GetLoginByCredentials(userLogin);
        }

        public async Task<SecurityAndUserDTO> GetUserProfileById(int id)
        {
            return await _unitOfWork.SecurityRepository.GetUserAndProfileById(id);
        }

        public async Task RegisterUserAndSecurity(Security security, User user)
        {
            await _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();

            var securityToInsert = new Security
            {
                User = user.FirstName + user.LastName,
                UserName = $"{user.FirstName} {user.LastName}",
                Password = security.Password,
                //Role = security.Role,
                UserId = user.Id
            };

            await _unitOfWork.SecurityRepository.Add(securityToInsert);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}