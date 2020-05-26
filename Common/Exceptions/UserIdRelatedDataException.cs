using System;

namespace WebApiDeveloperChallenge.Common.Exceptions
{
  public class UserIdRelatedDataException : Exception
  {
    public UserIdRelatedDataException() : base("You do not have the right to modify an item that belongs to another user.")
    {

    }
  }
}
