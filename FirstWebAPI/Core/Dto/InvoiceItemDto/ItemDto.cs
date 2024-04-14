namespace FirstWebAPI.Core.Dto.InvoiceItemDto
{
    public class ItemDto
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
