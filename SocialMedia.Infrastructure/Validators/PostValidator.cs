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
                .WithMessage("La descripcion es requerida.");

            RuleFor(post => post.Description)
                .Length(10, 100)
                .WithMessage("La descripcion solo acepta caracteres entre 10 y 100");
            //RuleFor(post => post.Date)
            //    .NotNull()
            //    .LessThan(DateTime.Now);

            RuleFor(post => post.Title)
                .NotNull()
                .Length(10, 60)
                .WithMessage("El titlo no puede ser nula");
        }
    }
}
