using FluentValidation;

namespace BMedia.Application.Features.Contents.Commands.CreateContent;

public class CreateContentCommandValidator : AbstractValidator<CreateContentCommand>
{
    public CreateContentCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(500);
        RuleFor(x => x.Summary).MaximumLength(2000).When(x => x.Summary is not null);
        RuleFor(x => x.Language).NotEmpty().MaximumLength(10);
        RuleFor(x => x.SeoTitle).MaximumLength(300).When(x => x.SeoTitle is not null);
        RuleFor(x => x.SeoDescription).MaximumLength(500).When(x => x.SeoDescription is not null);
        RuleFor(x => x.SeoKeywords).MaximumLength(500).When(x => x.SeoKeywords is not null);
        RuleFor(x => x.ScheduledPublishAt).GreaterThan(DateTime.UtcNow).When(x => x.ScheduledPublishAt.HasValue);
    }
}
