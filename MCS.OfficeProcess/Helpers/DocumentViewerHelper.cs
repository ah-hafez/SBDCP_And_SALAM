using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using System.Configuration;
using System.IO;
using Microsoft.Ajax.Utilities;
using System;

using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;

using System.Text;
using System.Web.UI.WebControls;
using MCS.Framework.Exceptions;
using MCS.Framework.Logging;

namespace MCS.OfficeProcess.Helpers
{
    public class DocumentViewerHelper
    {


        protected static string ArchivingPDFPath = "ArchivingPDF";

        public static string CreatePDF(string path, string exportDir)
        {
            Application app = new Application();
            app.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            app.Visible = false;
            var oldPath = path;
            var objPresSet = app.Documents;
            string pdfPath = "";
            //objPresSet.Close(false);
            //app.ReleaseComObject(objPres);
            //objPres.Quit();
            //Marshal.ReleaseComObject(objPres);
            //System.IO.Packaging.Package.Open(pdfPath);
            try
            {
                var objPres = objPresSet.Open(path, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);
                var pdfFileName = Path.ChangeExtension(path, ".pdf");
                pdfPath = Path.Combine(exportDir, pdfFileName);
                objPres.ExportAsFixedFormat(
                    pdfPath,
                    WdExportFormat.wdExportFormatPDF,
                    false,
                    WdExportOptimizeFor.wdExportOptimizeForPrint,
                    WdExportRange.wdExportAllDocument
                );
                objPres.Close();
            }
            catch (Exception ex)
            {

                pdfPath = null;
            }
            finally
            {

                ((_Application)app).Quit();
                File.Delete(oldPath);
            }

            return pdfPath;
        }
        public static string CreatePDF_New(string path, string exportDir)
        {
          
            var oldPath = path;

            string pdfPath = "";
            var pdfFileName = Path.ChangeExtension(path, ".pdf");
            pdfPath = Path.Combine(exportDir, pdfFileName);
            try
            {
               
                ProcessAsUser.Launch(System.AppContext.BaseDirectory + @"Tool\DocWordToPDF.exe " + path);

                if (File.Exists(pdfPath))
                {
                  
                    byte[] pdfData = File.ReadAllBytes(pdfPath);
                    //File.Delete(pdfPath);

                }
                else
                {
                    Logger.WriteInformation("pdfPath File Not Found = " + pdfPath);
                }

            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

            }
            finally
            {


                File.Delete(oldPath);
            }

            return pdfPath;
        }
        public static string ConvertDocToDocx(string path)
        {

            Application app = new Application();
            app.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            app.Visible = false;
            var oldPath = path;
            var objPresSet = app.Documents;


            object objFalse = false;
            object objTrue = true;
            object missing = System.Reflection.Missing.Value;


            var objPres = objPresSet.Open(path, ref objFalse, ref objFalse, ref objFalse, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            string newFileName = path.Replace(".doc", ".docx");
            byte[] pdfFile = null;
            if (System.IO.File.Exists(newFileName))
            {
                System.IO.File.Delete(newFileName);
            }

            try
            {
                objPres.SaveAs2(newFileName, WdSaveFormat.wdFormatXMLDocument,
                             CompatibilityMode: WdCompatibilityMode.wdWord2013);

            }
            catch
            {
                newFileName = null;
            }
            finally
            {
                objPres.Close();
                ((_Application)app).Quit();
                File.Delete(oldPath);
                //pdfFile = System.IO.File.ReadAllBytes(newFileName);
                //File.Delete(newFileName);
            }



            return newFileName;
        }


        //var response = await client.PostAsync("http://localhost/MCS.WordToPDF/api/converter/").ConfigureAwait(false);

    }


}