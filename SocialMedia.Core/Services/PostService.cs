﻿using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;

namespace SocialMedia.Core.Services
{
    public class PostService(IUnitOfWork unitOfWork) : IPostService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public IEnumerable<Post> GetPosts(PostQueryFilter postQueryFilter)
        {
            var posts = _unitOfWork.PostRepository.GetAll();

            if (postQueryFilter.userId != null)
            {
                posts = posts.Where(x => x.UserId == postQueryFilter.userId);
            }

            if (postQueryFilter.date != null)
            {
                posts = posts
                    .Where(x => 
                           x.Date.ToShortDateString() == 
                           postQueryFilter.date?.ToShortDateString());
            }
            if (postQueryFilter.description != null)
            {
                posts = posts
                    .Where(x => 
                            x.Description.ToLower()
                            .Contains(postQueryFilter.description));
            }
            return posts;
        }

        public async Task<Post> GetPost(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public async Task InsertPost(Post post)
        {
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);

            if (user == null)
            {
                throw new BusinessException("User does'nt exist");
            }

            var userPost = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId);

            if (userPost.Count() < 10)
            {
                var lastPost = userPost.OrderByDescending(x => x.Date).FirstOrDefault();
                if ((DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BusinessException("You are not able to publish");
                }
            }

            if (post.Description.Contains("Sexo") == true)
            {
                throw new BusinessException("Content not alowed");
            }

            await _unitOfWork.PostRepository.Add(post);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdatePost(Post post)
        {
            _unitOfWork.PostRepository.Update(post);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}