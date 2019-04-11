using FluentValidation;

namespace Kros.ToDos.Api.Application.Commands
{
    /// <summary>
    /// Base validation rules.
    /// </summary>
    public static class ValidationRules
    {
        /// <summary>
        /// Validation rule for Description.
        /// </summary>
        /// <typeparam name="T">Command type.</typeparam>
        /// <param name="rule">Rule</param>
        /// <returns>Validation rule</returns>
        public static IRuleBuilderOptions<T, string> DescriptionValidation<T>(this IRuleBuilder<T, string> rule)
            => rule
            .NotEmpty()
            .NotNull()
            .MaximumLength(255);

        /// <summary>
        /// Validation rule for Name.
        /// </summary>
        /// <typeparam name="T">Command type.</typeparam>
        /// <param name="rule">Rule</param>
        /// <returns>Validation rule</returns>
        public static IRuleBuilderOptions<T, string> NameValidation<T>(this IRuleBuilder<T, string> rule)
            => rule
            .NotEmpty()
            .NotNull()
            .MaximumLength(50);
    }
}