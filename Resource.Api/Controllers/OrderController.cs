using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Resource.Api.Models;
using System.Security.Claims;

namespace Resource.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ProductStore store;

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public OrderController(ProductStore store)
        {
            this.store = store;
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("")]
        public IActionResult GetOrder()
        {
            if (!store.Orders.ContainsKey(UserId)) return Ok(Enumerable.Empty<Product>());

            var ProdId = store.Orders.Single(x=>x.Key == UserId).Value;
            var orderBooks = store.Products.Where(x => ProdId.Contains(x.Id));

            return Ok(orderBooks);
        }

    }
}
