using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contract.V1;
using TweetBook.Contract.V1.Requests;
using TweetBook.Contract.V1.Responses;
using TweetBook.Domain;
using TweetBook.Entensions;
using TweetBook.Services;
using static TweetBook.Contract.V1.ApiRoute;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController:Controller
    {

       public readonly IPostService _PostService;
        private readonly IMapper _mapper;

        public PostsController(IPostService postService, IMapper mapper)
        {
           _PostService = postService;
            _mapper = mapper;
        }
        [HttpGet(ApiRoute.Posts.GetAll)]
            public async Task<IActionResult> GetAll()
        {
            var posts=await _PostService.GetPostsAsync();
            var postReponses = _mapper.Map<List<CreateResponse>>(posts);
            return Ok(postReponses);
        }
        [HttpGet(ApiRoute.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {

            var post =await _PostService.GetPostByIdAsync(postId);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CreateResponse>(post));
        }
        [HttpPost(ApiRoute.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Name = postRequest.Name,
                userId= HttpContext.GetUserId()
            };

            // Add multiple tags manually from request
            foreach (var tagName in postRequest.Tags)
            {
                post.Tags.Add(new TweetBook.Domain.Tags
                {
                    CreaterId = Guid.NewGuid(),
                    PostId = post.Id,
                    Name = tagName,
                    CreatedBy = HttpContext.GetUserId(),
                    CreatedOn = DateTime.UtcNow
                });
            }

            await _PostService.CreatePostAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoute.Posts.Get.Replace("{postId}", post.Id.ToString());

            var response = _mapper.Map<CreateResponse>(post);

            return Created(locationUrl, response);
        }


        [HttpPut(ApiRoute.Posts.Update)]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid postId, [FromBody] UpdatePostRequest postRequest)
        {
            var userOwnsPost= await _PostService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post" });
            }

            var post=await _PostService.GetPostByIdAsync(postId);
            post.Name = postRequest.Name;
          
            var updated = await _PostService.UpdatePostAsync(post);

            if (!updated)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CreateResponse>(post));

        }
        [HttpDelete(ApiRoute.Posts.Delete)]
        public async Task<IActionResult> DeletePost([FromRoute] Guid postId)
        {
            var userOwnsPost = await _PostService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post" });
            }

           
            var updated = await _PostService.DeletePostAsync(postId);

            if (!updated)
            {
                return NotFound();
            }

            return Ok();

        }

    }
}
