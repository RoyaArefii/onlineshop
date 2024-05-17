using System.Globalization;

namespace PublicTools.Tools
{
    public static class Helpers
    {
        #region [- GetRandomNumber(int length) ]
        public static long GetRandomNumber(int length)
        {
            return long.Parse(new string((from s in Enumerable.Repeat("123456789", length)
                                          select s[new Random().Next(9)]).ToArray()));
        }
        #endregion

        public static string ConvertToPersianDate(DateTime dateTime)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return string.Format(@"{0}/{1}/{2}",persianCalendar.GetYear(dateTime), persianCalendar.GetMonth(dateTime),persianCalendar.GetDayOfMonth(dateTime));
        }
    }
}
