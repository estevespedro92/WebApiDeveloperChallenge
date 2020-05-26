using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApiDeveloperChallenge.Common.Extensions;
using WebApiDeveloperChallenge.Models;

namespace WebApiDeveloperChallenge.Repositories
{
  public class ContactRepository : RepositoryUserBase<Contact, ContactsContext>
  {
    private readonly ContactsContext _context;
    private readonly Guid _currentUserId;

    public ContactRepository(ContactsContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
    {
      _context = context;
      _currentUserId = httpContextAccessor.HttpContext.User.GetUserId();
    }

    public async Task<List<Contact>> Get(bool isSkillsIncluded)
    {
      if (isSkillsIncluded)
        return await _context.Contacts.Where(s => s.UserId.Equals(_currentUserId)).Include(s => s.ContactSkills)
          .ThenInclude(cs => cs.Skill).ToListAsync();

      return await _context.Contacts.Where(s => s.UserId.Equals(_currentUserId)).ToListAsync();
    }

    public async Task<Contact> GetById(Guid id, bool isSkillsIncluded)
    {
      if (isSkillsIncluded)
        return await _context.Contacts.Include(s => s.ContactSkills).ThenInclude(cs => cs.Skill)
          .FirstOrDefaultAsync(s => s.UserId.Equals(_currentUserId) && s.Id.Equals(id));

      return await _context.Contacts.FirstOrDefaultAsync(s =>
        s.UserId.Equals(_currentUserId) && s.UserId.Equals(_currentUserId));
    }

    public async Task<Contact> Create(Contact contact, IList<Guid> skillsIds)
    {
      using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
      await Create(contact);

      if (skillsIds.Any())
      {
        var contactSkills = skillsIds.Select(s => new ContactSkill
        {
          ContactId = contact.Id,
          SkillId = s
        });

        await _context.ContactSkills.AddRangeAsync(contactSkills);
      }

      await _context.SaveChangesAsync();
      transactionScope.Complete();
      transactionScope.Dispose();
      return await GetById(contact.Id, true);
    }

    public async Task<Contact> Update(Contact contact, IList<Guid> skillsIds)
    {
      using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

      var oldContact = await GetById(contact.Id, true);

      // Preserve userid;
      contact.UserId = oldContact.UserId;

      _context.ContactSkills.RemoveRange(oldContact.ContactSkills);

      var contactSkills = skillsIds.Select(s => new ContactSkill
      {
        ContactId = contact.Id,
        SkillId = s
      });

      await _context.ContactSkills.AddRangeAsync(contactSkills);
      _context.Entry(oldContact).CurrentValues.SetValues(contact);

      await _context.SaveChangesAsync();
      transactionScope.Complete();
      transactionScope.Dispose();
      return await GetById(contact.Id, true);
    }
  }
}