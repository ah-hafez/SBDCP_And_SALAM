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

namespace MCS.Framework.Controls
{
    public enum CalenderType
    {
        Gregorian,
        Ummalqura
    }

    public static class Calendar
    {
        public static string RenderCalendar(string calendarControlId, string hdnIdToSaveGregorianDate, string hdnIdToSaveUmmalquraDate, CalenderType calendarType, string languageShortName, string defaultDate = "", string className = "")
        {
            StringBuilder controlStream = new StringBuilder();

            if (className != string.Empty)
            {
                controlStream.AppendFormat("<input type='text' onkeypress = 'return CalenderIsDate(event);' id='{0}' maxlength='20' class='{1}' name='{2}' />", calendarControlId, className, calendarType.ToString().ToLower());
            }
            else
            {
                controlStream.AppendFormat("<input type='text' onkeypress = 'return CalenderIsDate(event);' maxlength='20' id='{0}' name='{2}' />", calendarControlId, calendarType.ToString().ToLower());
            }

            controlStream.AppendFormat("<script> Calender('{0}', '{1}', '{2}', '{3}', '{4}', '{5}'); </script>", calendarControlId, hdnIdToSaveGregorianDate, hdnIdToSaveUmmalquraDate, calendarType.ToString().ToLower(), languageShortName.ToLower(), defaultDate);

            return controlStream.ToString();
        }

        public static string RenderCalendarResources()
        {
            StringBuilder controlStream = new StringBuilder();

            string calImagWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.Calendar.Images.calImg.gif", typeof(Calendar));

            controlStream.Append("<style > .is-calendarsPicker + img { position: relative; left: -24px; top: 0px; } </style>");
            controlStream.AppendFormat("<div style='display: none;'> <img id='calImg' src='{0}' alt='Popup' class='trigger'> </div>", calImagWebResourceUrl);

            //string a = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.Calendar.JS.jquery.calendars.js", typeof(Calendar));
            //string b = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.Calendar.JS.jquery.calendars.plus.js", typeof(Calendar));
            //string c = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.Calendar.JS.jquery.calendars.picker.js", typeof(Calendar));
            //string d = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.Calendar.JS.jquery.calendars.picker-ar.js", typeof(Calendar));
            //string e = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.Calendar.JS.jquery.calendars.ummalqura.js", typeof(Calendar));
            //string f = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.Calendar.JS.jquery.calendars.ummalqura-ar.js", typeof(Calendar));
            //string g = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.Calendar.JS.jquery.calendars-ar.js", typeof(Calendar));
            //string h = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.Calendar.JS.CalendarScript.js", typeof(Calendar));

            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", b);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", c);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", d);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", e);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", f);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", g);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", h);


            string js = Scripts.Render("~/MCS.Framework.Controls/JSCalendar").ToString();

            //string js = Scripts.Render("~/MCS.Framework.Controls/JSCalendar").ToString();
            string css = Styles.Render("~/MCS.Framework.Controls/StylesCalendar").ToString();

           return controlStream.ToString() + js + css;
        }

        public static string DateConverter(string date, CalenderType from, CalenderType to, string hdnIdToStoreTheConvertedDate)
        {
            StringBuilder controlStream = new StringBuilder();

            controlStream.AppendFormat("DateConverter('{0}', '{1}', '{2}', '{3}');", date, from.ToString().ToLower(), to.ToString().ToLower(), hdnIdToStoreTheConvertedDate);

            return controlStream.ToString();
        }
    }
}
