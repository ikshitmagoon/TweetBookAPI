//using Cosmonaut;
//using Cosmonaut.Extensions;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class CosmoPostService/* : IPostService*/
    {
       // private readonly ICosmosStore<CosmosPostDTO> _cosmosStore;

       // public CosmoPostService(ICosmosStore<CosmosPostDTO> cosmosStore)
       // {
       //     _cosmosStore = cosmosStore;
       // }
       //public async  Task<bool> CreatePostAsync(Post post)
       // {
       //     var cosmoPost = new CosmosPostDTO
       //     {
       //         Id = Guid.NewGuid().ToString(),
       //         Name = post.Name
       //     };
       //   var response=  await _cosmosStore.AddAsync(cosmoPost);
       //     post.Id= Guid.Parse(cosmoPost.Id); 
       //     return response.IsSuccess;
       // }

       // public async Task<bool> DeletePostAsync(Guid id)
       // {
       //    var response =await _cosmosStore.RemoveByIdAsync(id.ToString());
       //     return response.IsSuccess;
       // }

       // public async Task<Post> GetPostByIdAsync(Guid id)
       // {
       //    return await _cosmosStore.FindAsync(id.ToString()) is CosmosPostDTO cosmoPost ? new Post
       //     {
       //         Id = Guid.Parse(cosmoPost.Id),
       //         Name = cosmoPost.Name
       //     } : null;
       // }

       // public async Task<List<Post>> GetPostsAsync()
       // {
       //    var posts= await _cosmosStore.Query().ToListAsync();
       //     return posts.Select(p => new Post
       //     {
       //         Id = Guid.Parse(p.Id),
       //         Name = p.Name
       //     }).ToList();
       // }

       // public async Task<bool> UpdatePostAsync(Post post)
       // {
       //    var cosmoPost = new CosmosPostDTO
       //     {
       //         Id = post.Id.ToString(),
       //         Name = post.Name
       //     };
       //     var response = await _cosmosStore.UpdateAsync(cosmoPost);
       //     return response.IsSuccess;
       // }
    }
}
