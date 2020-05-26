using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApiDeveloperChallenge.Common.Extensions;
using WebApiDeveloperChallenge.Common.Interfaces;

namespace WebApiDeveloperChallenge.Models
{
  public class ContactsContext : DbContext
  {
    public readonly IHttpContextAccessor _httpContextAccessor;
    public ContactsContext(DbContextOptions<ContactsContext> options,IHttpContextAccessor httpContextAccessor ) : base(options)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<ContactSkill> ContactSkills { get; set; }
    public DbSet<UserApplication> Users { get; set; }

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

      modelBuilder.Entity<UserApplication>()
        .HasMany(u => u.Contacts)
        .WithOne(c => c.User)
        .HasForeignKey(c => c.UserId)
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<UserApplication>()
        .HasMany(u => u.Skills)
        .WithOne(s => s.User)
        .HasForeignKey(s => s.UserId)
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Contact>().HasIndex(s => new { s.Firstname, s.Lastname, s.UserId }).IsUnique();
      modelBuilder.Entity<Skill>().HasIndex(s => new {s.Name, s.Level, s.UserId }).IsUnique();
      modelBuilder.Entity<UserApplication>().HasIndex(u => u.Username).IsUnique();
    }


    /// <summary>
    /// Override SaveChangesAsync to add UserId
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      var entitiesList = ChangeTracker.Entries<IUserIdEntity>().Where(e => e.State.Equals(EntityState.Added)).ToList();
      if(!entitiesList.Any())
        return base.SaveChangesAsync(cancellationToken);

      var userId = _httpContextAccessor.HttpContext.User.GetUserId();
      entitiesList.ForEach(e => e.Property(p => p.UserId).CurrentValue = userId);
      return base.SaveChangesAsync(cancellationToken);
    }
  }
}