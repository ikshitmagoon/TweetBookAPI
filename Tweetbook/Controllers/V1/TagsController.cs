using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contract.V1;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : Controller
    {
        private readonly IPostService _postService;

        public TagsController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet(ApiRoute.Tags.GetAll)]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetTagsAsync());
        }
        [HttpGet(ApiRoute.Tags.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {

            var tag = await _postService.GetTagByIdAsync(postId);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }
    }
}
