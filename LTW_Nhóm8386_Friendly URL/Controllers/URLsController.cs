using Microsoft.AspNetCore.Mvc;
using System.Linq;
using LTW_Nhóm8386_FriendlyURL.Data;
using LTW_Nhóm8386_FriendlyURL.Helpers;
using LTW_Nhóm8386_FriendlyURL.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace LTW_Nhóm8386_FriendlyURL.Controllers
{
    public class URLsController : Controller
    {
        private readonly AppDbContext _context;

        public URLsController(AppDbContext context)
        {
            _context = context;
        }

        // Trang chính
        public IActionResult Index()
        {
            return View();
        }

        // Xử lý tạo Friendly URL
        [HttpPost]
        public IActionResult GenerateFriendlyUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return BadRequest("URL không hợp lệ!");

            // Chuẩn hóa URL trước khi xử lý
            string normalizedUrl = UrlHelper.NormalizeUrl(url);

            // Tạo đối tượng Uri từ URL đã chuẩn hóa
            Uri uri = new Uri(normalizedUrl);
            string domain = uri.Host; // Lấy domain chính xác

            // Tạo Friendly URL từ toàn bộ URL
            string friendlySlug = UrlHelper.GenerateFriendlySlug(normalizedUrl);  // Sửa lỗi tại đây

            // Kiểm tra URL đã tồn tại chưa
            var existingUrl = _context.URLs.FirstOrDefault(u => u.OriginalUrl == normalizedUrl);
            if (existingUrl != null)
            {
                return Json(new { friendlyUrl = $"/{existingUrl.FriendlyUrl}" });
            }

            // Lưu URL vào database
            var newUrl = new URL
            {
                OriginalUrl = normalizedUrl,
                FriendlyUrl = friendlySlug,
                WebsiteName = domain,
                CreatedAt = DateTime.UtcNow
            };

            _context.URLs.Add(newUrl);
            _context.SaveChanges();

            return Json(new { friendlyUrl = $"/{friendlySlug}" });
        }

        // API lấy danh sách URL đã lưu
        [HttpGet]
        public IActionResult GetSavedUrls()
        {
            var urls = _context.URLs.AsNoTracking().OrderByDescending(u => u.CreatedAt).ToList();
            return Json(urls.Select(u => new
            {
                id = u.Id,
                friendlyUrl = $"/{u.FriendlyUrl}",
                websiteName = u.WebsiteName
            }));
        }

        // Xóa URL đã lưu
        [HttpPost]
        public IActionResult DeleteUrl(int id)
        {
            var url = _context.URLs.FirstOrDefault(u => u.Id == id);
            if (url == null)
                return NotFound(new { success = false, message = "URL không tồn tại." });

            _context.URLs.Remove(url);
            _context.SaveChanges();
            return Json(new { success = true });
        }

        // Chuyển hướng đến URL gốc
        [HttpGet("{slug}")]
        public IActionResult RedirectToOriginal(string slug)
        {
            var urlRecord = _context.URLs.FirstOrDefault(u => u.FriendlyUrl == slug);
            if (urlRecord == null)
            {
                return NotFound("Không tìm thấy URL được rút gọn này. Vui lòng kiểm tra lại!");
            }

            // Trả về Redirect 301 (Moved Permanently)
            return RedirectPermanent(urlRecord.OriginalUrl);
        }
    }
}
