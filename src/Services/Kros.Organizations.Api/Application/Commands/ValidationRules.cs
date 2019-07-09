using FluentValidation;

namespace Kros.Organizations.Api.Application.Commands
{
    /// <summary>
    /// Base validation rules for Organization Dtos.
    /// </summary>
    public static class ValidationRules
    {

        /// <summary>
        /// Base validation rules for Organization Dtos name
        /// </summary>
        /// <typeparam name="T">Organization Dtos name property</typeparam>
        /// <param name="rule">Rule</param>
        /// <returns></returns>
        public static IRuleBuilder<T, string> OrganizationName<T>(this IRuleBuilder<T, string> rule) =>
            rule.NotEmpty()
                .NotNull()
                .MaximumLength(50);

        /// <summary>
        /// Base validation rules for Organization Dtos Business Id
        /// </summary>
        /// <typeparam name="T">Organization Dtos business id property</typeparam>
        /// <param name="rule">Rule</param>
        /// <returns></returns>
        public static IRuleBuilder<T, string> OrganizationBusinessId<T>(this IRuleBuilder<T, string> rule) =>
            rule.MaximumLength(15);

    }
}
