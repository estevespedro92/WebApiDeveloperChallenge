using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiDeveloperChallenge.Models
{
  public class ContactSkill
  {
    public Guid ContactId { get; set; }

    public Guid SkillId { get; set; }

    public virtual Contact Contact { get; set; }

    public virtual Skill Skill { get; set; }
  }
}