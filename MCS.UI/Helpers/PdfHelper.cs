using iTextSharp.text;
using iTextSharp.text.pdf;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PdfDocument = SelectPdf.PdfDocument;

namespace MCS.UI.Helpers
{
    public static class PdfHelper
    {
        public static byte[] ConvertHtml2PDF(string content)
        {
            HtmlToPdf converter = new HtmlToPdf(); 
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.MaxPageLoadTime = 120; 
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(content, string.Empty);
            return doc.Save();

        }
        public static byte[] ConvertHtml2PDFExp(string content)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.MaxPageLoadTime = 120;
            converter.Options.MarginTop = 100;
            converter.Options.MarginLeft = 100;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(content, string.Empty);
            return doc.Save();

        }
        public static byte[] ConcatenateAndAddContent(List<byte[]> pdfs)
        {
            if (pdfs.Count == 1)
                return pdfs[0];

            byte[] result;

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document();

                PdfCopy copy = new PdfCopy(document, ms);
                document.Open();

                foreach (byte[] pdf in pdfs)
                {
                    PdfReader reader = new PdfReader(pdf);

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        copy.AddPage(copy.GetImportedPage(reader, i));
                    }

                    reader.Close();
                }

                document.Close();
                result = ms.ToArray();
            }

            return result;
        }


        public static byte[] ConvertHtml2PDF_2(string content)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.MaxPageLoadTime = 120;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            PdfDocument doc = converter.ConvertHtmlString(content, string.Empty);
            return doc.Save();

        }
    }
}