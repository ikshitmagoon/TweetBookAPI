using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TweetBook.Contract.V1.Requests;
using TweetBook.Data;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class PostService: IPostService
    {

        //private readonly List<Post> _posts;
        //public PostService() {
        //    _posts = new List<Post>();

        //    for (var i = 0; i < 5; i++)
        //    {
        //        _posts.Add(new Post
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = $"post Name {i}"

        //        });
        //    }
        //}
        private readonly DataContext _dataContext;
        public PostService(DataContext dataContext)
        {
          _dataContext = dataContext;
        }

        public async  Task<List<Post>> GetPostsAsync()
        {
            return await _dataContext.Posts.ToListAsync();
        }

       public async Task<Post> GetPostByIdAsync(Guid id)
        {
           return await _dataContext.Posts.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> CreatePostAsync(Post postrequest)
        {
            _dataContext.Posts.Add(postrequest);

           var create= await _dataContext.SaveChangesAsync();
            return create>0 ;
        }
        public async Task<bool> UpdatePostAsync(Post post)
        {
            //var exist = GetPostById(post.Id) != null;
            //if (!exist)
            //{
            //    return false;
            //}

            //var index = _dataContext.Posts.FindIndexAsync(x => x.Id == post.Id);

            //_posts[index] = post;
            //return true;

             _dataContext.Posts.Update(post);

            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0 ;
        }
        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var exist = await GetPostByIdAsync(postId);
            if (exist==null)
            {
                return false;
            }

            _dataContext.Posts.Remove(exist);

            var deleted = await _dataContext.SaveChangesAsync();
            return deleted>0;
        }
       public async Task<bool> UserOwnsPostAsync(Guid postId, string getUserId)
        {
            var post= await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x=>x.Id == postId);
            if (post == null)
            {
                return false;
            }
            if(post.userId == getUserId)
            {
                return true;
            }
            return false;
        }
    }
}
