using FluentValidation;

namespace Kros.ToDos.Api.Application.Commands
{
    /// <summary>
    /// Validator for <see cref="CreateToDoCommand"/>.
    /// </summary>
    public class CreateToDoCommandValidator : AbstractValidator<CreateToDoCommand>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public CreateToDoCommandValidator()
        {
            RuleFor(x => x.Description).MaximumLength(255).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(50).NotEmpty();
        }
    }
}
