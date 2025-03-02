using Microsoft.AspNetCore.Mvc;
using LTW_Nhóm8386_FriendlyURL.Services;
using Microsoft.Extensions.Logging;

namespace LTW_Nhóm8386_FriendlyURL.Controllers
{
    public class UrlShortenerController : Controller
    {
        private readonly UrlShortenerService _service;
        private readonly ILogger<UrlShortenerController> _logger;

        public UrlShortenerController(UrlShortenerService service, ILogger<UrlShortenerController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public IActionResult Shorten(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                _logger.LogError("URL không hợp lệ.");
                return BadRequest(new { error = "URL không được để trống!" });
            }

            try
            {
                var shortUrl = _service.ShortenUrl(url);
                return Json(new { shortUrl = Url.Action("RedirectToOriginal", "UrlShortener", new { shortUrl }, Request.Scheme) });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi rút gọn URL: {ex.Message}");
                return StatusCode(500, new { error = "Lỗi máy chủ. Vui lòng thử lại sau!" });
            }
        }

        [HttpGet("{shortUrl}")]
        public IActionResult RedirectToOriginal(string shortUrl)
        {
            if (string.IsNullOrEmpty(shortUrl))
            {
                _logger.LogError("Short URL không hợp lệ.");
                return BadRequest("Short URL không được để trống!");
            }

            var originalUrl = _service.GetOriginalUrl(shortUrl);
            if (originalUrl == null)
            {
                return NotFound("Không tìm thấy URL gốc!");
            }

            return Redirect(originalUrl);
        }
    }
}
