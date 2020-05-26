using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApiDeveloperChallenge.Common.Extensions;
using WebApiDeveloperChallenge.Common.Interfaces;

namespace WebApiDeveloperChallenge.Repositories
{
  public abstract class RepositoryUserBase<TEntity, TContext> :  IUserRepository<TEntity>
    where TEntity : class, IEntity, IUserIdEntity
    where TContext : DbContext
  {
    private readonly TContext _context;
    private readonly Guid _currentUserId;

    protected RepositoryUserBase(TContext context, IHttpContextAccessor httpContextAccessor)
    {
      _context = context;
      _currentUserId = httpContextAccessor.HttpContext.User.GetUserId();
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
      return await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id) && e.UserId.Equals(_currentUserId));
    }

    public async Task<List<TEntity>> GetAll()
    {
      return await _context.Set<TEntity>().Where(e => e.UserId.Equals(_currentUserId)).ToListAsync();
    }

    public async Task<TEntity> Update(TEntity entity)
    {
      var oldEntity = await Get(entity.Id);
      entity.UserId = oldEntity.UserId;
      _context.Entry(oldEntity).CurrentValues.SetValues(entity);
      await _context.SaveChangesAsync();
      return entity;
    }

    public async Task<List<TEntity>> GetAllWithoutUserId()
    {
      return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> GetWithoutUserId(Guid id)
    {
      return await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id));
    }
  }
}