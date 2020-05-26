using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiDeveloperChallenge.Common.Interfaces;

namespace WebApiDeveloperChallenge.Repositories
{
  public abstract class RepositoryBase<TEntity, TContext> : IRepository<TEntity>
    where TEntity : class, IEntity
    where TContext : DbContext
  {
    private readonly TContext _context;

    protected RepositoryBase(TContext context)
    {
      _context = context;
    }

    public async Task<TEntity> Create(TEntity entity)
    {
      await _context.Set<TEntity>().AddAsync(entity);
      await _context.SaveChangesAsync();
      return entity;
    }

    public async Task<TEntity> Delete(Guid id)
    {
      var entity = await Get(id);
      if (entity == null)
        return null;
      _context.Set<TEntity>().Remove(entity);
      await _context.SaveChangesAsync();
      return entity;
    }

    public async Task<TEntity> Get(Guid id)
    {
      return await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<TEntity>> GetAll()
    {
      return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> Update(TEntity entity)
    {
      _context.Entry(entity).State = EntityState.Modified;
      await _context.SaveChangesAsync();
      return entity;
    }
  }
}