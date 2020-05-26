using System;
using Microsoft.AspNetCore.Http;
using WebApiDeveloperChallenge.Models;

namespace WebApiDeveloperChallenge.Repositories
{
  public class SkillRepository : RepositoryUserBase<Skill, ContactsContext>
  {
    public SkillRepository(ContactsContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
    {
    }
  }
}