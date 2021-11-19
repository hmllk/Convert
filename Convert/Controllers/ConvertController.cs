using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Convert.Extension;
using Convert.PostModel;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Convert.Controllers
{
    [ApiController]
    [Route("convert")]
    public class ConvertController : ControllerBase
    {
        private readonly ILogger<ConvertController> _logger;
        private readonly IOfficeService _officeService;

        public ConvertController(ILogger<ConvertController> logger, IOfficeService officeService)
        {
            _logger = logger;
            _officeService = officeService;
        }

        [HttpPost("wordtoimg")]
        public async Task<IActionResult> WordToImgs([FromBody] PDFPostModel post)
        {
            var formFile = HttpContext.Request.Form?.Files?.FirstOrDefault();
            if (formFile == null)
            {
                throw new ApplicationException("上传文件不存在");
            }

            //保留到本地
            var strPath = Path.Combine(AppContext.BaseDirectory, "wwwroot", $"{post.Key}",
                $"{DateTime.Now:s}");
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }

            var path = Path.Combine(strPath, formFile.Name);

            await using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync((Stream) stream, new CancellationToken());
                var array = stream.ToArray();
                if (!System.IO.File.Exists(path))
                {
                    await System.IO.File.WriteAllBytesAsync(path, array);
                }
            }

            //转为pdf
            _officeService.ConvertToPdf(path, strPath);

            //转为img
            var pdfFile = Path.Combine(strPath, $"{formFile.Name}.pdf");
            await _officeService.PdfToImage(formFile.Name, pdfFile, strPath);
            return new JsonResult("ok");
        }
    }
}