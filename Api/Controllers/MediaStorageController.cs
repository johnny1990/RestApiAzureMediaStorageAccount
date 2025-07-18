using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaStorageController : ControllerBase
    {
        private readonly IMediaStorageService _mediaStorageService;

        public MediaStorageController(IMediaStorageService mediaStorageService)
        {
            _mediaStorageService = mediaStorageService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ListAllFiles()
        {
            try
            {
                var files = await _mediaStorageService.GetAllFiles();
                return Ok(files);
            }
            catch (Azure.RequestFailedException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UploadFileMedia(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("File is null");
            }

            if (file.Length == 0)
            {
                return BadRequest("File is empty");
            }

            using var stream = file.OpenReadStream();
            var fileName = await _mediaStorageService.UploadFile(stream, file.FileName, file.ContentType);
            return Ok(fileName);
        }

        [HttpPut("{fileName}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ReplaceFileMedia(string fileName, IFormFile fileToReplace)
        {
            if (fileToReplace == null)
            {
                return BadRequest("File to replace is null");
            }
            if (fileToReplace.Length == 0)
            {
                return BadRequest("File to replace is empty");
            }
            using var stream = fileToReplace.OpenReadStream();
            var replacedFileName = await _mediaStorageService.ReplaceFile(stream, fileToReplace.FileName, fileToReplace.ContentType, fileName);
            return Ok(replacedFileName);
        }


        [HttpDelete("{fileName}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteFileMedia(string fileName)
        {
            var result = await _mediaStorageService.DeleteFile(fileName);
            return result ? NoContent() : NotFound();
        }
    }
}
