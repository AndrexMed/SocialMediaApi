namespace SocialMedia.Core.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; } = null!;
        public string? Image { get; set; }
    }
}
