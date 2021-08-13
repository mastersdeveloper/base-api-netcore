using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork unitOfWork;

        public PostService(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }

        public async Task<bool> DeletePost(int id)
        {
            await this.unitOfWork.PostRepository.Delete(id);
            await this.unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Post> GetPost(int id)
        {
            return await this.unitOfWork.PostRepository.GetById(id);
        }

        public IEnumerable<Post> GetPosts(PostQueryFilter filters)
        {
            var posts = this.unitOfWork.PostRepository.GetAll();

            if (filters.UserId != null)
            {
                posts = posts.Where(x => x.UserId == filters.UserId);
            }

            if (filters.Date != null)
            {
                posts = posts.Where(x => x.Date.ToShortDateString() == filters.Date?.ToShortDateString());
            }

            if (filters.Descripcion != null)
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(filters.Descripcion.ToLower()));
            }

            return posts;
        }

        public async Task InsertPost(Post post)
        {
            var user = await this.unitOfWork.UserRepository.GetById(post.UserId);

            if (user == null)
            {
                throw new BusinessException("User doesn't exist");
            }

            var userPost = await this.unitOfWork.PostRepository.GetPostsByUser(post.UserId);

            if (userPost.Count() < 10)
            {
                var lastPost = userPost.LastOrDefault();

                if ((DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BusinessException("You are not able to publish the post");
                }
            }

            if (post.Description.Contains("Sexo"))
            {
                throw new BusinessException("Content not allowed");
            }

            await this.unitOfWork.PostRepository.Add(post);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdatePost(Post post)
        {
            this.unitOfWork.PostRepository.Update(post);
            await this.unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
