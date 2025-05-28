using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageRepository _imageRepository;

    public ImageController(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload(ImageUploadRequestDto request)
    {
        ValidateFileUpload(request);

        if (ModelState.IsValid)
        {
            var imageDomainModel = new Image
            {
                File = request.File,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileName = request.FileName,
                FileDescription = request.FileDescription,
            };

            await _imageRepository.Upload(imageDomainModel);
            return Ok(imageDomainModel);
        }
        return BadRequest(ModelState);
    }

    private void ValidateFileUpload(ImageUploadRequestDto request)
    {
        
        var allowedExtensions = new string[] { "jpg", "jpeg", "png" };

        if (allowedExtensions.Contains(Path.GetExtension(request.FileName)))
        {
            ModelState.AddModelError("File", "Unsupported file type");
        }

        if (request.File.Length > 10485760)
        {
            ModelState.AddModelError("File", "File size too large");
        }
    }
}