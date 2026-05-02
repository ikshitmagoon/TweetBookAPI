using Microsoft.AspNetCore.Mvc;
using TweetBook.Contract;
using TweetBook.Domain;

namespace TweetBook.Controllers.V1
{
    public class PostsController:Controller
    {

        private List<Post> _posts;


        public PostsController()
        {
            _posts = new List<Post>();

            for(var i = 0; i < 5; i++)
            {
                _posts.Add(new Post
                {
                    id = Guid.NewGuid().ToString()

                });
            }
        }
        [HttpGet(ApiRoute.Posts.GetAll)]
            public IActionResult GetAll()
        {

            return Ok(_posts);
        }
    }
}
