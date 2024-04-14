namespace FirstWebAPI.Core.Dto.ProductDto
{
    public class AddProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}
