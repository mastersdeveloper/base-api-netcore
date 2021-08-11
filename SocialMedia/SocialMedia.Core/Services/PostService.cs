using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;

        public PostService(IPostRepository _postRepository, IUserRepository _userRepository)
        {
            this.postRepository = _postRepository;
            this.userRepository = _userRepository;
        }

        public async Task<bool> DeletePost(int id)
        {
            return await this.postRepository.DeletePost(id);
        }

        public async Task<Post> GetPost(int id)
        {
            return await this.postRepository.GetPost(id);
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await this.postRepository.GetPosts();
        }

        public async Task InsertPost(Post post)
        {
            var user = await this.userRepository.GetUser(post.UserId);

            if (user == null)
            {
                throw new Exception("User doesn't exist");
            }

            if (post.Description.Contains("Sexo"))
            {
                throw new Exception("Content not allowed");
            }

            await this.postRepository.InsertPost(post);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            return await this.postRepository.UpdatePost(post);
        }
    }
}
