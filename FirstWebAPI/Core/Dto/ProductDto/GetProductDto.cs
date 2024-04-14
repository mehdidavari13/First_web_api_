namespace FirstWebAPI.Core.Dto.ProductDto
{
    public class GetProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
