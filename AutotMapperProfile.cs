using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WebApiDeveloperChallenge.Models;
using WebApiDeveloperChallenge.Representations;

namespace WebApiDeveloperChallenge
{
  public class AutotMapperProfile : Profile
  {
    public AutotMapperProfile()
    {
      CreateMap<ContactRepresentation, Contact>();
      CreateMap<Contact, ContactRepresentation>();
      CreateMap<Contact, ContactSkillRepresentation>()
        .ForMember(cs => cs.Contact, o => o.MapFrom(d => d))
        .ForMember(cs => cs.Skills, o => o.MapFrom(d => d.ContactSkills.Select(c => c.Skill)));
      CreateMap<Skill, SkillRepresentation>();
      CreateMap<SkillRepresentation, Skill>();
      CreateMap<ContactSkillRepresentation, ContactRepresentation>();
    }
  }
}