using Microsoft.OpenApi.Expressions;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository;

public class LocalImageRepository : IImageRepository
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly NZWalksDbContext _dbContext;

    public LocalImageRepository(IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor,NZWalksDbContext dbContext)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }   
    public async Task<Image> Upload(Image image)
    {
        var fileNameWithExtension = $"{image.FileName}{image.FileExtension}";
        var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", fileNameWithExtension);
        
        
        using var steam = new FileStream(localFilePath, FileMode.Create);
        await image.File.CopyToAsync(steam);

        var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
        
        image.FIlePath=urlFilePath;
        
        await _dbContext.Images.AddAsync(image);
        await _dbContext.SaveChangesAsync();

        return image;
    }
}