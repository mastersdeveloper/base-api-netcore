using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository postRepository;

        public PostController(IPostRepository _postRepository)
        {
            this.postRepository = _postRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await this.postRepository.GetPosts();

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await this.postRepository.GetPost(id);

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Post post)
        {
            await this.postRepository.InsertPost(post);

            return Ok(post);
        }
    }
}
