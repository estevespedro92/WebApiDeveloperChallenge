using System;
using WebApiDeveloperChallenge.Models;

namespace WebApiDeveloperChallenge.Repositories
{
  public class SkillRepository : RepositoryBase<Skill, ContactsContext>
  {
    public SkillRepository(ContactsContext context) : base(context)
    {
    }
  }
}