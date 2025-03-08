using FinTrack.Api.Data.DbContexts;
using FinTrack.Api.Data.IRepositories;
using FinTrack.Api.Domain.Models.Commons;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinTrack.Api.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Auditable
{
    private readonly DbSet<TEntity> dbSet;
    private readonly AppDbContext dbContext;

    public Repository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.dbSet = dbContext.Set<TEntity>();
    }
    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await this.dbSet.FirstOrDefaultAsync(e => e.Id == id);
        dbSet.Remove(entity);
        return true;
    }

    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
         => (await this.dbSet.AddAsync(entity)).Entity;


    public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default)
         => (await this.dbContext.SaveChangesAsync() > 0);

    public IQueryable<TEntity> SelectAll(Expression<Func<TEntity, bool>> expression = null)
    {
        var query = expression is null ? dbSet : dbSet.Where(expression);
        return query;
    }

    public async Task<TEntity> SelectAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        => await this.SelectAll(expression).FirstOrDefaultAsync();

    public TEntity Update(TEntity entity)
        => this.dbSet.Update(entity).Entity;
}
