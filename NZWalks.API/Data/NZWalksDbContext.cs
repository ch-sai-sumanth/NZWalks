using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data;

public class NZWalksDbContext :DbContext
{

    public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }
    
    
    DbSet<Difficulty> Difficulties { get; set; }
    DbSet<Region> Regions { get; set; }
    DbSet<Walk> Walks { get; set; }
    
}