
namespace SocialMedia.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SocialMedia.Core.Entities;
    using SocialMedia.Core.Interfaces;
    using SocialMedia.Infrastructure.Data;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UserRepository : IUserRepository
    {
        private readonly SocialMediaContext context;

        public UserRepository(SocialMediaContext _context)
        {
            this.context = _context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await context.Users.ToListAsync();

            return users;
        }
        public async Task<User> GetUser(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            return user;
        }
    }
}
