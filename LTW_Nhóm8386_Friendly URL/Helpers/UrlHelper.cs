using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LTW_Nhóm8386_FriendlyURL.Helpers
{
    public static class UrlHelper
    {
        // Hàm chuyển đổi chuỗi thành dạng slug chuẩn: chữ thường, không dấu, dấu gạch ngang thay cho khoảng trắng/ ký tự đặc biệt.
        public static string GenerateFriendlyText(string url)
        {
            // Ví dụ: lấy phần đường dẫn từ URL
            Uri uri = new Uri(url);
            string path = uri.AbsolutePath;

            // Loại bỏ dấu tiếng Việt
            string normalized = RemoveDiacritics(path);

            // Thay các ký tự không hợp lệ bằng dấu gạch ngang
            string friendly = Regex.Replace(normalized, @"[^a-zA-Z0-9]+", "-").ToLower();

            // Loại bỏ dấu gạch ngang thừa ở đầu/cuối
            friendly = friendly.Trim('-');

            return friendly;
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
