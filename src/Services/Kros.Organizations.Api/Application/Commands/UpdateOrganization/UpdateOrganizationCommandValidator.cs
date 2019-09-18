using FluentValidation;

namespace Kros.Organizations.Api.Application.Commands
{
    /// <summary>
    /// Validator for <see cref="UpdateOrganizationCommand"/>.
    /// </summary>
    public class UpdateOrganizationCommandValidator : AbstractValidator<UpdateOrganizationCommand>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public UpdateOrganizationCommandValidator()
        {
            RuleFor(o => o.OrganizationName).OrganizationName();
            RuleFor(o => o.BusinessId).OrganizationBusinessId();
        }
    }
}
