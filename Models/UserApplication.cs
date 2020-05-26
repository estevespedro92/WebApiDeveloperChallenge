using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using WebApiDeveloperChallenge.Common.Interfaces;

namespace WebApiDeveloperChallenge.Models
{
  [Table("User")]
  public class UserApplication : IEntity
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Username { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }

    [JsonIgnore]
    public ICollection<Contact> Contacts { get; set; }

    [JsonIgnore]
    public ICollection<Skill> Skills { get; set; }
  }
}