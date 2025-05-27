using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalksController : ControllerBase
{
    private readonly IWalkRepository _walkRepository;
    private readonly IMapper _mapper;

    public WalksController(IWalkRepository walkRepository,IMapper mapper)
    {
        _walkRepository = walkRepository;
        _mapper = mapper;
    }
    [HttpPost]
    public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
    {
        var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
        
        await _walkRepository.CreateWalkAsync(walkDomainModel);
        return Ok();
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetWalk([FromRoute] Guid id)
    {
        var walkDomainModel = await _walkRepository.GetWalk(id);
        
        if(walkDomainModel == null)
            return NotFound();
        
        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn , [FromQuery] string? filterQuery,
        [FromQuery] string? orderBy, [FromQuery] bool isAscending,
        [FromQuery] int pageNumber=1 , int pageSize=100)
    {
        var walksDomainModels = await _walkRepository.GetAllWalksAsync(filterOn,filterQuery,orderBy,isAscending,pageNumber,pageSize);
        return Ok(_mapper.Map<List<WalkDto>>(walksDomainModels));
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateWalk([FromRoute] Guid id,[FromBody] UpdateWalkRequestDto updateWalkRequestDto)
    {
        var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);
        walkDomainModel = await _walkRepository.UpdateWalkAsync(id, walkDomainModel);
        
        if (walkDomainModel == null)
            return NotFound();
        
        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
    {
        var deletedWalkDomainModel = await _walkRepository.DeleteWalkAsync(id);

        if(deletedWalkDomainModel==null)
            return NotFound();
        return Ok(_mapper.Map<WalkDto>(deletedWalkDomainModel));
    }
    
}