using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FirstWebAPI.Core.Context;
using FirstWebAPI.Core.Contracts;
using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Repositories
{
    public class InvoiceItemRepository : GenericRepository<InvoiceItem>, IInvoiceItemRepository
    {
        
        public InvoiceItemRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context , contextAccessor)
        {
           
        }

        public  async Task<InvoiceItem> IsDuplicateItemProduct (InvoiceItem item)
        {
            var duplicate = await _context.Set<InvoiceItem>().FirstOrDefaultAsync(i => i.ProductId == item.ProductId && i.InvoiceId == item.InvoiceId);
            return duplicate;
        }
        public override async Task<List<InvoiceItem>> GetAllAsync()
        {
            
            return await _context.Set<InvoiceItem>()
                .Where(i => i.Invoice.User!.Id == GetUserId())
                .ToListAsync();
        }
        public override async Task<InvoiceItem> AddAsync(InvoiceItem newInvoiceItem)
        {
            
            
                await _context.Set<InvoiceItem>().AddAsync(newInvoiceItem);
                await _context.SaveChangesAsync();
           
            
                 return newInvoiceItem;
        }

        public override async Task<InvoiceItem> GetAsync(int? id)
        {
            if (id == null)
                return null;
            var entity = await _context.Set<InvoiceItem>().FirstOrDefaultAsync(i => i.Id == id && i.Invoice.User!.Id == GetUserId());
            return entity;
        }
        public override async Task UpdateAsync(InvoiceItem entity)
        {
            _context.Set<InvoiceItem>().Update(entity);
            await _context.SaveChangesAsync();
        }
        public override async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<InvoiceItem>().FirstOrDefaultAsync(i => i.Id == id && i.Invoice.User!.Id == GetUserId());
            _context.Set<InvoiceItem>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task<bool> Exists(int id)
        {
            var entity = await _context.Set<InvoiceItem>().FirstOrDefaultAsync(i => i.Id == id && i.Invoice.User!.Id == GetUserId());

            return entity != null;
        }
    }
}
