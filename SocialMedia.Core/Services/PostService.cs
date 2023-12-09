using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Services
{
    public class PostService(IPostRepository postRepository,
                             IUserRepository userRepository) : IPostService
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _postRepository.GetPosts();
        }

        public async Task<Post> GetPost(int id)
        {
            return await _postRepository.GetPost(id);
        }

        public async Task InsertPost(Post post)
        {
            var user = await _userRepository.GetUser(post.UserId);

            if (user == null)
            {
                throw new Exception("User does'nt exist");
            }

            if (post.Description.Contains("Sexo") == true)
            {
                throw new Exception("Content not alowed");
            }
            await _postRepository.InsertPost(post);
        }

        public async Task<bool> UpdatePost(int id, Post post)
        {
            return await _postRepository.UpdatePost(id, post);
        }

        public async Task<bool> DeletePost(int id)
        {
            return await _postRepository.DeletePost(id);
        }
    }
}