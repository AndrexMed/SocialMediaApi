using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        public async Task<IEnumerable<Posts>> GetPosts()
        {
            var posts = Enumerable.Range(1, 10).Select(x => new Posts
            {
                PostId = x,
                Description = $"Description {x}",
                Date = DateTime.Now,
                Image = $"https://misapis.com/{x}",
                UserID = x * 2
            });

            await Task.Delay(10);
            return posts;
        }
    }
}
