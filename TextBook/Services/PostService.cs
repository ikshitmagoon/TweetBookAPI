using Microsoft.AspNetCore.Http.HttpResults;
using TweetBook.Contract.V1.Requests;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class PostService: IPostService
    {

        private readonly List<Post> _posts;
        public PostService() {
            _posts = new List<Post>();

            for (var i = 0; i < 5; i++)
            {
                _posts.Add(new Post
                {
                    Id = Guid.NewGuid(),
                    Name = $"post Name {i}"

                });
            }
        }


        public List<Post> GetPosts()
        {
            return _posts;
        }

       public Post GetPostById(Guid id)
        {
           return _posts.SingleOrDefault(x => x.Id == id);
        }

        public Post CreatePost(CreatePostRequest postrequest)
        {
           var post= new Post { Id = postrequest.Id };
            if (post.Id != Guid.Empty)
            {
                post.Id = Guid.NewGuid();
            }
            return post;
        }
        public bool UpdatePost(Post post)
        {
            var exist = GetPostById(post.Id) != null;
            if (!exist)
            {
                return false;
            }

            var index = _posts.FindIndex(x => x.Id == post.Id);

            _posts[index] = post;
            return true;
        }
        public bool DeletePost(Guid postId)
        {
            var exist = GetPostById(postId);
            if (exist==null)
            {
                return false;
            }

            _posts.Remove(exist);
            return true;
        }
    }
}
