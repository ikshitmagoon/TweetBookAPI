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
            public IActionResult GetAll()
        {

            return Ok(_PostService.GetPosts());
        }
        [HttpGet(ApiRoute.Posts.Get)]
        public IActionResult Get([FromRoute] Guid postId)
        {

            var post = _PostService.GetPostById(postId);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpPost(ApiRoute.Posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest postRequest) {
            Post posts = new Post();
             posts= _PostService.CreatePost(postRequest);
         
            _PostService.GetPosts().Add(posts);
            var BaseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl=BaseUrl+"/"+ApiRoute.Posts.Get.Replace("{postId}",posts.Id.ToString());

            var response = new CreateResponse {Id = posts.Id };

            return Created(locationUrl,response);
        }

        [HttpPut(ApiRoute.Posts.Update)]
        public IActionResult UpdatePost([FromRoute] Guid postId, [FromBody] UpdatePostRequest postRequest)
        {
            var post = new Post
            {
                Id = postId,
                Name = postRequest.Name
            };

            var updated = _PostService.UpdatePost(post);

            if (!updated)
            {
                return NotFound();
            }

            return Ok(post);

        }
        [HttpDelete(ApiRoute.Posts.Delete)]
        public IActionResult DeletePost([FromRoute] Guid postId)
        {
          
            var updated = _PostService.DeletePost(postId);

            if (!updated)
            {
                return NotFound();
            }

            return Ok();

        }

    }
}
