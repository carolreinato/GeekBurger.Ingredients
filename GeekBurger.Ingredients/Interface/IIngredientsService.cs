using GeekBurger.Ingredients.Contract.DTO;
using Microsoft.Azure.ServiceBus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Interface
{
    public interface IIngredientsService
    {
        Task<List<IngredientsResponse>> GetProductsByRestrictions(IngredientsRequest request);
        Task MergeProductAndIngredients(Message message);
    }
}
