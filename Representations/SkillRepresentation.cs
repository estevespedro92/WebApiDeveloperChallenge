using System;
using System.ComponentModel.DataAnnotations;
using WebApiDeveloperChallenge.Common.Enums;
using WebApiDeveloperChallenge.Common.Interfaces;

namespace WebApiDeveloperChallenge.Representations
{
  public class SkillRepresentation : IEntity
  {
    public Guid Id { get; set; }

    [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
    [Required]
    public string Name { get; set; }

    [Required]
    public SkillLevelEnum Level { get; set; }
  }
}