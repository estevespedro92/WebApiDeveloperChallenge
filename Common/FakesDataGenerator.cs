using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApiDeveloperChallenge.Models;

namespace WebApiDeveloperChallenge.Common
{
  public class FakesDataGenerator
  {
    /// <summary>
    /// Set default data in database
    /// </summary>
    /// <param name="provider"></param>
    public static async void Initialize(IServiceProvider provider)
    {
      await using var context = new ContactsContext(provider.GetRequiredService<DbContextOptions<ContactsContext>>());

      if (context.Contacts.Any() && context.Skills.Any())
        return;

      context.Contacts.AddRange(
        new List<Contact>
        {
          new Contact
          {
            Firstname = "Marc",
            Lastname = "DelaHouse",
            Address = "Rue du test 45",
            MobilePhoneNumber = "+41 73 908 72 54"
          },
          new Contact
          {
            Firstname = "Christian",
            Lastname = "Duppont",
            Address = "Rue du Marché 12",
            MobilePhoneNumber = "+41 79 143 89 45"
          }
        }
      );

      context.Skills.AddRange(new List<Skill>
      {
        new Skill{ Name = "C#", Level = 0},
      });
      context.SaveChanges();
    }
  }
}