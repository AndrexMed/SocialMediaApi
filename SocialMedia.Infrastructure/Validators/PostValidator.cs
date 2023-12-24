using FluentValidation;
using SocialMedia.Core.DTOs;

namespace SocialMedia.Infrastructure.Validators
{
    public class PostValidator : AbstractValidator<PostDTO>
    {
        public PostValidator()
        {
            RuleFor(post => post.Description)
                .NotNull()
                .Length(10, 100);

            //RuleFor(post => post.Date)
            //    .NotNull()
            //    .LessThan(DateTime.Now);

            RuleFor(post => post.Title)
                .NotNull()
                .Length(10, 60);
        }
    }
}
