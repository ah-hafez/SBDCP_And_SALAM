using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YESSER.NCS.MCS.Service.Helpers
{
    public static class PdfHelper
    {
        public static byte[] ConvertHtml2PDF(string content)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            PdfDocument doc = converter.ConvertHtmlString(content, string.Empty);
            return doc.Save();
        }
    }
}