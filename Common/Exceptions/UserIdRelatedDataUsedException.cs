using System;

namespace WebApiDeveloperChallenge.Common.Extensions
{
  public class UserIdRelatedDataUsedException : Exception
  {
    public UserIdRelatedDataUsedException() : base("You do not have the right to use an item that belongs to another user.")
    {

    }
  }
}
