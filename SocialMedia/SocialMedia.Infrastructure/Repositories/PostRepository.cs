using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly SocialMediaContext context;

        public PostRepository(SocialMediaContext _context)
        {
            this.context = _context;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts = await context.Posts.ToListAsync();

            return posts;
        }
        public async Task<Post> GetPost(int id)
        {
            var posts = await context.Posts.FirstOrDefaultAsync(x => x.PostId == id);

            return posts;
        }

        public async Task InsertPost(Post post)
        {
            this.context.Posts.Add(post);
            await this.context.SaveChangesAsync();
        }
    }
}
