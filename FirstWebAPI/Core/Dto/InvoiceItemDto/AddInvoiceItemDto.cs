using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Dto.InvoiceItemDto
{
    public class AddInvoiceItemDto
    {
        
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
