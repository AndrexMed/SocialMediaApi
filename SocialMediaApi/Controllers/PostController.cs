using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;
using SocialMediaApi.Responses;
using System.Net;

namespace SocialMediaApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IMapper mapper,
                                    IPostService postService,
                                        IUriService uriService) : ControllerBase
    {
        private readonly IPostService _postService = postService;
        private readonly IMapper _mapper = mapper;
        private readonly IUriService _uriService = uriService;

        /// <summary>
        /// Test comments
        /// </summary>
        /// <param name="postQueryFilter">filters to apply</param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetPosts))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetPosts([FromQuery]PostQueryFilter postQueryFilter)
        {
            var posts = _postService.GetPosts(postQueryFilter);

            var postsDTOs = _mapper.Map<IEnumerable<PostDTO>>(posts);

            var metadata = new MetaData
            {
                TotalCount = posts.TotalCount,
                PageSize = posts.PageSize,
                CurrentPage = posts.CurrentPage,
                TotalPages = posts.TotalPages,
                HasNextPage = posts.HasNextPage,
                HasPreviousPage = posts.HasPreviousPage,

                NextPageUrl = _uriService.GetPostPaginatorUri(postQueryFilter, Url.RouteUrl(nameof(GetPosts))).ToString()
            };

            var response = new ApiResponse<IEnumerable<PostDTO>>(postsDTOs)
            {
                metaData = metadata
            };

            Response.Headers.Add("x-Pagination", JsonConvert.SerializeObject(metadata));
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
            postToUpdate.Id = id;

            var result = await _postService.UpdatePost(postToUpdate);
            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }

        [Authorize(Roles = nameof(RoleType.Administrator))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _postService.DeletePost(id);

            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }
    }
}
