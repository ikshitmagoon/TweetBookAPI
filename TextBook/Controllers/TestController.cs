using Microsoft.AspNetCore.Mvc;
using TweetBook.Contract;

namespace TweetBook.Controllers
{
    public class TestController:Controller
    {
        [HttpGet(ApiRoute.Posts.Get)]
        public IActionResult Get()
        {
            return Ok(new { name = "Ikshit" });
        }
    }
}
