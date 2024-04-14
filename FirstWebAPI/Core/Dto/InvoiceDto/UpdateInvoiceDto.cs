using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Dto.InvoiceDto
{
    public class UpdateInvoiceDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; } = 0;
        public List<InvoiceItem>? InvoiceItems { get; set; }

    }
}
