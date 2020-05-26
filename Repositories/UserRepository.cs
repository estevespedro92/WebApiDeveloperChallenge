using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApiDeveloperChallenge.Common.Extensions;
using WebApiDeveloperChallenge.Models;

namespace WebApiDeveloperChallenge.Repositories
{
  public class UserRepository : RepositoryBase<UserApplication, ContactsContext>
  {
    private readonly ContactsContext _context;
    public UserRepository(ContactsContext context) : base(context)
    {
      _context = context;
    }

    public async Task<UserApplication> GetUser(string userName, string password)
    {
      return await _context.Set<UserApplication>()
        .FirstOrDefaultAsync(e => e.Username.Equals(userName) && e.Password.Equals(password));
    }
  }
}