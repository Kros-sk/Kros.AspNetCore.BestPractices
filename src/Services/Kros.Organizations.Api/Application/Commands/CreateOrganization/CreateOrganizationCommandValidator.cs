using FluentValidation;

namespace Kros.Organizations.Api.Application.Commands
{
    /// <summary>
    /// Validator for <see cref="CreateOrganizationCommand"/>.
    /// </summary>
    public class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public CreateOrganizationCommandValidator()
        {
            RuleFor(o => o.OrganizationName).OrganizationName();
            RuleFor(o => o.BusinessId).OrganizationBusinessId();
        }
    }
}
