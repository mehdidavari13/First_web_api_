using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FirstWebAPI.Core.Context;
using FirstWebAPI.Core.Contracts;

namespace FirstWebAPI.Core.Repositories
{
    public class GenericRepository<T>:IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IHttpContextAccessor _contextAccessor;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public GenericRepository(ApplicationDbContext context ,IHttpContextAccessor contextAccessor) :this (context)
            
        {
            _contextAccessor = contextAccessor;
        }



        protected int GetUserId() => int.Parse(_contextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier)!);



        public virtual async Task<T> AddAsync(T newEntity)
        {
            
            await _context.Set<T>().AddAsync(newEntity);
            await _context.SaveChangesAsync();
            return newEntity;
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> Exists(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            return entity != null;
        }
        public virtual async Task<List<T>> GetAllAsync()
        {
           return await _context.Set<T>().ToListAsync();
        }
        public virtual async Task<T> GetAsync(int? id)
        {
            if (id == null)
                return null;
            var entity = await _context.Set<T>().FindAsync(id);
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
