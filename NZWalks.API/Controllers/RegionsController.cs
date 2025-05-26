using Microsoft.AspNetCore.Mvc;
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
    public IActionResult GetAllRegions()
    {
       
        var regions=dbContext.Regions.ToList();
        
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
    public IActionResult GetRegionById([FromRoute] Guid id)
    {

        // var region = dbContext.Regions.Find(id);
        
        var region=dbContext.Regions.FirstOrDefault(x => x.Id == id);
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
    public IActionResult CreateRegion([FromBody] AddRegionRequestDto addRegionRequest)
    {

        var regionDomainModel = new Region()
        {
            Code = addRegionRequest.Code,
            Name = addRegionRequest.Name,
            RegionImageUrl = addRegionRequest.RegionImageUrl,
        };
        dbContext.Regions.Add(regionDomainModel);
        dbContext.SaveChanges();

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
    public IActionResult CreateRegion([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto updateRegionRequest)
    {

       var existingRegion = dbContext.Regions.FirstOrDefault(x => x.Id == id);
       if(existingRegion==null)
           return NotFound();
       
       existingRegion.Code = updateRegionRequest.Code;
       existingRegion.Name = updateRegionRequest.Name;
       existingRegion.RegionImageUrl = updateRegionRequest.RegionImageUrl;
       dbContext.Regions.Update(existingRegion);
       dbContext.SaveChanges();

       var regionDto = new RegionDto()
       {
           Id = existingRegion.Id,
           Code = existingRegion.Code,
           Name = existingRegion.Name,
           RegionImageUrl = existingRegion.RegionImageUrl,
       };
       return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public IActionResult DeleteRegion([FromRoute] Guid id)
    {
        var regionDomainModel=dbContext.Regions.FirstOrDefault(x => x.Id == id);
        
        if(regionDomainModel==null)
            return NotFound();
        
        dbContext.Regions.Remove(regionDomainModel);
        dbContext.SaveChanges();
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