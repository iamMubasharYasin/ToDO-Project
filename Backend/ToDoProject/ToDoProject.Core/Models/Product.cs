using ToDoProject.Core.Enums;

namespace ToDoProject.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public ProductStatus Status { get; set; }
    }
}