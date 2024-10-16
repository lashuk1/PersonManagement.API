using Microsoft.EntityFrameworkCore;
using PersonManagement.Domain.Entities;

namespace PersonManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Person> Persons { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<AssociatedPerson> AssociatedPersons { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AssociatedPerson>()
           .HasOne(ap => ap.Person)
           .WithMany(p => p.AssociatedPersons)
           .HasForeignKey(ap => ap.PersonId)
           .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AssociatedPerson>()
            .HasOne(ap => ap.Associated)
            .WithMany()
            .HasForeignKey(ap => ap.AssociatedPersonId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}