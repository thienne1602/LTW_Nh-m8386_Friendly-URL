using Microsoft.AspNetCore.Mvc;
using System.Linq;
using LTW_Nhóm8386_FriendlyURL.Data;
using LTW_Nhóm8386_FriendlyURL.Helpers;
using LTW_Nhóm8386_FriendlyURL.Models;

namespace LTW_Nhóm8386_FriendlyURL.Controllers
{
    public class URLsController : Controller
    {
        private readonly AppDbContext _context;

        public URLsController(AppDbContext context)
        {
            _context = context;
        }

        // Trang chính: hiển thị form nhập URL và danh sách URL đã lưu
        public IActionResult Index()
        {
            return View();
        }

        // Xử lý POST: chuyển đổi URL thành friendly URL và lưu vào database
        [HttpPost]
        public IActionResult GenerateFriendlyUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return BadRequest("URL không hợp lệ!");

            // Tạo slug cơ bản từ URL gốc (dựa vào đường dẫn)
            string baseSlug = UrlHelper.GenerateFriendlyText(url);
            string slug = baseSlug;
            int count = 1;

            // Kiểm tra tính duy nhất của slug trong DB
            while (_context.URLs.Any(u => u.FriendlyUrl == slug))
            {
                slug = $"{baseSlug}-{count}";
                count++;
            }

            var newUrl = new URL
            {
                OriginalUrl = url,
                FriendlyUrl = slug
            };

            _context.URLs.Add(newUrl);
            _context.SaveChanges();

            // Tạo friendly URL dạng relative
            string friendlyUrl = $"/r/{slug}";
            return Json(new { friendlyUrl });
        }

        // Lấy danh sách URL đã lưu
        [HttpGet]
        public IActionResult GetSavedUrls()
        {
            var urls = _context.URLs.OrderByDescending(u => u.CreatedAt).ToList();
            return Json(urls.Select(u => new { id = u.Id, friendlyUrl = $"/r/{u.FriendlyUrl}" }));
        }

        // Xóa URL đã lưu theo id
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

        // Action chuyển hướng: Khi người dùng truy cập /r/{slug}, chuyển hướng về URL gốc
        [HttpGet("/r/{slug}")]
        public IActionResult RedirectToOriginal(string slug)
        {
            var urlRecord = _context.URLs.FirstOrDefault(u => u.FriendlyUrl == slug);
            if (urlRecord == null)
                return NotFound();

            return Redirect(urlRecord.OriginalUrl);
        }
    }
}
