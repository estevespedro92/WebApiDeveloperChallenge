using System;

namespace WebApiDeveloperChallenge.Common.Interfaces
{
  /// <summary>
  /// Interface from userId entity
  /// </summary>
  public interface IUserIdEntity
  {
    public Guid UserId { get; set; }
  }
}