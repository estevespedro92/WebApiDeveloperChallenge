using Microsoft.EntityFrameworkCore;

namespace WebApiDeveloperChallenge.Models
{
  public class ContactsContext : DbContext
  {
    public ContactsContext(DbContextOptions<ContactsContext> options) : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<ContactSkill> ContactSkills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<ContactSkill>()
        .HasKey(cs => new {cs.ContactId, cs.SkillId});

      modelBuilder.Entity<ContactSkill>()
        .HasOne(cs => cs.Contact)
        .WithMany(c => c.ContactSkills)
        .HasForeignKey(cs => cs.ContactId);

      modelBuilder.Entity<ContactSkill>()
        .HasOne(cs => cs.Skill)
        .WithMany(c => c.ContactSkills)
        .HasForeignKey(cs => cs.SkillId);

      modelBuilder.Entity<Contact>().HasIndex(s => new { s.Firstname, s.Lastname }).IsUnique();
      modelBuilder.Entity<Skill>().HasIndex(s => new {s.Name, s.Level}).IsUnique();

    }
  }
}