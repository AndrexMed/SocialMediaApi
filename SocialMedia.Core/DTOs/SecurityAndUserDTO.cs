using SocialMedia.Core.Enumerations;

namespace SocialMedia.Core.DTOs
{
    public class SecurityAndUserDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string? Telephone { get; set; }
        public bool IsActive { get; set; }
        //--------------------------------------------
        public int SecurityId { get; set; }
        public string User { get; set; }
        //public string UserName { get; set; }
        public string Password { get; set; }
        public RoleType? Role { get; set; }
    }
}