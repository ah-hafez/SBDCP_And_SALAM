using System;
using System.Globalization;
using System.Text;

namespace MCS.Common
{
    public static class DateTimeUtility
    {
        static UmAlQuraCalendar _umAlQuraCalendar = null;

        static DateTimeUtility()
        {
            _umAlQuraCalendar = new UmAlQuraCalendar();
        }

        public static string ConvertToUmAlQuraCalendar(DateTime dateTime)
        {
            //TODO:Pass Date Format As Parameter 

            int year = _umAlQuraCalendar.GetYear(dateTime);
            int month = _umAlQuraCalendar.GetMonth(dateTime);
            int day = _umAlQuraCalendar.GetDayOfMonth(dateTime);

            return string.Format(year + "/" + month + "/" + day);
        }

        public static string ConvertToUmAlQuraCalendar_NewFormat(DateTime dateTime)
        {
            //TODO:Pass Date Format As Parameter 

            int year = _umAlQuraCalendar.GetYear(dateTime);
            int month = _umAlQuraCalendar.GetMonth(dateTime);
            int day = _umAlQuraCalendar.GetDayOfMonth(dateTime);

            return string.Format(day.ToString("00") + "/" + month.ToString("00") + "/" + year);
        }
        public static string ConvertToUmAlQuraCalendarWithTime(DateTime dateTime)
        {
            //TODO:Pass Date Format As Parameter 

            int year = _umAlQuraCalendar.GetYear(dateTime);
            int month = _umAlQuraCalendar.GetMonth(dateTime);
            int day = _umAlQuraCalendar.GetDayOfMonth(dateTime);

            return string.Format(dateTime.ToString("hh:mm tt") + "-" + year + "/" + month + "/" + day);
        }
        public static string ConvertToUmAlQuraCalendarFullFormat(DateTime dateTime)
        {
            //TODO:Pass Date Format As Parameter 

            int year = _umAlQuraCalendar.GetYear(dateTime);
            int month = _umAlQuraCalendar.GetMonth(dateTime);
            int day = _umAlQuraCalendar.GetDayOfMonth(dateTime);

            //return string.Format(day + "/" + month + "/" + year + "هـ " + dateTime.ToString("HH:mm"));
            //return string.Format(dateTime.ToString("HH:mm") + " - " + year + "/" + month + "/" + day + " هـ  ");

            return string.Format(  year + "/" + month + "/" + day + " هـ  " );
        }

        public static string ConvertToUmAlQuraCalendarViewedFormat(DateTime dateTime)
        {
            //TODO:Pass Date Format As Parameter 

            int year = _umAlQuraCalendar.GetYear(dateTime);
            int month = _umAlQuraCalendar.GetMonth(dateTime);
            int day = _umAlQuraCalendar.GetDayOfMonth(dateTime);

            //return string.Format(day + "/" + month + "/" + year + "هـ " + dateTime.ToString("HH:mm"));
            return string.Format(" " + dateTime.ToString("hh:mm tt") + year + "/" + month + "/" + day);
        }

        public static string ConvertToUmAlQuraCalendarAsNumber(DateTime dateTime)
        {
            int year = _umAlQuraCalendar.GetYear(dateTime);
            int month = _umAlQuraCalendar.GetMonth(dateTime);
            int day = _umAlQuraCalendar.GetDayOfMonth(dateTime);
            int hour = _umAlQuraCalendar.GetHour(dateTime);
            int minute = _umAlQuraCalendar.GetMinute(dateTime);
            int second = _umAlQuraCalendar.GetSecond(dateTime);

            StringBuilder dateAsNumber = new StringBuilder();

            dateAsNumber.Append(year).Append(month).Append(day).Append(hour).Append(minute).Append(second);

            return dateAsNumber.ToString();
        }

        public static int GetHijriYear(DateTime dateTime)
        {
            return _umAlQuraCalendar.GetYear(dateTime);
        }

        public static int GetHijriMonth(DateTime dateTime)
        {
            return _umAlQuraCalendar.GetMonth(dateTime);
        }

        public static DateTime ConvertToDate(string dateString)
        {
            DateTime dateValue;
            string formats = SystemConfigurations.DateFormats;
            string[] dateFormats = formats.Split(',');

            if (DateTime.TryParseExact(dateString, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
            {
                return dateValue;
            }

            return DateTime.MinValue;
        }
        public static DateTime HijriToGreg(string hijri)
        {
            string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
            "dd/MM/yyyy","d/M/yyyy",
            "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
            "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
            "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
            "yyyy M d","dd MM yyyy","d M yyyy",
            "dd M yyyy","d MM yyyy"};
            CultureInfo arCul = new CultureInfo("ar-SA");
            CultureInfo enCul = new CultureInfo("en-US");
            DateTime tempDate = DateTime.ParseExact(hijri, allFormats,
                   arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            return tempDate;

        }
    }
}
