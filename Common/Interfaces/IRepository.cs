using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiDeveloperChallenge.Common.Interfaces
{
  /// <summary>
  /// Interface from default repository methods
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IRepository<T> where T : class, IEntity
  {
    Task<List<T>> GetAll();
    Task<T> Get(Guid id);

    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(Guid id);
  }
}