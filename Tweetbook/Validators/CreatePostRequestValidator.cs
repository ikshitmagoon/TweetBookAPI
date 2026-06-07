using FluentValidation;
using TweetBook.Contract.V1.Requests;
using TweetBook.Contract.V1.Responses;

namespace TweetBook.Validators
{
    public class CreatePostRequestValidator: AbstractValidator<CreatePostRequest>
    {
        public CreatePostRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Matches("^[a-zA-Z ]*$");
        }
    }
}
