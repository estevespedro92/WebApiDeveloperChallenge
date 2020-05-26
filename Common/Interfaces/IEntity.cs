using System;

namespace WebApiDeveloperChallenge.Common.Interfaces
{
  /// <summary>
  /// Interface from key entity
  /// </summary>
  public interface IEntity
  {
    public Guid Id { get; set; }
  }
}