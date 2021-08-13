using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SocialMediaContext context;

        private readonly IPostRepository postRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Comment> commentRepository;

        public UnitOfWork(SocialMediaContext _context)
        {
            this.context = _context;
        }

        public IPostRepository PostRepository => this.postRepository ?? new PostRepository(this.context);
        public IRepository<User> UserRepository => this.userRepository ?? new BaseRepository<User>(this.context);
        public IRepository<Comment> CommentRepository => this.commentRepository ?? new BaseRepository<Comment>(this.context);

        public void Dispose()
        {
            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
