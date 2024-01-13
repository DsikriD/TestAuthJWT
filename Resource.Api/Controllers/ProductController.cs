using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resource.Api.Models;

namespace Resource.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductStore store;

        public ProductController(ProductStore store)
        {
            this.store = store;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetProduct()
        {
            return Ok(store.Products);
        }
    }
}
