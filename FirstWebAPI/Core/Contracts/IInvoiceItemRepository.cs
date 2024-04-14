using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Contracts
{
    public interface IInvoiceItemRepository:IGenericRepository<InvoiceItem>

    {
        Task<InvoiceItem> IsDuplicateItemProduct(InvoiceItem item);
    }
    
}
