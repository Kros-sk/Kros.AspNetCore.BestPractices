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
            RuleFor(x => x.Description).MaximumLength(255).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(50).NotEmpty();
        }
    }
}
