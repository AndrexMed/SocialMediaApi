
namespace SocialMedia.Core.Entities
{
    public class Posts
    {
        public int PostId { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
    }
}
