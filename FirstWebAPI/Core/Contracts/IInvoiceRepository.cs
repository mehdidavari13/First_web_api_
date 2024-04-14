using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Contracts
{
    public interface IInvoiceRepository:IGenericRepository<Invoice>
    {
        Task UpdateTotalAmount(int id);
    }
}
