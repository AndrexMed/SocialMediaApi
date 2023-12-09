using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMediaApi.Responses;

namespace SocialMediaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IPostRepository postRepository, IMapper mapper, IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postService.GetPosts();

            var postsDTOs = _mapper.Map<IEnumerable<PostDTO>>(posts);

            var response = new ApiResponse<IEnumerable<PostDTO>>(postsDTOs);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPost(id);
            var postDTO = _mapper.Map<PostDTO>(post);

            var response = new ApiResponse<PostDTO>(postDTO);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PostDTO postDTO)
        {
            var post = _mapper.Map<Post>(postDTO);  

            await _postService.InsertPost(post);

            postDTO = _mapper.Map<PostDTO>(post);

            var response = new ApiResponse<PostDTO>(postDTO);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, PostDTO postDTO)
        {
            var postToUpdate = _mapper.Map<Post>(postDTO);
            postToUpdate.PostId = id;

            var result = await _postService.UpdatePost(id, postToUpdate);
            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _postService.DeletePost(id);

            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }
    }
}
