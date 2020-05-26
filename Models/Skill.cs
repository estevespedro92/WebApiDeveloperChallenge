using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiDeveloperChallenge.Common.Enums;
using WebApiDeveloperChallenge.Common.Interfaces;

namespace WebApiDeveloperChallenge.Models
{
  public class Skill : IEntity, IUserIdEntity
  {
    [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
    [Required]
    public string Name { get; set; }

    [Required]
    public SkillLevelEnum Level { get; set; }

    public Guid UserId { get; set; }

    public virtual UserApplication User { get; set; }

    public ICollection<ContactSkill> ContactSkills { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
  }
}