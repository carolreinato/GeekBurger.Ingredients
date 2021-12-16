using FluentValidation.Results;
using GeekBurger.Ingredients.Contract.DTO;

namespace GeekBurger.Ingredients.Validations
{
    public interface IIngredientsRequestValidator
    {
        ValidationResult Validate(IngredientsRequest request);
    }
}
