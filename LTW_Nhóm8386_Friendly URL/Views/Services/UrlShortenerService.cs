using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using LTW_Nhóm8386_FriendlyURL.Models;
using Microsoft.Extensions.Logging;

namespace LTW_Nhóm8386_FriendlyURL.Services
{
    public class UrlShortenerService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UrlShortenerService> _logger;

        public UrlShortenerService(ApplicationDbContext context, ILogger<UrlShortenerService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GenerateShortUrl(string originalUrl)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(originalUrl));
            var shortUrl = Convert.ToBase64String(hash).Substring(0, 6).Replace("/", "_").Replace("+", "-");

            return shortUrl;
        }

        public string ShortenUrl(string originalUrl)
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                throw new ArgumentException("URL không được để trống!");
            }

            var existing = _context.UrlShorteners.FirstOrDefault(u => u.OriginalUrl == originalUrl);
            if (existing != null) return existing.ShortUrl;

            var shortUrl = GenerateShortUrl(originalUrl);
            var newUrl = new UrlShortener { OriginalUrl = originalUrl, ShortUrl = shortUrl };

            _context.UrlShorteners.Add(newUrl);
            _context.SaveChanges();

            return shortUrl;
        }


        public string GetOriginalUrl(string shortUrl)
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
            {
                _logger.LogError("Short URL không hợp lệ!");
                throw new ArgumentException("Short URL không được để trống.");
            }

            
            var record = _context.UrlShorteners.FirstOrDefault(u => u.ShortUrl == shortUrl);

            if (record == null)
            {
                _logger.LogWarning($"Không tìm thấy URL với shortUrl: {shortUrl}");
                return null;
            }

            return record.OriginalUrl;
        }
    }
}
