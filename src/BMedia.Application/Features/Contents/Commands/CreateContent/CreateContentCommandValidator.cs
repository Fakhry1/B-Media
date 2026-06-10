using FluentValidation;

namespace BMedia.Application.Features.Contents.Commands.CreateContent;

public class CreateContentCommandValidator : AbstractValidator<CreateContentCommand>
{
    public CreateContentCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(500);
        RuleFor(x => x.Summary).MaximumLength(2000).When(x => x.Summary is not null);
        RuleFor(x => x.Language).NotEmpty().MaximumLength(10);
        RuleFor(x => x.ScheduledPublishAt).GreaterThan(DateTime.UtcNow).When(x => x.ScheduledPublishAt.HasValue);
    }
}
