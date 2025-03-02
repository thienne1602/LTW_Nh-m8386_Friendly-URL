using System.ComponentModel.DataAnnotations;

namespace LTW_Nhóm8386_FriendlyURL.Models
{
    public class UrlShortener
    {
        public int Id { get; set; }
        public string? OriginalUrl { get; set; }  // Thêm dấu '?' để cho phép null
        public string? ShortUrl { get; set; }     // Thêm dấu '?' để cho phép null
    }

}
