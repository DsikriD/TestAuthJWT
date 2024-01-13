using System.Collections.Generic;

namespace Resource.Api.Models
{
    public class ProductStore
    {
        public List<Product> Products => new List<Product>
        {
            new Product {Id=1,Name="Prod1",Price=11},
            new Product {Id=2,Name="Prod2",Price=22},
            new Product {Id=3,Name="Prod3",Price=33},
            new Product {Id=4,Name="Prod4",Price=44},
        };

        public Dictionary<Guid, int[]> Orders => new Dictionary<Guid, int[]>
        {
            {Guid.Parse("7f4565ad-a62c-4a4a-896c-27e007357156"), new int[] {1,2,3} },
            {Guid.Parse("43580a9d-e3b0-456d-9721-01a1bbc7226b"),new int[] {1,3} }
        };
    }
}
