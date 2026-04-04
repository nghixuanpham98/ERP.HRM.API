using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing products
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ERPDbContext _context;

        public ProductRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _context.Products
                .Where(p => p.IsDeleted == false)
                .ToListAsync();

        public async Task<Product?> GetByIdAsync(int id)
            => await _context.Products
                .Where(p => p.ProductId == id && p.IsDeleted == false)
                .FirstOrDefaultAsync();

        public async Task<Product?> GetByCodeAsync(string code)
            => await _context.Products
                .Where(p => p.ProductCode == code && p.IsDeleted == false)
                .FirstOrDefaultAsync();

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products
                .AnyAsync(p => p.ProductId == id && p.IsDeleted == false);
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var items = await _context.Products
                .Where(p => p.IsDeleted == false)
                .OrderBy(p => p.ProductCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.Products
                .Where(p => p.IsDeleted == false)
                .CountAsync();

            return (items, totalCount);
        }
    }
}
