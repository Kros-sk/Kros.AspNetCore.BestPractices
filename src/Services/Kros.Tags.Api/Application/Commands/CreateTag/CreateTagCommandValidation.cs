using FluentValidation;

namespace Kros.Tags.Api.Application.Commands
{
    /// <summary>
    /// Validator for <see cref="CreateTagCommand"/>
    /// </summary>
    public class CreateTagCommandValidation : AbstractValidator<CreateTagCommand>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public CreateTagCommandValidation()
        {
            RuleFor(t => t.Name).TagName();
            RuleFor(t => t.Description).TagDescription();
        }
    }
}
