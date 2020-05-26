using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApiDeveloperChallenge.Common.Interfaces;
using WebApiDeveloperChallenge.Models;

namespace WebApiDeveloperChallenge.Representations
{
  public class UserApplicationRepresentation : IEntity
  {
    [Required(AllowEmptyStrings = false)]
    public string Username { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }

    [JsonIgnore]
    public ICollection<Contact> Contacts { get; set; }

    [JsonIgnore]
    public ICollection<Skill> Skills { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
  }
}