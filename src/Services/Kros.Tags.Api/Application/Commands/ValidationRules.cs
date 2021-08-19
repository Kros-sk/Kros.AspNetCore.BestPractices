using FluentValidation;

namespace Kros.Tags.Api.Application.Commands
{
    /// <summary>
    /// Validation rules.
    /// </summary>
    public static class ValidationRules
    {

        /// <summary>
        /// Validation rules for name.
        /// </summary>
        /// <typeparam name="T">Command type.</typeparam>
        /// <param name="rule">Rule.</param>
        /// <returns>Validation rule.</returns>
        public static IRuleBuilder<T, string> TagName<T>(this IRuleBuilder<T, string> rule)
            => rule
            .NotEmpty()
            .NotNull()
            .MaximumLength(25);

        /// <summary>
        /// Validation for description.
        /// </summary>
        /// <typeparam name="T">Command type.</typeparam>
        /// <param name="rule">Rule.</param>
        /// <returns>Validation rule.</returns>
        public static IRuleBuilder<T, string> TagDescription<T>(this IRuleBuilder<T, string> rule)
            => rule.MaximumLength(40);
    }
}
