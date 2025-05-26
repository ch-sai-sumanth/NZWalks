using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : Controller
{
    private readonly NZWalksDbContext dbContext;

    public RegionsController(NZWalksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetAllRegions()
    {
       
        var regions=await dbContext.Regions.ToListAsync();
        
        var regionsDto= new List<RegionDto>();


        foreach (var region in regions)
        {
            regionsDto.Add( new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
                
            });
        }
        return  Ok(regionsDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
    {

        // var region = dbContext.Regions.Find(id);
        
        var region=await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        if(region==null)
            return NotFound();

        var regionDto = new RegionDto()
        {
            Id = region.Id,
            Code = region.Code,
            Name = region.Name,
            RegionImageUrl = region.RegionImageUrl,
        };
        
        return Ok(regionDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequest)
    {

        var regionDomainModel = new Region()
        {
            Code = addRegionRequest.Code,
            Name = addRegionRequest.Name,
            RegionImageUrl = addRegionRequest.RegionImageUrl,
        };
        await dbContext.Regions.AddAsync(regionDomainModel);
        await dbContext.SaveChangesAsync();

        var regionDto = new RegionDto()
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl,
        };
        return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
    }
    
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> CreateRegion([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto updateRegionRequest)
    {

        var region = new Region()
        {
            Code = updateRegionRequest.Code,
            Name = updateRegionRequest.Name,
            RegionImageUrl = updateRegionRequest.RegionImageUrl,
        };
        
        await dbContext.Regions.AddAsync(region);
        await dbContext.SaveChangesAsync();

        var regionDto = new RegionDto()
        {
            Id = region.Id,
            Code = region.Code,
            Name = region.Name,
            RegionImageUrl = region.RegionImageUrl,
        };
        return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
    {
        var regionDomainModel=await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        
        if(regionDomainModel==null)
            return NotFound();
        
        dbContext.Regions.Remove(regionDomainModel);
        await dbContext.SaveChangesAsync();
        var regionDto = new RegionDto()
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl,
        };
        return Ok(regionDto);
    }
}