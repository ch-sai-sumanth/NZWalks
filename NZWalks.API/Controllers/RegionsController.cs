using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;

    public RegionsController(IRegionRepository regionRepository,IMapper mapper)
    {
        _regionRepository = regionRepository;
        _mapper = mapper;
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetAllRegionsAsync()
    {

        var regions = await _regionRepository.GetAllAsync();
        return  Ok(_mapper.Map<List<RegionDto>>(regions));
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
    {

        // var region = dbContext.Regions.Find(id);

        var region = await _regionRepository.GetByIdAsync(id);
        if(region==null)
            return NotFound();
        
        return Ok(_mapper.Map<RegionDto>(region));
    }

    [HttpPost]
    public async Task<IActionResult> AddRegionAsync([FromBody] AddRegionRequestDto addRegionRequest)
    {

        var regionDomainModel = _mapper.Map<Region>(addRegionRequest);
        await _regionRepository.AddAsync(regionDomainModel);

        var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
        return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
    }
    
    [HttpPut]
    [Route("{id:Guid}")]
    [ValidateModel]
    public async Task<IActionResult> UpdateRegion([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto updateRegionRequest)
    {

        var region = _mapper.Map<Region>(updateRegionRequest);
        region=await _regionRepository.UpdateAsync(id,region);


        return Ok(_mapper.Map<RegionDto>(region));
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
    {
        var regionDomainModel = await _regionRepository.GetByIdAsync(id);
        
        if(regionDomainModel==null)
            return NotFound();
        
        await _regionRepository.DeleteAsync(id);
        
        return Ok(_mapper.Map<RegionDto>(regionDomainModel));
    }
}