using FluentValidation;

namespace Kros.ToDos.Api.Application.Commands
{
    /// <summary>
    /// Validator for <see cref="UpdateToDoCommand"/>.
    /// </summary>
    public class UpdateToDoCommandValidator : AbstractValidator<UpdateToDoCommand>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public UpdateToDoCommandValidator()
        {
            RuleFor(x => x.Description).DescriptionValidation();
            RuleFor(x => x.Name).NameValidation();
        }
    }
}
