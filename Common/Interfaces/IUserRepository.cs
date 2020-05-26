using System;
using System.Threading.Tasks;

namespace WebApiDeveloperChallenge.Common.Interfaces
{
  /// <summary>
  ///   Interface from default repository methods
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IUserRepository<T> : IRepository<T> where T : class, IEntity
  {
    /// <summary>
    /// Get data without check user id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T> GetWithoutUserId(Guid id);
  }
}