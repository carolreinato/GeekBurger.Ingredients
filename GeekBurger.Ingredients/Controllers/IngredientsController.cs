using GeekBurger.Ingredients.Contract.DTO;
using GeekBurger.Ingredients.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Controllers
{
    [Route("api/products")]
    public class IngredientsController : Controller
    {
        [HttpGet]
        [Route("byrestrictions")]
        public IActionResult GetProductsByRestrictions([FromServices] IIngredientsService service, [FromQuery] IngredientsRequest request)
        {
            var response = service.GetProductsByRestrictions(request).Result;
            if (response != null)
                return Ok(response);
            return NotFound("Nenhum produto encontrado");
        }
    }
}
