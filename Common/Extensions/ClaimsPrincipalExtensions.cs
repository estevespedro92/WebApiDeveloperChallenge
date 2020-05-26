using System;
using System.Security.Claims;

namespace WebApiDeveloperChallenge.Common.Extensions
{
  public static class ClaimsPrincipalExtensions
  {
    /// <summary>
    /// Get name identifier claim value
    /// </summary>
    /// <param name="identity"></param>
    /// <returns></returns>
    public static Guid GetUserId(this ClaimsPrincipal identity)
    {
      return Guid.Parse(identity.FindFirst(f => f.Type == ClaimTypes.NameIdentifier).Value);
    }
  }
}