namespace FirstWebAPI.Core.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; } = 0;
        public User? User { get; set; }
        public List<InvoiceItem>? InvoiceItems { get; set; }
    }
}
