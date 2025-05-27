using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository;

public class SQLWalkRepository : IWalkRepository
{
    private readonly NZWalksDbContext _dbContext;

    public SQLWalkRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Walk> CreateWalkAsync(Walk walk)
    { 
        await _dbContext.Walks.AddAsync(walk);
        await _dbContext.SaveChangesAsync();
        return walk;
    }

    public async Task<Walk> GetWalk(Guid id)
    {
        return await _dbContext.Walks
            .Include(x=>x.Difficulty)
            .Include(x=>x.Region)
            .FirstOrDefaultAsync(x=>x.Id == id);
    }

    public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null , string? filterQuery=null, 
        string? orderBy=null, bool isAscending=true, int  pageNumber=1,int pageSize=100)
    {
        var walks=_dbContext.Walks
            .Include(x => x.Difficulty)
            .Include(x => x.Region)
            .AsQueryable();
        
        //filtering
        if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(x=>x.Name.Contains(filterQuery));
            }
        }
        
        //sorting
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            if (orderBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
            }
            else if (orderBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(x=>x.LengthInKm) : walks.OrderByDescending(x=>x.LengthInKm);
            }
        }
        
        //pagination
        var skipResults = (pageNumber - 1) * pageSize;
        
        return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
    }

    public async Task<Walk?> UpdateWalkAsync(Guid id,Walk walk)
    {
      var existingWalk = _dbContext.Walks.FirstOrDefault(x=>x.Id == id);

      if (existingWalk == null)
          return null;
        
      existingWalk.Name = walk.Name;
      existingWalk.Description = walk.Description;
      existingWalk.LengthInKm = walk.LengthInKm;
      existingWalk.WalkImageUrl = walk.WalkImageUrl;
      existingWalk.DifficultyId = walk.DifficultyId;
      existingWalk.RegionId = walk.RegionId;
      
        await _dbContext.SaveChangesAsync();
        return existingWalk;
    }

    public async Task<Walk?> DeleteWalkAsync(Guid id)
    {
        var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x=>x.Id == id);
        
        if(existingWalk==null)
            return null;
        _dbContext.Walks.Remove(existingWalk);
        await _dbContext.SaveChangesAsync();
        return existingWalk;
        
    }
}