using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LTW_Nhóm8386_FriendlyURL.Helpers
{
    public static class UrlHelper
    {
        public static string GenerateFriendlyUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            // Phân tích URL và lấy phần đường dẫn
            Uri uri = new Uri(url);
            string path = uri.AbsolutePath;

            // Chuyển đổi tiếng Việt có dấu thành không dấu
            string normalized = RemoveDiacritics(path);

            // Loại bỏ ký tự đặc biệt: chỉ giữ lại chữ cái, số, dấu gạch ngang và dấu '/'
            string friendlyPath = Regex.Replace(normalized, @"[^a-zA-Z0-9/-]", "-").ToLower();

            // Chuẩn hóa dấu gạch ngang liên tiếp
            friendlyPath = Regex.Replace(friendlyPath, @"-+", "-").Trim('-');

            return $"{uri.Scheme}://{uri.Host}/{friendlyPath}";
        }

        private static string RemoveDiacritics(string text)
        {
            text = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
