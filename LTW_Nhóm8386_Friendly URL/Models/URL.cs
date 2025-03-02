using System;
using System.ComponentModel.DataAnnotations;

namespace LTW_Nhóm8386_FriendlyURL.Models
{
    public class URL
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "URL Gốc")]
        public string OriginalUrl { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Friendly URL")]
        public string FriendlyUrl { get; set; } = string.Empty; // Lưu slug

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
