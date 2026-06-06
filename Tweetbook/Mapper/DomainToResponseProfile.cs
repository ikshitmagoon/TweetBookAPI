using AutoMapper;
using TweetBook.Contract.V1.Responses;

namespace TweetBook.Mapper
{
    public class DomainToResponseProfile:Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Domain.Post, CreateResponse>().ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => new TagResponse { Name=x.Name,CreatedBy= x.CreatedBy,CreaterId= x.CreaterId,CreatedOn= x.CreatedOn,PostId= x.PostId })));
            CreateMap<Domain.Tags, TagResponse>();
        }
    }
}
