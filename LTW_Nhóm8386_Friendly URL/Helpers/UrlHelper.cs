using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LTW_Nhóm8386_FriendlyURL.Helpers
{
    public static class UrlHelper
    {
        // Chuẩn hóa URL hợp lệ
        public static string NormalizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL không hợp lệ!");
            }

            // Kiểm tra xem URL có http/https không
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "https://" + url;
            }

            // Kiểm tra tính hợp lệ của URL
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                throw new ArgumentException("URL không hợp lệ!");
            }

            return uri.ToString();
        }

        // Chuyển đổi toàn bộ URL thành Friendly Slug
        public static string GenerateFriendlySlug(string url)
        {
            Uri uri = new Uri(url);
            string fullPath = $"{uri.Host}{uri.AbsolutePath}".Trim('/');

            if (string.IsNullOrEmpty(fullPath))
            {
                fullPath = "trang-chu"; // Nếu trống, đặt mặc định là "trang-chu"
            }

            string normalized = RemoveDiacritics(fullPath);
            string friendly = Regex.Replace(normalized, "[^a-zA-Z0-9]+", "-").ToLower();
            return friendly.Trim('-');
        }

        private static string RemoveDiacritics(string text)
        {
            text = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
