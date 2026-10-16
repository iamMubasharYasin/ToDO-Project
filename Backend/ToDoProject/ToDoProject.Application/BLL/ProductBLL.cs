using ToDoProject.Application.Interfaces;
using ToDoProject.Core.Models;

namespace ToDoProject.Application.BLL
{
    public class ProductBLL : IProductBLL
    {
        private readonly IProductDAL _productDAL;
        private readonly IEmailService _emailService;

        public ProductBLL(
            IProductDAL productDAL,
            IEmailService emailService)
        {
            _productDAL = productDAL;
            _emailService = emailService;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productDAL.GetAllAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _productDAL.GetByIdAsync(id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product name is required.");
            }

            if (product.Price < 0)
            {
                throw new ArgumentException("Product price cannot be negative.");
            }

            var createdProduct = await _productDAL.CreateAsync(product);

            await _emailService.SendEmailAsync(
                "admin@example.com",
                "Product Created",
                $"Product '{createdProduct.Name}' was created successfully.");

            return createdProduct;
        }
    }
}