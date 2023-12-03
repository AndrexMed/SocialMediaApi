using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMediaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Posts()
        {
            var posts = await _postRepository.GetPosts();

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Post(int id)
        {
            var post = await _postRepository.GetPost(id);

            return Ok(post);
        }
    }
}
