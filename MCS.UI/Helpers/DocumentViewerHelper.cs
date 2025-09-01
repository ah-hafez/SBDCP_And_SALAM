using FileSignatures.Formats;
using MCS.Common;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using System.Configuration;
using System.IO;
using Spire.Doc;
using Document = Spire.Doc.Document;
using Microsoft.Ajax.Utilities;
using MCS.DTO;
using System;
using MCS.Framework.Logging;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using MCS.UI.Helpers;
using System.Text;
using Org.BouncyCastle.Crypto.Paddings;
using System.Web.UI.WebControls;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;

namespace MCS.UI
{
    public class DocumentViewerHelper
    {


        protected static string ArchivingPDFPath = "ArchivingPDF";

        public static byte[] GetPDFFile(string token, bool? burnAnnotations = null)
        {
            token = StringUtility.ClearTokenInput(token);
            var docViewer = new DotnetDaddy.DocumentViewer.DocViewer
            {
                Token = token
            };

            bool burnAnnotation = false;

            if ((!burnAnnotations.HasValue || (burnAnnotations.HasValue && burnAnnotations == true)) && !string.IsNullOrEmpty(docViewer.GetAnnotationData()))
            {
                burnAnnotation = true;
            }

            return docViewer.ExportToPdf(burnAnnotation);
        }
        public static string FullPath(string OfficeOnlineFileGuid)
        {
            string newPath = ConfigurationManager.AppSettings["DocsPath"] + OfficeOnlineFileGuid + ".docx";
            return newPath;
        }
        public static string ImgFullPath(string imgFileGuid)
        {
            string newPath = ConfigurationManager.AppSettings["DocsPath"] + imgFileGuid + ".png";
            return newPath;
        }

        public static byte[] GetOfficeFile(string OfficeOnlineFileGuid)
        {
            System.Threading.Thread.Sleep(1000);

            return File.ReadAllBytes(FullPath(OfficeOnlineFileGuid));
        }
        public static void WriteOfficeFile(byte[] content, string OfficeOnlineFileGuid)
        {
            File.WriteAllBytes(FullPath(OfficeOnlineFileGuid), content);

        }
        public static void WriteImgFile(byte[] content, string imgFileGuid)
        {
            File.WriteAllBytes(ImgFullPath(imgFileGuid), content);

        }
        public static void DeleteOfficeFile(string OfficeOnlineFileGuid)
        {

            File.Delete(FullPath(OfficeOnlineFileGuid));
        }
        public static string ConvertToPDF(byte[] buffer)
        {


            byte[] pdfData = null;
            string bodyContent = UnicodeEncoding.UTF8.GetString(buffer);

            var fileName = System.AppContext.BaseDirectory + @"Tool\Temp\" + Guid.NewGuid().ToString() + ".doc";

            string newFileName = fileName.Replace(".doc", ".pdf");
            File.WriteAllText(fileName, bodyContent);

            ProcessAsUser.Launch(System.AppContext.BaseDirectory + @"Tool\DocWordToPDF.exe " + @fileName);

            if (File.Exists(newFileName))
            {
                pdfData = File.ReadAllBytes(newFileName);
                File.Delete(newFileName);
                File.Delete(fileName);
                return Convert.ToBase64String(pdfData);

            }
            return null;



        }
        public static string CreatePDF(string path, string exportDir)
        {
            string pdfPath = "";

            try
            {

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, SystemConfigurations.OfficeUrl + "api/Document/CreatePDF?fileName=" + path + "&exportDir=" + exportDir);
                var response = client.SendAsync(request).Result;
                var responseMessage = response.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var pdfFileName = Path.ChangeExtension(path, ".pdf");
                pdfPath = Path.Combine(exportDir, pdfFileName);
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex);

            }
            finally
            {


            }

