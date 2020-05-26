using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiDeveloperChallenge.Representations
{
  public class ContactSkillSimplifiedRepresentation 
  {
    [Required]
    public ContactRepresentation Contact { get; set; }
    public IList<Guid> SkillIds { get; set; }
  }
}