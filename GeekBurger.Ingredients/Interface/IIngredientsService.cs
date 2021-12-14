using GeekBurger.Ingredients.Contract.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Interface
{
    public interface IIngredientsService
    {
        Task<List<IngredientsResponse>> GetProductsByRestrictions(IngredientsRequest request);
    }
}
