using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiDeveloperChallenge.Representations
{
  public class ContactSkillRepresentation 
  {
    [Required]
    public ContactRepresentation Contact { get; set; }
    public IList<SkillRepresentation> Skills { get; set; }
  }
}