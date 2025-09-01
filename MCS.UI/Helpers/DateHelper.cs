using System;
using System.Globalization;
using MCS.Framework.Controls;
using MCS.Common;
using MCS.UI.Areas.Admin.Models.Shared;

namespace MCS.UI.Helpers
{
    public static class DateHelper
    {
        public static string DateCalendar(DateTime DateConv, string DateLangCulture,bool WithDate=false)
        {
            DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();
            SettingVM dateType = SessionInfo.GetObjectFromSession(Constants.SettingDate) as SettingVM;
            var settingDateType = Convert.ToInt32(dateType.Value);

            if (settingDateType == DateType.Ummalqura.LookupIdentity(LookupCategory.DateType, DateLangCulture) && DateLangCulture.StartsWith("en"))
            {
                DateLangCulture = "ar-sa";
            }

            /// Set the date time format to the given culture
            DTFormat = new CultureInfo(DateLangCulture, false).DateTimeFormat;

            /// Set the calendar property of the date time format to the given calendar
            //switch (settingDateType.LookupInternalID(LookupCategory.DateType, DateLangCulture))
            //{
            //    case (int)DateType.Ummalqura:
            //        DTFormat.Calendar = new HijriCalendar();
            //        break;

            //    case (int)DateType.Gregorian:
            //        DTFormat.Calendar = new GregorianCalendar();
            //        break;
            //    default:
            //        return "";
            //}

            DTFormat.Calendar = new HijriCalendar();

            /// We format the date structure to whatever we want
            DTFormat.ShortDatePattern = "dd/MM/yyyy";
            if (WithDate == true)
            {
                return DateConv.Date.ToString("dd/MM/yyyy" + " " + DateConv.ToShortTimeString(), DTFormat);
            }
            else
            {
                return DateConv.Date.ToString("dd/MM/yyyy", DTFormat);
                
            }
            
        }
        public static string ConvertDateCalendar(DateTime DateConv, CalenderType Calendar, string DateLangCulture)
        {
            DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();
            /// We can't have the hijri date writen in English. We will get a runtime error

            if (Calendar == CalenderType.Ummalqura && DateLangCulture.StartsWith("en"))
            {
                DateLangCulture = "ar-sa";
            }

            /// Set the date time format to the given culture
            DTFormat = new CultureInfo(DateLangCulture, false).DateTimeFormat;

            /// Set the calendar property of the date time format to the given calendar
            switch (Calendar)
            {
                case CalenderType.Ummalqura:
                    DTFormat.Calendar = new HijriCalendar();
                    break;

                case CalenderType.Gregorian:
                    DTFormat.Calendar = new GregorianCalendar();
                    break;
                default:
                    return "";
            }

            /// We format the date structure to whatever we want
            DTFormat.ShortDatePattern = "dd/MM/yyyy";
            return DateConv.Date.ToString("dd/MM/yyyy", DTFormat);
        }
        public static string ConvertDate(DateTime DateConv)
        {

            SettingVM dateType = SessionInfo.GetObjectFromSession(Constants.SettingDate) as SettingVM;
            var settingDateType = Convert.ToInt32(dateType.Value);
            var DateValue = (int)DateType.Gregorian.LookupIdentity(LookupCategory.DateType, string.Empty);

            CultureInfo arSA = new CultureInfo("ar-SA");
            arSA.DateTimeFormat.Calendar = new HijriCalendar();

            //datetime = DateTime.ParseExact(datetime, "dd/M/yyyy", arSA).ToShortDateString();

            DateTimeFormatInfo DTFormat;


            /// Set the date time format to the given culture
            DTFormat = new CultureInfo(arSA.ToString(), false).DateTimeFormat;

            /// Set the calendar property of the date time format to the given calendar
            switch (settingDateType.LookupInternalID(LookupCategory.DateType, string.Empty))
            {
                case (int)DateType.Ummalqura:
                    DTFormat.Calendar = new HijriCalendar();
                    break;

                case (int)DateType.Gregorian:

                    DTFormat.Calendar = new GregorianCalendar();
                    break;
                default:
                    return "";
            }

            /// We format the date structure to whatever we want
            DTFormat.ShortDatePattern = "dd/MM/yyyy";
            return DateConv.Date.ToString("dd/MM/yyyy", DTFormat);
        }
        public static string GetDate(string datetime)
        {
            if (datetime == null || datetime == string.Empty)
            {
                return "";
            }
            else
            {
                SettingVM dateType = SessionInfo.GetObjectFromSession(Constants.SettingDate) as SettingVM;
                int? settingDateType = dateType != null && dateType.Value != null ? Convert.ToInt32(dateType.Value) : (int?)null;
                var DateValue = DateType.Gregorian.LookupIdentity(LookupCategory.DateType, string.Empty);
                if (settingDateType != DateValue)
                {
                    return datetime;
                }
                else
                {
                    CultureInfo arSA = new CultureInfo("ar-SA");
                    arSA.DateTimeFormat.Calendar = new HijriCalendar();

                    datetime = DateTime.ParseExact(datetime, "d/M/yyyy", arSA).ToShortDateString();

                    return datetime;
                }
            }
        }
    }
}