            return pdfPath;
        }
        public static byte[] ConvertXmlToDocx(byte[] content)
        {
            Document document = new Document();
            Stream stream = new MemoryStream(content);
            document.LoadFromStream(stream, FileFormat.WordXml);
            MemoryStream outputStream = new MemoryStream();
            document.SaveToFile(outputStream, FileFormat.Docx2013);
            return outputStream.ToArray();
        }
        //public static string ConvertDocToDocx(string fileName, byte[] content)
        //{

        //    var TempStorgepath = System.Configuration.ConfigurationManager.AppSettings["WordAddInStoragePath"];
        //    var path = TempStorgepath + fileName;
        //    System.IO.File.WriteAllBytes(path, content);
        //    Application app = new Application();
        //    app.DisplayAlerts = WdAlertLevel.wdAlertsNone;
        //    app.Visible = false;
        //    var oldPath = path;
        //    var objPresSet = app.Documents;
        //    var objPres = objPresSet.Open(path, MsoTriState.msoFalse, MsoTriState.msoCTrue, MsoTriState.msoFalse);
        //    string newFileName = path.Replace(".doc", ".docx");
        //    if (System.IO.File.Exists(newFileName))
        //    {
        //        System.IO.File.Delete(newFileName);
        //    }

        //    try
        //    {
        //        objPres.SaveAs2(newFileName, WdSaveFormat.wdFormatXMLDocument,
        //                     CompatibilityMode: WdCompatibilityMode.wdWord2013);

        //    }
        //    catch
        //    {
        //        newFileName = null;
        //    }
        //    finally
        //    {
        //        objPres.Close();
        //        ((_Application)app).Quit();
        //        File.Delete(oldPath);
        //    }



        //    return newFileName;
        //}

        public static byte[] ConvertDocToDocx(string fileName, byte[] content)
        {

            var TempStorgepath = System.Configuration.ConfigurationManager.AppSettings["WordAddInStoragePath"];
            var path = TempStorgepath + fileName;
            byte[] pdfFile = null;
            System.IO.File.WriteAllBytes(path, content);
            string newFileName = path.Replace(".doc", ".docx");

            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, SystemConfigurations.OfficeUrl + "/api/Document/ConvertDocToDocx?fileName=" + path);
                var response = client.SendAsync(request).Result;
                var responseMessage = response.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }
            }
            catch
            {
                newFileName = null;
            }
            finally
            {


                pdfFile = System.IO.File.ReadAllBytes(newFileName);
                File.Delete(newFileName);
            }



            return pdfFile;
        }

        public static byte[] ConvertDocToDocx_New(string filename, byte[] content)
        {
            var TempStorgepath = System.Configuration.ConfigurationManager.AppSettings["WordAddInStoragePath"];
            var path = TempStorgepath + filename;
            System.IO.File.WriteAllBytes(path, content);
            var oldPath = path;
            string newFileName = path.Replace(".doc", ".docx");
            //Initialize an instance of Document class
            Document document = new Document();
            //Load a Docx file
            document.LoadFromFile(path);

            //Convert the Doc file to Docx
            document.SaveToFile(newFileName, FileFormat.Docx);
            File.Delete(oldPath);
            var pdfFile = System.IO.File.ReadAllBytes(newFileName);
            File.Delete(newFileName);
            return pdfFile;
        }



        //public static string CreatePDF(string path, string exportDir)
        //{
        //    Application app = new Application();
        //    app.DisplayAlerts = WdAlertLevel.wdAlertsNone;
        //    app.Visible = false;
        //    var oldPath = path;
        //    var objPresSet = app.Documents;
        //    string pdfPath = "";
        //    //objPresSet.Close(false);
        //    //app.ReleaseComObject(objPres);
        //    //objPres.Quit();
        //    //Marshal.ReleaseComObject(objPres);
        //    //System.IO.Packaging.Package.Open(pdfPath);
        //    try
        //    {
        //        var objPres = objPresSet.Open(path, MsoTriState.msoTrue, MsoTriState.msoTrue, MsoTriState.msoCTrue);
        //        var pdfFileName = Path.ChangeExtension(path, ".pdf");
        //        pdfPath = Path.Combine(exportDir, pdfFileName);
        //        objPres.ExportAsFixedFormat(
        //            pdfPath,
        //            WdExportFormat.wdExportFormatPDF,
        //            false,
        //            WdExportOptimizeFor.wdExportOptimizeForPrint,
        //            WdExportRange.wdExportAllDocument
        //        );
        //        objPres.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteException(ex);
        //        pdfPath = null;
        //    }
        //    finally
        //    {

        //        ((_Application)app).Quit();
        //        File.Delete(oldPath);
        //    }

        //    return pdfPath;
        //}


    }
}