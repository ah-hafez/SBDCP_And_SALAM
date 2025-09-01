using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static iTextSharp.text.pdf.events.IndexEvents;

namespace MCS.DoconutMVC.Helpers
{
    public static class DoconutHelper
    {
        public static byte[] AddWatermark(byte[] data, string watermarkText)
        {
            try
            {
                data = RemoveWatermark(data);
                //create pdfreader object to read sorce pdf
                PdfReader pdfReader = new PdfReader(data);
                //pdfReader.Catalog.Remove(PdfName.OCPROPERTIES);
                //create stream of filestream or memorystream etc. to create output file
                using (MemoryStream msOutput = new MemoryStream())
                {
                    //create pdfstamper object which is used to add addtional content to source pdf file
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, msOutput);

                    BaseFont bf = BaseFont.CreateFont(@"C:\Windows\Fonts\ARIAL.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font f = new Font(bf, 23);

                    var pdfLayers = pdfStamper.GetPdfLayers();

                    if (!pdfStamper.GetPdfLayers().Any(x => x.Key == "WatermarkLayer"))
                    {
                        PdfLayer layer = new PdfLayer("WatermarkLayer", pdfStamper.Writer);
                        for (int pageIndex = 1; pageIndex <= pdfReader.NumberOfPages; pageIndex++)
                        {
                            //Rectangle class in iText represent geomatric representation... in this case, rectanle object would contain page geomatry
                            Rectangle rect = pdfReader.GetPageSizeWithRotation(pageIndex);
                            //pdfcontentbyte object contains graphics and text content of page returned by pdfstamper
                            PdfContentByte cb = pdfStamper.GetOverContent(pageIndex);

                            cb.BeginLayer(layer);

                            PdfGState gState = new PdfGState();

                            gState.FillOpacity = 0.1f; // define opacity level
                            cb.SetGState(gState);

                            // set font size and style for layer water mark text to generate full page
                            cb.SetFontAndSize(BaseFont.CreateFont(
                                        BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12);

                            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(watermarkText, f), Convert.ToInt64(0.25 * rect.Width), Convert.ToInt64(0.1 * rect.Height), 45, PdfWriter.RUN_DIRECTION_RTL, 1);
                            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(watermarkText, f), Convert.ToInt64(0.25 * rect.Width), Convert.ToInt64(0.3 * rect.Height), 45, PdfWriter.RUN_DIRECTION_RTL, 1);
                            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(watermarkText, f), Convert.ToInt64(0.25 * rect.Width), Convert.ToInt64(0.5 * rect.Height), 45, PdfWriter.RUN_DIRECTION_RTL, 1);
                            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(watermarkText, f), Convert.ToInt64(0.25 * rect.Width), Convert.ToInt64(0.7 * rect.Height), 45, PdfWriter.RUN_DIRECTION_RTL, 1);
                            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(watermarkText, f), Convert.ToInt64(0.25 * rect.Width), Convert.ToInt64(0.9 * rect.Height), 45, PdfWriter.RUN_DIRECTION_RTL, 1);

                            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(watermarkText, f), Convert.ToInt64(0.75 * rect.Width), Convert.ToInt64(0.1 * rect.Height), 45, PdfWriter.RUN_DIRECTION_RTL, 1);
                            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(watermarkText, f), Convert.ToInt64(0.75 * rect.Width), Convert.ToInt64(0.3 * rect.Height), 45, PdfWriter.RUN_DIRECTION_RTL, 1);
                            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(watermarkText, f), Convert.ToInt64(0.75 * rect.Width), Convert.ToInt64(0.5 * rect.Height), 45, PdfWriter.RUN_DIRECTION_RTL, 1);
                            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(watermarkText, f), Convert.ToInt64(0.75 * rect.Width), Convert.ToInt64(0.7 * rect.Height), 45, PdfWriter.RUN_DIRECTION_RTL, 1);
                            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(watermarkText, f), Convert.ToInt64(0.75 * rect.Width), Convert.ToInt64(0.9 * rect.Height), 45, PdfWriter.RUN_DIRECTION_RTL, 1);

                            cb.EndLayer();
                        }
                    }
                    //iterate through all pages in source pdf


                    pdfStamper.Close();

                    return msOutput.ToArray();
                }
            }
            catch (Exception)
            {
                return data;
            }
        }

        //public static byte[] RemoveWatermark(byte[] data, string watermarkText)
        //{
        //    try
        //    {
        //        //create pdfreader object to read sorce pdf
        //        PdfReader pdfReader = new PdfReader(data);

        //        string guid = Guid.NewGuid().ToString("N");
        //        string workingFolder = ConfigurationManager.AppSettings["DocsPath"];
        //        string fullPath = workingFolder + guid + ".pdf";
        //        //create stream of filestream or memorystream etc. to create output file
        //        using (MemoryStream msOutput = new MemoryStream())
        //        {
        //            //create pdfstamper object which is used to add addtional content to source pdf file
        //            PdfStamper pdfStamper = new PdfStamper(pdfReader, msOutput);
        //            var pdfLayers = pdfStamper.GetPdfLayers();
        //            foreach (var layer in pdfLayers)
        //            {
        //                PdfLayer lay = (PdfLayer)layer.Value;
        //                lay.On = false;

        //            }

        //            //pdfLayers = pdfStamper.GetPdfLayers();
        //            //if (pdfLayers != null && pdfLayers.Count > 0)
        //            //{
        //            //    pdfStamper.GetPdfLayers().Clear();
        //            //}
        //            //if (pdfStamper.GetPdfLayers().Any(x => x.Key == "WatermarkLayer"))
        //            //{
        //            //    pdfStamper.GetPdfLayers().Clear();
        //            //}
        //            //iterate through all pages in source pdf

        //            pdfStamper.Close();
        //            System.IO.File.WriteAllBytes(fullPath, msOutput.ToArray());
        //            return msOutput.ToArray();



        //        }






        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public static byte[] RemoveWatermark(byte[] data)
        {
            PdfReader pdfReader = new PdfReader(data);

            string guid = Guid.NewGuid().ToString("N");
            string workingFolder = ConfigurationManager.AppSettings["DocsPath"];
            string fullPath = workingFolder + guid + ".pdf";


            //NOTE, This will destroy all layers in the document, only use if you don't have additional layers
            //Remove the OCG group completely from the document.
            //reader2.Catalog.Remove(PdfName.OCPROPERTIES);
            pdfReader.Catalog.Remove(PdfName.OCPROPERTIES);
            //Clean up the reader, optional
            pdfReader.RemoveUnusedObjects();

            //Placeholder variables
            PRStream stream;
            String content;
            PdfDictionary page;
            PdfArray contentarray;

            //Get the page count
            int pageCount2 = pdfReader.NumberOfPages;
            //Loop through each page
            for (int i = 1; i <= pageCount2; i++)
            {
                //Get the page
                page = pdfReader.GetPageN(i);
                //Get the raw content
                contentarray = page.GetAsArray(PdfName.CONTENTS);
                if (contentarray != null)
                {
                    //Loop through content
                    for (int j = 0; j < contentarray.Size; j++)
                    {
                        //Get the raw byte stream
                        stream = (PRStream)contentarray.GetAsStream(j);
                        //Convert to a string
                        content = System.Text.Encoding.ASCII.GetString(PdfReader.GetStreamBytes(stream));
                        //Look for the OCG token in the stream as well as our watermarked text
                        if (content.IndexOf("/OC") >= 0)
                        {
                            //Remove it by giving it zero length and zero data
                            stream.Put(PdfName.LENGTH, new PdfNumber(0));
                            stream.SetData(new byte[0]);
                        }
                    }
                }
            }
            using (MemoryStream msOutput = new MemoryStream())
            {
                PdfStamper pdfStamper = new PdfStamper(pdfReader, msOutput);
                pdfStamper.Close();
                return msOutput.ToArray();
            }
            //    //Write the content out
            //    using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    using (PdfStamper stamper = new PdfStamper(pdfReader, fs))
            //    {

            //    }
            //}

            //return System.IO.File.ReadAllBytes(fullPath);
        }

        public static byte[] deletePage(byte[] pdf, int pageNo, string watermarkText)
        {
            byte[] all;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.SetPageSize(PageSize.A4);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;
                var newContent = RemoveWatermark(pdf);
                PdfReader reader;
                reader = new PdfReader(newContent);
                int pages = reader.NumberOfPages;

                for (int i = 1; i <= pages; i++)
                {
                    if (i != pageNo)
                    {
                        page = writer.GetImportedPage(reader, i);
                        var pgsize = new Rectangle(page.Width, page.Height);
                        doc.SetPageSize(pgsize);
                        doc.NewPage();
                        cb.AddTemplate(page, 0, 0);
                    }
                }
                doc.Close();
                all = ms.GetBuffer();
                ms.Flush();
                ms.Dispose();

                //all = AddWatermark(all, watermarkText);
            }
            return all;
        }

        public static byte[] deletAll(byte[] pdf, int pageNo)
        {
            byte[] all;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.SetPageSize(PageSize.A4);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                PdfReader reader;
                reader = new PdfReader(pdf);
                int pages = reader.NumberOfPages;

                for (int i = 1; i <= pages; i++)
                {
                    if (i != pageNo)
                    {
                        page = writer.GetImportedPage(reader, i);
                        var pgsize = new Rectangle(page.Width, page.Height);
                        doc.SetPageSize(pgsize);
                        doc.NewPage();
                        cb.AddTemplate(page, 0, 0);
                    }
                }
                doc.Close();
                all = ms.GetBuffer();
                ms.Flush();
                ms.Dispose();
            }
            return all;
        }

        public static byte[] concatAndAddContent(List<byte[]> pdf, string watermarkText)
        {
            if (pdf.Count == 1)
                return pdf[0];

            byte[] all;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.SetPageSize(PageSize.A4);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                PdfReader reader;
                foreach (byte[] p in pdf)
                {
                    var newContent = RemoveWatermark(p);
                    reader = new PdfReader(newContent);
                    int pages = reader.NumberOfPages;

                    // loop over document pages
                    for (int i = 1; i <= pages; i++)
                    {
                        page = writer.GetImportedPage(reader, i);
                        var pgSize = new Rectangle(page.Width, page.Height);
                        doc.SetPageSize(pgSize);
                        doc.NewPage();
                        cb.AddTemplate(page, 0, 0);
                    }
                }

                doc.Close();
                all = ms.GetBuffer();
                //if (!string.IsNullOrWhiteSpace(watermarkText))
                //    all = AddWatermark(all, watermarkText);
                ms.Flush();
                ms.Dispose();
            }
            return all;
        }

        public static byte[] ReplacePage(byte[] pdf, byte[] AddedDocumet, int pageNo, string watermarkText)
        {
            byte[] all;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.SetPageSize(PageSize.A4);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                PdfReader reader;
                var newContent = RemoveWatermark(pdf);
                reader = new PdfReader(newContent);
                int pages = reader.NumberOfPages;

                PdfReader NewR;
                NewR = new PdfReader(AddedDocumet);

                // loop over document pages
                for (int i = 1; i <= pages; i++)
                {
                    if (i != (pageNo + 1))// || pages == 1)
                    {
                        page = writer.GetImportedPage(reader, i);
                        var pgSize = new Rectangle(page.Width, page.Height);
                        doc.SetPageSize(pgSize);
                        doc.NewPage();
                        cb.AddTemplate(page, 0, 0);
                    }
                    else
                    {
                        page = writer.GetImportedPage(NewR, 1);
                        var pgSize = new Rectangle(page.Width, page.Height);
                        doc.SetPageSize(pgSize);
                        doc.NewPage();
                        cb.AddTemplate(page, 0, 0);
                    }
                }

                doc.Close();
                all = ms.GetBuffer();
                //all = AddWatermark(all, watermarkText);
                ms.Flush();
                ms.Dispose();
            }

            return all;
        }

        public static byte[] MovePrev(byte[] pdf, byte[] AddedDocumet, int pageNo, string watermarkText)
        {
            byte[] all;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.SetPageSize(PageSize.A4);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                PdfReader reader;
                var newContent = RemoveWatermark(pdf);
                reader = new PdfReader(newContent);
                int pages = reader.NumberOfPages;

                PdfReader NewR;
                NewR = new PdfReader(AddedDocumet);

                // loop over document pages
                for (int i = 1; i <= pages; i++)
                {
                    if (pageNo != i && pageNo != 0)
                    {
                        page = writer.GetImportedPage(reader, i);
                        var pgSize = new Rectangle(page.Width, page.Height);
                        doc.SetPageSize(pgSize);
                        doc.NewPage();
                        cb.AddTemplate(page, 0, 0);
                    }
                    else if (pageNo > 0)
                    {
                        page = writer.GetImportedPage(reader, i);
                        var pgSize = new Rectangle(page.Width, page.Height);
                        doc.SetPageSize(pgSize);
                        doc.NewPage();
                        cb.AddTemplate(page, 0, 0);
                        page = writer.GetImportedPage(NewR, 1);
                        var pgSize1 = new Rectangle(page.Width, page.Height);
                        doc.SetPageSize(pgSize1);
                        doc.NewPage();

                        cb.AddTemplate(page, 0, 0);
                    }
                    else if (pageNo == 0 && i == 1)
                    {
                        for (int j = 0; j < NewR.NumberOfPages; j++)
                        {
                            page = writer.GetImportedPage(NewR, j + 1);
                            var pgSize1 = new Rectangle(page.Width, page.Height);
                            doc.SetPageSize(pgSize1);
                            doc.NewPage();
                            cb.AddTemplate(page, 0, 0);
                        }
                        page = writer.GetImportedPage(reader, i);
                        var pgSize = new Rectangle(page.Width, page.Height);
                        doc.SetPageSize(pgSize);
                        doc.NewPage();
                        cb.AddTemplate(page, 0, 0);
                    }
                    else
                    {
                        page = writer.GetImportedPage(reader, i);
                        var pgSize = new Rectangle(page.Width, page.Height);
                        doc.SetPageSize(pgSize);
                        doc.NewPage();
                        cb.AddTemplate(page, 0, 0);
                    }
                }
                doc.Close();
                all = ms.GetBuffer();
                //all = AddWatermark(all, watermarkText);
                ms.Flush();
                ms.Dispose();
            }
            return all;
        }

        public static byte[] MoveNext(byte[] pdf, byte[] AddedDocumet, int pageNo, string watermarkText)
        {
            byte[] all;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.SetPageSize(PageSize.A4);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;
                var newContent = RemoveWatermark(pdf);
                PdfReader reader;
                reader = new PdfReader(newContent);
                int pages = reader.NumberOfPages;

                PdfReader NewR;
                NewR = new PdfReader(AddedDocumet);

                // loop over document pages
                for (int i = 1; i <= pages; i++)
                {
                    if ((pageNo + 1) != i)
                    {
                        page = writer.GetImportedPage(reader, i);
                        var pgSize = new Rectangle(page.Width, page.Height);
                        doc.SetPageSize(pgSize);
                        doc.NewPage();
                        cb.AddTemplate(page, 0, 0);
                    }
                    else
                    {
                        page = writer.GetImportedPage(reader, i);
                        var pgSize = new Rectangle(page.Width, page.Height);
                        doc.SetPageSize(pgSize);
                        doc.NewPage();
                        cb.AddTemplate(page, 0, 0);

                        for (int j = 0; j < NewR.NumberOfPages; j++)
                        {
                            page = writer.GetImportedPage(NewR, j + 1);
                            var pgSize1 = new Rectangle(page.Width, page.Height);
                            doc.SetPageSize(pgSize1);
                            doc.NewPage();
                            cb.AddTemplate(page, 0, 0);
                        }
                    }
                }
                doc.Close();
                all = ms.GetBuffer();
                //all = AddWatermark(all, watermarkText);
                ms.Flush();
                ms.Dispose();
            }
            return all;
        }

        public static byte[] MovePageUp(byte[] pdf, int pageNo)
        {
            byte[] all;

            //1
            //  0  1  2
            // { 1, 2, 3 }
            PdfReader reader;
            reader = new PdfReader(pdf);
            int pages = reader.NumberOfPages;
            int[] order = Enumerable.Range(1, pages).ToArray();
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(reader.GetPageSizeWithRotation(1)))
                {
                    //Use a PdfCopy to duplicate each page
                    using (PdfCopy copy = new PdfCopy(doc, ms))
                    {
                        doc.Open();
                        copy.SetLinearPageMode();
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            if (order[i - 1] == pageNo)
                            {
                                int x = order[i - 1];
                                order[i - 1] = order[i - 2];
                                order[i - 2] = x;

                            }

                            copy.AddPage(copy.GetImportedPage(reader, i));
                        }
                        //Reorder pages
                        copy.ReorderPages(order);
                        doc.Close();
                        all = ms.GetBuffer();
                        ms.Flush();
                        ms.Dispose();
                    }
                }
            }

            return all;


        }
        public static byte[] MovePageDown(byte[] pdf, int pageNo)
        {
            byte[] all;

            //1
            //  0  1  2
            // { 1, 2, 3 }
            PdfReader reader;
            reader = new PdfReader(pdf);
            int pages = reader.NumberOfPages;
            int[] order = Enumerable.Range(1, pages).ToArray();
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(reader.GetPageSizeWithRotation(1)))
                {
                    //Use a PdfCopy to duplicate each page
                    using (PdfCopy copy = new PdfCopy(doc, ms))
                    {
                        doc.Open();
                        copy.SetLinearPageMode();
                        bool ischange = false;
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {

                            if (order[i - 1] == pageNo & !ischange)
                            {
                                int x = order[i - 1];
                                order[i - 1] = order[i];
                                order[i] = x;
                                ischange = true;
                            }

                            copy.AddPage(copy.GetImportedPage(reader, i));
                        }
                        //Reorder pages
                        copy.ReorderPages(order);
                        doc.Close();
                        all = ms.GetBuffer();
                        ms.Flush();
                        ms.Dispose();
                    }
                }
            }

            return all;


        }

    }
}