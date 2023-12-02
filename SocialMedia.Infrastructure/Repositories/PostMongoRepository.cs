using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostMongoRepository : IPostRepository
    {
        public async Task<IEnumerable<Posts>> GetPosts()
        {
            var posts = Enumerable.Range(1, 10).Select(x => new Posts
            {
                PostId = x,
                Description = $"Description MONGO{x}",
                Date = DateTime.Now,
                Image = $"https://misapis.com/{x}",
                UserID = x * 2
            });

            await Task.Delay(10);
            return posts;
        }
    }
}
