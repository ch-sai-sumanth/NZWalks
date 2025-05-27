using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository;

public interface IWalkRepository
{
    Task<Walk> CreateWalkAsync(Walk walk);
    Task<Walk?> GetWalk(Guid id);
    
    Task<List<Walk>> GetAllWalksAsync(string? filterOn=null, string? filterQuery=null,
        string? orderBy=null, bool isAscending=true, int pageNumber=1,int pageSize=100);
    
    Task<Walk?> UpdateWalkAsync(Guid id,Walk walk);
    
    Task<Walk?> DeleteWalkAsync(Guid id);
}