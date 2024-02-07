using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLearn360.API.Controllers.ImageController
{
	[Route("api/Images")]
	[ApiController]
    public class UploadImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadImageController(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
		#region UploadCourseImage
		[HttpPost("courseimage")]
		public IActionResult UploadCourseImage()
		{
			try
			{

				var file = Request.Form.Files[0];


				if (file.Length > 0)
				{
					var fileName = $"{Guid.NewGuid()}_{file.FileName}";
					var path = $"{_webHostEnvironment.WebRootPath}\\CourseImages\\{fileName}";
					string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
					using (var stream = new FileStream(path, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					string foldername = "CourseImages";
					return Ok($"https://{currentUrl}/api/Images/{foldername}/{fileName}");
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		#endregion

		#region UploadOrganizationImage
		[HttpPost("organizationimage")]
		public IActionResult UploadOrganizationImage()
		{
			try
			{
				var file = Request.Form.Files[0];
				if (file.Length > 0)
				{
					var fileName = $"{Guid.NewGuid()}_{file.FileName}";
					var path = $"{_webHostEnvironment.WebRootPath}\\OrganizationImages\\{fileName}";
					string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
					using (var stream = new FileStream(path, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					string foldername = "OrganizationImages";
					return Ok($"https://{currentUrl}/api/Images/{foldername}/{fileName}");
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		#endregion

		#region UploadPathImage
		[HttpPost("pathimage")]
		public IActionResult UploadPathImage()
		{
			try
			{
				var file = Request.Form.Files[0];
				if (file.Length > 0)
				{
					var fileName = $"{Guid.NewGuid()}_{file.FileName}";
					var path = $"{_webHostEnvironment.WebRootPath}\\PathImages\\{fileName}";
					string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
					using (var stream = new FileStream(path, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					string foldername = "PathImages";
					return Ok($"https://{currentUrl}/api/Images/{foldername}/{fileName}");
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		#endregion

		#region UploadFilialImage
		[HttpPost("filialimage")]
		public IActionResult UploadFilialImage()
		{
			try
			{

				var file = Request.Form.Files[0];

				if (file.Length > 0)
				{
					var fileName = $"{Guid.NewGuid()}_{file.FileName}";
					var path = $"{_webHostEnvironment.WebRootPath}\\FilialImages\\{fileName}";
					string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
					using (var stream = new FileStream(path, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					string foldername = "FilialImages";
					return Ok($"https://{currentUrl}/api/Images/{foldername}/{fileName}");
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		#endregion

		#region UploadUserImage
		[HttpPost("userimage")]
		public IActionResult UploadUserImage()
		{
			try
			{
				var file = Request.Form.Files[0];

				if (file.Length > 0)
				{
					var fileName = $"{Guid.NewGuid()}_{file.FileName}";
					var path = $"{_webHostEnvironment.WebRootPath}\\UserImages\\{fileName}";
					string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
					using (var stream = new FileStream(path, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					string foldername = "UserImages";
					return Ok($"https://{currentUrl}/api/Images/{foldername}/{fileName}");
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		#endregion

		#region UploadGroupImage
		[HttpPost("groupimage")]
		public IActionResult UploadGroupImage()
		{
			try
			{
				var file = Request.Form.Files[0];

				if (file.Length > 0)
				{
					var fileName = $"{Guid.NewGuid()}_{file.FileName}";
					var path = $"{_webHostEnvironment.WebRootPath}\\GroupImages\\{fileName}";
					string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
					using (var stream = new FileStream(path, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					string foldername = "GroupImages";
					return Ok($"https://{currentUrl}/api/Images/{foldername}/{fileName}");
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		#endregion

		#region UploadQuestionImage
		[HttpPost("questionimage")]
		public IActionResult UploadQuestionImage()
		{
			try
			{
				var file = Request.Form.Files[0];

				if (file.Length > 0)
				{
					var fileName = $"{Guid.NewGuid()}_{file.FileName}";
					var path = $"{_webHostEnvironment.WebRootPath}\\QuestionImages\\{fileName}";
					string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
					using (var stream = new FileStream(path, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					string foldername = "QuestionImages";
					return Ok($"https://{currentUrl}/api/Images/{foldername}/{fileName}");
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		#endregion

		#region UploadImageCKEDITOR
		[HttpPost, DisableRequestSizeLimit]
		public IActionResult UploadImageCkeditor()
		{
			try
			{

				var file = Request.Form.Files[0];

				if (file.Length > 0)
				{
					var fileName = $"{Guid.NewGuid()}_{file.FileName}";
					var path = $"{_webHostEnvironment.WebRootPath}\\CkEditorImages\\{fileName}";
					string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
					using (var stream = new FileStream(path, FileMode.Create))
					{
						try
						{
							file.CopyTo(stream);

						}
						catch (Exception err)
						{
							Console.WriteLine(err);
							throw;
						}
					}

					string foldername = "CkEditorImages";
					return Ok(new { url = $"https://{currentUrl}/api/Images/{foldername}/{fileName}" });
				}
				else
				{
					return NotFound("Désolé mais l'upload de l'image a échoué");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		#endregion

		#region LoadFile
		[HttpGet("{foldername}/{fileName}")]
		public async Task<IActionResult> LoadFile(string foldername, string fileName)
		{
			try
			{
				var path = $"{_webHostEnvironment.WebRootPath}\\{foldername}\\{fileName}";

				var memory = new MemoryStream();
				using (var stream = new FileStream(path, FileMode.Open))
				{
					await stream.CopyToAsync(memory);
				}
				memory.Position = 0;
				return File(memory, @"application/octet-stream", Path.GetFileName(path));
			}
			catch (Exception err)
			{
				return BadRequest(err);
			}
		}
		#endregion

		#region LoadingByDefault
		[HttpGet("loadingbydefault/{fileName}")]
		public async Task<IActionResult> LoadImageByDefault(string fileName)
		{

			try
			{
				var path = $"{_webHostEnvironment.WebRootPath}\\ImageByDefault\\{fileName}";

				var memory = new MemoryStream();
				using (var stream = new FileStream(path, FileMode.Open))
				{
					await stream.CopyToAsync(memory);
				}
				memory.Position = 0;
				return File(memory, @"application/octet-stream", Path.GetFileName(path));
			}
			catch (Exception err)
			{
				return BadRequest(err);
			}

		}
		#endregion
	}
}
