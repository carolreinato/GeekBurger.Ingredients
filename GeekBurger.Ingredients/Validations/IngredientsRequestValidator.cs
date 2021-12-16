using FluentValidation;
using GeekBurger.Ingredients.Contract.DTO;

namespace GeekBurger.Ingredients.Validations
{
    public class IngredientsRequestValidator : AbstractValidator<IngredientsRequest>, IIngredientsRequestValidator
    {
        public IngredientsRequestValidator()
        {
            RuleFor(p => p.Restrictions).NotEmpty().NotNull();
            RuleFor(p => p.StoreName).NotEmpty().NotNull();
        }
    }
}
