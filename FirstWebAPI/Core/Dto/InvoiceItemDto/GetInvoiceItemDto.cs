using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Dto.InvoiceItemDto
{
    public class GetInvoiceItemDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Invoice Invoice { get; set; }
        public Product Product { get; set; }
    }
}
