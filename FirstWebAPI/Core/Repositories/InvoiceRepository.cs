using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FirstWebAPI.Core.Context;
using FirstWebAPI.Core.Contracts;
using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }

        public override async Task<Invoice> AddAsync(Invoice newInvoice)
        {

            newInvoice.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            
            await _context.Set<Invoice>().AddAsync(newInvoice);
            await _context.SaveChangesAsync();
            return newInvoice;
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<Invoice>().FirstOrDefaultAsync(i => i.Id == id && i.User!.Id == GetUserId());
            _context.Set<Invoice>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task<bool> Exists(int id)
        {
            var entity = await _context.Set<Invoice>().FirstOrDefaultAsync(i => i.Id == id && i.User!.Id == GetUserId());

            return entity != null;
        }
        
        public override async Task<Invoice> GetAsync(int? id)
        {
            if (id == null)
                return null;
            //var result = from Invoice in _context.Set<Invoice>()
            //             join user in _context.Users on Invoice.User.Id =

            var entity = await _context.Set<Invoice>().Include(i => i.User)
                .Include(item => item.InvoiceItems)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(i => i.Id == id && i.User!.Id == GetUserId());
          
            return entity;
        }

        public override async Task UpdateAsync(Invoice entity)
        {
            _context.Set<Invoice>().Update(entity);
            await _context.SaveChangesAsync();
        }
        public override async Task<List<Invoice>> GetAllAsync()
        {
            var dbInvoices = await _context.Invoices.Where(c => c.User!.Id == GetUserId()).ToListAsync();
            return dbInvoices;
        }

        public async Task UpdateTotalAmount (int id)
        {
            var invoice = await GetAsync(id);
            invoice.TotalAmount = invoice.InvoiceItems?.Sum(item => item.Quantity * item.Product.UnitPrice) ?? 0;
            await UpdateAsync(invoice);
        }

      
    }
}
