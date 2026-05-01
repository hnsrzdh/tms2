using System;
using System.Globalization;

namespace TMS.MVC.Infrastructure
{
    public static class JalaliDateParser
    {
        public static bool TryParseJalaliDate(string? jalali, out DateTime gregorian)
        {
            gregorian = default;

            if (string.IsNullOrWhiteSpace(jalali))
                return false;

            // قبول فرمت‌های 1404/1/5 و 1404/01/05
            var parts = jalali.Trim().Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3) return false;

            if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var jy)) return false;
            if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var jm)) return false;
            if (!int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var jd)) return false;

            if (jm < 1 || jm > 12) return false;
            if (jd < 1 || jd > 31) return false;

            try
            {
                var pc = new PersianCalendar();
                gregorian = pc.ToDateTime(jy, jm, jd, 0, 0, 0, 0);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}