using ToDoProject.Application.Interfaces;
using ToDoProject.Core.Enums;
using ToDoProject.Core.Models;

namespace ToDoProject.Infrastructure.DAL
{
    public class ProductDAL : IProductDAL
    {
        private static readonly List<Product> Products =
        [
            new Product
            {
                Id = 1,
                Name = "Example Product",
                Price = 100,
                Status = ProductStatus.Active
            }
        ];

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Product>>(Products);
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            var product = Products.FirstOrDefault(x => x.Id == id);

            return Task.FromResult(product);
        }

        public Task<Product> CreateAsync(Product product)
        {
            product.Id = Products.Count == 0
                ? 1
                : Products.Max(x => x.Id) + 1;

            Products.Add(product);

            return Task.FromResult(product);
        }
    }
}