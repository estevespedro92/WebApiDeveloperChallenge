using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiDeveloperChallenge.Models;

namespace WebApiDeveloperChallenge.Repositories
{
  public class ContactRepository : RepositoryBase<Contact, ContactsContext>
  {
    private readonly ContactsContext _context;
    public ContactRepository(ContactsContext context) : base(context)
    {
      _context = context;
    }

    public async Task<List<ContactSkill>> GetWithSkills()
    {
      return await _context.Set<ContactSkill>().Include(c =>c.Contact).Include(c => c.Skill).ToListAsync();
    }
  }
}