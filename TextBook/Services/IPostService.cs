using TweetBook.Contract.V1.Requests;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IPostService
    { 
        Task<List<Post>> GetPostsAsync();

        Task<Post> GetPostByIdAsync(Guid id);

        Task<bool> CreatePostAsync(Post post);
        Task<bool> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(Guid id);

    }
}
