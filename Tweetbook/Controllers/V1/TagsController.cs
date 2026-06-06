using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contract.V1;
using TweetBook.Contract.V1.Responses;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public TagsController(IPostService postService,IMapper mapper)
        {
            _postService = postService;
            _mapper=mapper;
        }
        [HttpGet(ApiRoute.Tags.GetAll)]
        //[Authorize(Roles ="Admin")]
        [Authorize(Policy = "mustworkforOptum")]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _postService.GetTagsAsync();
            var tagsResponses = _mapper.Map<List<TagResponse>>(tags);
            return Ok(tagsResponses);
        }
        [HttpGet(ApiRoute.Tags.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {

            var tag = await _postService.GetTagByIdAsync(postId);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<TagResponse>(tag));
        }
    }
}
