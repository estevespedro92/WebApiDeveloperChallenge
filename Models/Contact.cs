using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiDeveloperChallenge.Common.Interfaces;

namespace WebApiDeveloperChallenge.Models
{
  public class Contact : IEntity
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    [StringLength(60, ErrorMessage = "Firstname can't be longer than 60 characters")]
    [Required]
    public string Firstname { get; set; }

    [StringLength(60, ErrorMessage = "Lastname can't be longer than 60 characters")]
    [Required]
    public string Lastname { get; set; }

    public string Fullname => $"{Firstname} {Lastname}";

    [StringLength(120, ErrorMessage = "Address can't be longer than 120 characters")]
    [Required]
    public string Address { get; set; }

    [EmailAddress]
    [StringLength(120, ErrorMessage = "Email can't be longer than 120 characters")]
    public string Email { get; set; }

    [RegularExpression(@"(\+41)\s(7)([3,5,7,8,9]{1})\s(\d{3})\s(\d{2})\s(\d{2})", ErrorMessage = "MobilePhoneNumber is not in valid format")]
    [StringLength(16, ErrorMessage = "MobilePhoneNumber can't be longer than 16 characters")]
    [Required]
    public string MobilePhoneNumber { get; set; }

    public ICollection<ContactSkill> ContactSkills { get; set; }

  }
}