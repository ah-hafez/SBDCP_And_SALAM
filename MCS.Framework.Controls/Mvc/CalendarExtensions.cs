using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Handlers;
using System.Web.Mvc;
using System.Web.Optimization;

#region [ Resources ]

[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Calendar.JS.jquery.calendars.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Calendar.JS.jquery.calendars.plus.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Calendar.JS.jquery.calendars.picker.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Calendar.JS.jquery.calendars.picker-ar.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Calendar.JS.jquery.calendars.ummalqura.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Calendar.JS.jquery.calendars.ummalqura-ar.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Calendar.JS.jquery.calendars-ar.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Calendar.JS.CalendarScript.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Calendar.Styles.jquery.calendars.picker.css", "text/css")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Calendar.Images.calImg.gif", "img/gif")]

#endregion [ Resources ]

namespace MCS.Framework.Controls.Mvc
{
    public static class CalendarExtensions
    {
        public static MvcHtmlString Calendar(this HtmlHelper html, string CalendarControlId, string hdnIdToSaveGregorianDate, string hdnIdToSaveUmmalquraDate, CalenderType calendarName, string languageShortName, string defaultDate = "", string className = "")
        {
            return MvcHtmlString.Create(Framework.Controls.Calendar.RenderCalendar(CalendarControlId, hdnIdToSaveGregorianDate, hdnIdToSaveUmmalquraDate, calendarName, languageShortName, defaultDate, className));
        }

        public static MvcHtmlString RenderCalendarResources(this HtmlHelper html)
        {
            return MvcHtmlString.Create(Framework.Controls.Calendar.RenderCalendarResources());
        }

        public static MvcHtmlString DateConverter(this HtmlHelper html, string date, CalenderType from, CalenderType to, string hdnIdToStoreTheConvertedDate)
        {
            return MvcHtmlString.Create(Framework.Controls.Calendar.DateConverter(date, from, to, hdnIdToStoreTheConvertedDate));
        }
    }
}
