using ToDoProject.Application.Interfaces;
using ToDoProject.Core.Models;

namespace ToDoProject.API.Endpoints
{
    public static class ProductEndpoints
    {
        public static IEndpointRouteBuilder MapProductEndpoints(
            this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/products")
                .WithTags("Products");

            group.MapGet("/", GetAllProductsAsync);

            group.MapGet("/{id:int}", GetProductByIdAsync);

            group.MapPost("/", CreateProductAsync);

            return endpoints;
        }

        private static async Task<IResult> GetAllProductsAsync(
            IProductBLL productBLL)
        {
            var products = await productBLL.GetAllAsync();

            return Results.Ok(products);
        }

        private static async Task<IResult> GetProductByIdAsync(
            int id,
            IProductBLL productBLL)
        {
            var product = await productBLL.GetByIdAsync(id);

            if (product is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(product);
        }

        private static async Task<IResult> CreateProductAsync(
            Product product,
            IProductBLL productBLL)
        {
            var createdProduct = await productBLL.CreateAsync(product);

            return Results.Created(
                $"/api/products/{createdProduct.Id}",
                createdProduct);
        }
    }
}