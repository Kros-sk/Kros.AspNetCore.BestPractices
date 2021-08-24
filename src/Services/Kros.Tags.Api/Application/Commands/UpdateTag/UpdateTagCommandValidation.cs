using FluentValidation;

namespace Kros.Tags.Api.Application.Commands
{
    /// <summary>
    /// Validator for <see cref="UpdateTagCommand"/>.
    /// </summary>
    public class UpdateTagCommandValidation : AbstractValidator<UpdateTagCommand>
    {

        /// <summary>
        /// Ctor.
        /// </summary>
        public UpdateTagCommandValidation()
        {
            RuleFor(t => t.Name).TagName();
            RuleFor(t => t.Description).TagDescription();
        }
    }
}
