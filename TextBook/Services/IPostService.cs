using TweetBook.Contract.V1.Requests;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IPostService
    {
        List<Post> GetPosts();

        Post GetPostById(Guid id);

        Post CreatePost(CreatePostRequest post);
        bool UpdatePost(Post post);
        bool DeletePost(Guid id);

    }
}
