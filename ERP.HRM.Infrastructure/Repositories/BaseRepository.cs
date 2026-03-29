using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    /// <summary>
    /// Base repository class with common CRUD operations and soft delete support
    /// </summary>
    public abstract class BaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ERPDbContext Context;

        protected BaseRepository(ERPDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the DbSet for this entity type
        /// </summary>
        protected virtual DbSet<TEntity> DbSet => Context.Set<TEntity>();

        /// <summary>
        /// Get all active (non-deleted) entities
        /// </summary>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Get entity by ID (active only)
        /// </summary>
        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await DbSet
                .FirstOrDefaultAsync(x => x.IsDeleted == false && EF.Property<int>(x, "Id") == id);
        }

        /// <summary>
        /// Get entity by ID with includes
        /// </summary>
        public virtual async Task<TEntity?> GetByIdWithIncludesAsync(int id, params string[] includes)
        {
            var query = DbSet.AsQueryable();
            
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query
                .FirstOrDefaultAsync(x => x.IsDeleted == false && EF.Property<int>(x, "Id") == id);
        }

        /// <summary>
        /// Add a new entity
        /// </summary>
        public virtual async Task AddAsync(TEntity entity)
        {
            entity.CreatedDate ??= DateTime.UtcNow;
            entity.IsDeleted = false;
            await DbSet.AddAsync(entity);
        }

        /// <summary>
        /// Update an entity
        /// </summary>
        public virtual Task UpdateAsync(TEntity entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            DbSet.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Soft delete an entity
        /// </summary>
        public virtual Task DeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.ModifiedDate = DateTime.UtcNow;
            DbSet.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Hard delete an entity (permanent)
        /// </summary>
        public virtual Task HardDeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Check if entity exists
        /// </summary>
        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await DbSet.AnyAsync(x => x.IsDeleted == false && EF.Property<int>(x, "Id") == id);
        }

        /// <summary>
        /// Count active entities
        /// </summary>
        public virtual async Task<int> CountAsync()
        {
            return await DbSet.Where(x => !x.IsDeleted).CountAsync();
        }

        /// <summary>
        /// Get paged results
        /// </summary>
        public virtual async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; // Max 100 per page

            var query = DbSet.Where(x => !x.IsDeleted);
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        /// <summary>
        /// Get paged results with filter
        /// </summary>
        public virtual async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> filter)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = DbSet.Where(x => !x.IsDeleted);
            query = filter(query);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
