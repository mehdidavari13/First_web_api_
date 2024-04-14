using FirstWebAPI.Core.Dto.InvoiceItemDto;
namespace FirstWebAPI.Core.Dto.InvoiceDto
{
    public class InvoiceDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public List<ItemDto>? Items { get; set; }

    }
}
