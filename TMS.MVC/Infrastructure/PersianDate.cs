using System;
using System.Globalization;

namespace TMS.MVC.Infrastructure
{
    public static class PersianDate
    {
        private static readonly PersianCalendar Pc = new PersianCalendar();

        public static string ToPersian(DateTime? dt)
        {
            if (!dt.HasValue) return "—";
            return ToPersian(dt.Value);
        }

        public static string ToPersian(DateTime dt)
        {
            var y = Pc.GetYear(dt);
            var m = Pc.GetMonth(dt);
            var d = Pc.GetDayOfMonth(dt);
            return string.Format("{0:0000}/{1:00}/{2:00}", y, m, d);
        }

        public static string ToPersianWithTime(DateTime? dt)
        {
            if (!dt.HasValue) return "—";
            var d = dt.Value;
            return ToPersian(d) + " " + d.ToString("HH:mm");
        }
    }
}
