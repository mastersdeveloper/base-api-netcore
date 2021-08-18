using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Response;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    /*
     Controller:
        - ControllerBase: creado para trabajar con API
        - Controller: contiene lo mismo que ControllerBase pero agregar los fixtures para trabajar con MVC
     
     ApiController: nos permite trabajar con API RestFul, agrega una cantidad de fixtures que nos ayuda para trabajar,
                    tales fixtures como Autentication
     */

    [Authorize]
    [Produces("application/json")]//Indicando que solo se usa json al momento de devolver la data
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly IMapper mapper;
        private readonly IUriService uriService;

        public PostController(IPostService _postService, IMapper _mapper, IUriService _uriService)
        {
            this.postService = _postService;
            this.mapper = _mapper;
            this.uriService = _uriService;
        }

        /// <summary>
        /// Retrieve all posts
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetPosts))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<PostDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetPosts([FromQuery] PostQueryFilter filters)
        {
            var posts = this.postService.GetPosts(filters);

            var postsDto = this.mapper.Map<IEnumerable<PostDto>>(posts);

            var metadata = new Metadata
            {
                TotalCount = posts.TotalCount,
                PageSize = posts.PageSize,
                CurrentPage = posts.CurrentPage,
                TotalPages = posts.TotalPages,
                HasNextPage = posts.HasNextPage,
                HasPreviousPage = posts.HasPreviousPage,
                NextPageUrl = this.uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString(),
                PreviousPageUrl = this.uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString()
            };

            var response = new ApiResponse<IEnumerable<PostDto>>(postsDto)
            {
                Meta = metadata
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await this.postService.GetPost(id);

            var postDto = this.mapper.Map<PostDto>(post);
            var response = new ApiResponse<PostDto>(postDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PostDto postDto)
        {
            var post = this.mapper.Map<Post>(postDto);
            await this.postService.InsertPost(post);

            postDto = this.mapper.Map<PostDto>(post);
            var response = new ApiResponse<PostDto>(postDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, PostDto postDto)
        {
            var post = this.mapper.Map<Post>(postDto);
            post.Id = id;

            var result = await this.postService.UpdatePost(post);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.postService.DeletePost(id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }
    }
}
