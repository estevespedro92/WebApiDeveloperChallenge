using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDeveloperChallenge.Models
{
  public class Contact
  {
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Fullname { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string MobilePhoneNumber { get; set; }
  }
}
