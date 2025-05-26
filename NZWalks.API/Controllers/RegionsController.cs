using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly IRegionRepository regionRepository;

    public RegionsController(IRegionRepository regionRepository)
    {
        this.regionRepository = regionRepository;
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetAllRegionsAsync()
    {

        var regions = await regionRepository.GetAllAsync();
        
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

        var region = await regionRepository.GetByIdAsync(id);
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
    public async Task<IActionResult> AddRegionAsync([FromBody] AddRegionRequestDto addRegionRequest)
    {

        var regionDomainModel = new Region()
        {
            Code = addRegionRequest.Code,
            Name = addRegionRequest.Name,
            RegionImageUrl = addRegionRequest.RegionImageUrl,
        };
        await regionRepository.AddAsync(regionDomainModel);

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
    public async Task<IActionResult> UpdateRegion([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto updateRegionRequest)
    {

        var region = new Region()
        {
            Code = updateRegionRequest.Code,
            Name = updateRegionRequest.Name,
            RegionImageUrl = updateRegionRequest.RegionImageUrl,
        };
        
        region=await regionRepository.UpdateAsync(id,region);
        
        if(region==null)
            return NotFound();

        var regionDto = new RegionDto()
        {
            Code = region.Code,
            Name = region.Name,
            RegionImageUrl = region.RegionImageUrl,
        };
        return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
    {
        var regionDomainModel = await regionRepository.GetByIdAsync(id);
        
        if(regionDomainModel==null)
            return NotFound();
        
        await regionRepository.DeleteAsync(id);
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