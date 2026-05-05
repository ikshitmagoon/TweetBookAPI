using Microsoft.AspNetCore.Mvc;
using TweetBook.Contract.V1;
using TweetBook.Contract.V1.Requests;
using TweetBook.Contract.V1.Responses;
using TweetBook.Domain;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    public class PostsController:Controller
    {

       public readonly IPostService _PostService;


        public PostsController(IPostService postService)
        {
           _PostService = postService;
        }
        [HttpGet(ApiRoute.Posts.GetAll)]
            public async Task<IActionResult> GetAll()
        {

            return Ok(await _PostService.GetPostsAsync());
        }
        [HttpGet(ApiRoute.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {

            var post =await _PostService.GetPostByIdAsync(postId);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpPost(ApiRoute.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest) {
         

            var posts = new Post { Name = postRequest.Name };
             await _PostService.CreatePostAsync(posts);

            var BaseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl=BaseUrl+"/"+ApiRoute.Posts.Get.Replace("{postId}",posts.Id.ToString());

            var response = new CreateResponse {Id = posts.Id };

            return Created(locationUrl,response);
        }

        [HttpPut(ApiRoute.Posts.Update)]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid postId, [FromBody] UpdatePostRequest postRequest)
        {
            var post = new Post
            {
                Id = postId,
                Name = postRequest.Name
            };

            var updated = await _PostService.UpdatePostAsync(post);

            if (!updated)
            {
                return NotFound();
            }

            return Ok(post);

        }
        [HttpDelete(ApiRoute.Posts.Delete)]
        public async Task<IActionResult> DeletePost([FromRoute] Guid postId)
        {
          
            var updated = await _PostService.DeletePostAsync(postId);

            if (!updated)
            {
                return NotFound();
            }

            return Ok();

        }

    }
}
