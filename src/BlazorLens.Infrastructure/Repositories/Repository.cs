using BlazorLens.Application.Interfaces;
using BlazorLens.Infrastructure.Data;
using BlazorLens.Shared.Kernel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorLens.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity<Guid>
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
