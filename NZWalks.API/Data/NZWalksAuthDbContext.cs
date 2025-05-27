using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data;

public class NZWalksAuthDbContext : IdentityDbContext<IdentityUser>
{
    public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var roles = new List<IdentityRole>()
        {
            new IdentityRole()
            {
                Id = "6997a368-a379-4af0-a29b-fcb65bd8f3fc",
                ConcurrencyStamp = "6997a368-a379-4af0-a29b-fcb65bd8f3fc",
                Name = "Reader",
                NormalizedName = "Reader".ToUpper()
            },
            new IdentityRole()
            {
                Id = "7c621b64-aa74-418c-8564-fd0b50bd0770",
                ConcurrencyStamp = "7c621b64-aa74-418c-8564-fd0b50bd0770",
                Name = "Writer",
                NormalizedName = "Writer".ToUpper()
            }
        };
        
        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}