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

        // Trang chính: hiển thị form nhập URL
        public IActionResult Index()
        {
            return View();
        }

        // Xử lý POST: chuyển đổi URL thành Friendly URL
        [HttpPost]
        public IActionResult GenerateFriendlyUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return BadRequest("URL không hợp lệ!");
            }

            string friendlyUrl = UrlHelper.GenerateFriendlyUrl(url);

            // Kiểm tra nếu URL đã tồn tại trong DB (để tránh trùng lặp)
            var existingUrl = _context.URLs.FirstOrDefault(u => u.OriginalUrl == url);
            if (existingUrl == null)
            {
                var newUrl = new URL
                {
                    OriginalUrl = url,
                    FriendlyUrl = friendlyUrl
                };
                _context.URLs.Add(newUrl);
                _context.SaveChanges();
            }

            return Json(new { friendlyUrl });
        }

        // Lấy danh sách Friendly URL đã lưu
        [HttpGet]
        public IActionResult GetSavedUrls()
        {
            var urls = _context.URLs.OrderByDescending(u => u.CreatedAt).ToList();
            return Json(urls);
        }
    }
}
