using MCS.DoconutMVC.Helpers;
using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

// Please make sure you have copied ALL THREE DLLS and Doconut.lic file to the bin folder
// DocumentConfig.dll, DocumentViewer.dll & DocumentFormats.dll
// They are provided in the main asp.net zip. Download link provided in trial email

[assembly: WebResource("MCS.DoconutMVC.Content.Annotations.css", "text/css")]
[assembly: WebResource("MCS.DoconutMVC.Scripts.dropzone.basic.css", "text/css")]
[assembly: WebResource("MCS.DoconutMVC.Scripts.dropzone.dropzone.css", "text/css")]

[assembly: WebResource("MCS.DoconutMVC.Resources.dynamsoft.webtwain.css", "text/css")]

[assembly: WebResource("MCS.DoconutMVC.Scripts.dropzone.dropzone-amd-module.js", "text/javascript")]//##
[assembly: WebResource("MCS.DoconutMVC.Scripts.dropzone.dropzone.js", "text/javascript")]
[assembly: WebResource("MCS.DoconutMVC.Scripts.annotations.js", "text/javascript")]
[assembly: WebResource("MCS.DoconutMVC.Scripts.viewer.js", "text/javascript")]
[assembly: WebResource("MCS.DoconutMVC.Scripts.jquery-3.6.4.min.js", "text/javascript")]

[assembly: WebResource("MCS.DoconutMVC.Resources.dynamsoft.webtwain.initiate.js", "text/javascript")]
[assembly: WebResource("MCS.DoconutMVC.Resources.dynamsoft.webtwain.config.js", "text/javascript")]
[assembly: WebResource("MCS.DoconutMVC.Resources.dynamsoft.dynamsoft.webtwain.install.js", "text/javascript")]

[assembly: WebResource("MCS.DoconutMVC.images.Approved_Stamp.png", "image/png")]
[assembly: WebResource("MCS.DoconutMVC.images.arrow.png", "image/png")]
[assembly: WebResource("MCS.DoconutMVC.images.circle.png", "image/png")]
[assembly: WebResource("MCS.DoconutMVC.images.close.png", "image/png")]//##
[assembly: WebResource("MCS.DoconutMVC.images.ellipse.png", "image/png")]
[assembly: WebResource("MCS.DoconutMVC.images.freehand.png", "image/png")]
[assembly: WebResource("MCS.DoconutMVC.images.image.png", "image/png")]
[assembly: WebResource("MCS.DoconutMVC.images.line.png", "image/png")]
[assembly: WebResource("MCS.DoconutMVC.images.loadingAnimation.gif", "image/gif")]//##
[assembly: WebResource("MCS.DoconutMVC.images.note.png", "image/png")]
[assembly: WebResource("MCS.DoconutMVC.images.rectangle.png", "image/png")]
[assembly: WebResource("MCS.DoconutMVC.images.square.png", "image/png")]//##
[assembly: WebResource("MCS.DoconutMVC.images.stamp.png", "image/png")]
[assembly: WebResource("MCS.DoconutMVC.images.text.png", "image/png")]//##
[assembly: WebResource("MCS.DoconutMVC.images.triangle.png", "image/png")]

[assembly: WebResource("MCS.DoconutMVC.files.Sample.doc", "application/msword")]
[assembly: WebResource("MCS.DoconutMVC.files.Sample.ppt", "application/vnd.ms-powerpoint")]

#region styling
[assembly: WebResource("MCS.DoconutMVC.Content.bootstrap.min.css", "text/css")]
[assembly: WebResource("MCS.DoconutMVC.Content.rtl.min.css", "text/css")]
[assembly: WebResource("MCS.DoconutMVC.Content.font-awesome.min.css", "text/css")]
[assembly: WebResource("MCS.DoconutMVC.Content.mob_style.css", "text/css")]
[assembly: WebResource("MCS.DoconutMVC.Content.style.css", "text/css")]
[assembly: WebResource("MCS.DoconutMVC.Content.User.lib.fontawesome.css.all.css", "text/css")]

[assembly: WebResource("MCS.DoconutMVC.Scripts.bootstrap.min.js", "text/javascript")]
[assembly: WebResource("MCS.DoconutMVC.Scripts.jquery.ui.touch-punch.min.js", "text/javascript")]


[assembly: WebResource("MCS.DoconutMVC.fonts.FontAwesome.otf", "font/opentype")]
[assembly: WebResource("MCS.DoconutMVC.fonts.fontawesome-webfont.eot", "application/vnd.ms-fontobject")]
[assembly: WebResource("MCS.DoconutMVC.fonts.fontawesome-webfont.svg", "image/svg+xml")]
[assembly: WebResource("MCS.DoconutMVC.fonts.fontawesome-webfont.ttf", "application/font-sfnt")]
[assembly: WebResource("MCS.DoconutMVC.fonts.fontawesome-webfont.woff", "application/font-woff")]

[assembly: WebResource("MCS.DoconutMVC.content.uicon.svg", "image/svg+xml")]
[assembly: WebResource("MCS.DoconutMVC.content.uicon.ttf", "application/font-sfnt")]
[assembly: WebResource("MCS.DoconutMVC.content.uicon.woff", "application/font-woff")]
#endregion styling

namespace DoconutViewer.Controllers
{
    public class DocoNutController : Controller
    {
        public enum enUpLoadMode
        {
            Normal = 1,
            Repalce = 2,
            PreSave = 3,
            AfterSave = 4
        }
        public readonly ConfigurationHelper configurationHelper;
        public DocoNutController()
        {
            configurationHelper = (ConfigurationHelper)ConfigurationManager.GetSection("DocumentViewerSectionGroup/DocumentViewer");
        }
        StringBuilder stringBuilder = new StringBuilder();
        public ActionResult Index(string hidecontrols, string showwatermark, string sIncludedItemDoc, string annotationonly, string showbarcode, string explanations)
        {
            try
            {
                string culture = Session["Culture"]?.ToString() ?? "ar-JO";
                CultureInfo cultureInfo;
                cultureInfo = new CultureInfo(culture);
                System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                Session["BasePath"] = configurationHelper.BasePath;
                byte[] docoNutDocument = null;

                if (!string.IsNullOrEmpty(sIncludedItemDoc))
                {
                    //TODO: handle main/attachment documents here
                    if (Session["DocoNutIncDocument"] != null)
                    {
                        docoNutDocument = Session["DocoNutIncDocument"] as byte[];
                    }
                }
                else if (!string.IsNullOrEmpty(explanations))
                {
                    //TODO: handle main/attachment documents here
                    if (Session["DocoNutexplanations"] != null)
                    {
                        docoNutDocument = Session["DocoNutexplanations"] as byte[];
                    }
                }
                else
                {
                    if (Session["DocoNutDocument"] != null)
                    {
                        docoNutDocument = Session["DocoNutDocument"] as byte[];
                    }
                    Session["WaterMarkMain"] = showwatermark;
                }


                var viewer = new DocViewer
                {
                    ID = "ctlDoc",
                    IncludeJQuery = false,
                    DebugMode = false,
                    BasePath = configurationHelper.BasePath,
                    // You will need to change the base path if the MVC project is inside a folder; 
                    // eg. BasePath = "TestViewer"; if the Url is such:  http://localhost:xxx/TestViewer/
                    FitType = "",
                    Zoom = 50,
                    TimeOut = 10 // After how many minutes of idleness, does the document free up memory
                };

                // Get the required client side script and css

                ViewBag.ViewerScripts = viewer.ReferenceScripts();   // Please make sure you have copied Doconut.lic in bin folder
                ViewBag.ViewerCSS = viewer.ReferenceCss();           // Download link provided in trial email 
                ViewBag.ViewerID = viewer.ClientID;
                ViewBag.ViewerObject = viewer.JsObject;
                // open default document

                string token = null;

                dynamic config;
                if (docoNutDocument == null)
                {
                    //display empty page on init
                    config = new WordConfig { PaperSize = DocPaperSize.A4 };
                    token = viewer.OpenDocument(new byte[1], "txt", config);
                    Session["printToken"] = token; //here
                }
                else
                {
                    string printToken = null;
                    docoNutDocument = DoconutHelper.RemoveWatermark(docoNutDocument);
                    if (!string.IsNullOrWhiteSpace(showwatermark) && showwatermark.ToLower() == "true")
                    {
                        if (((bool)Session["WatermarkPermissions"]))
                        {
                            config = new PdfConfig { DefaultRender = true, CMapPath = "" };
                            printToken = viewer.OpenDocument(docoNutDocument, "pdf", config);

                            if (string.IsNullOrEmpty(printToken))
                            {
                                printToken = viewer.OpenDocument(docoNutDocument, "tif");
                            }
                        }

                        docoNutDocument = DoconutHelper.AddWatermark(docoNutDocument, Session["WatermarkText"].ToString());



                    }
                    else
                    {
                        if (Session["IsEditMode"] == null || !(bool)Session["IsEditMode"])
                        {
                            docoNutDocument = DoconutHelper.AddWatermark(docoNutDocument, Session["WatermarkText"].ToString());
                        }
                    }

                    config = new PdfConfig { DefaultRender = true, CMapPath = "" };
                    token = viewer.OpenDocument(docoNutDocument, "pdf", config);

                    if (string.IsNullOrEmpty(token))
                    {
                        token = viewer.OpenDocument(docoNutDocument, "tif");
                    }

                    Session["printToken"] = string.IsNullOrEmpty(printToken) ? token : printToken;
                }

                if (token.IsNullOrWhiteSpace())
                {
                    throw new Exception(viewer.InternalError);
                }

                // Get final Init arguments to render the viewer

                ViewBag.ViewerInit = viewer.GetAjaxInitArguments(token);

                ViewBag.globalToken = token; // Get the first / default token value to the JS variable.


                // You need to store this in session if you want to 
                // call methods like export, annotation export etc on
                // the document

                Session[token] = viewer;

                return View();
            }
            catch (Exception ex)
            {

                stringBuilder.AppendLine(ex.Message);

                while (ex.InnerException != null)
                {
                    stringBuilder.AppendLine(ex.InnerException.Message);
                    ex = ex.InnerException;
                }

                //System.IO.File.AppendAllText(@"C:\MCS.UI_logs\Doconut.ada", stringBuilder.ToString());
                throw;
            }
        }

        [HttpPost]
        public ContentResult UploadFile(string documentToken, int modeValue, int pageIndex)
        {
            var isSavedSuccessfully = true;
            var fName = "";

            try
            {
                foreach (string fileName in Request.Files)
                {
                    var file = Request.Files[fileName];

                    if (null == file)
                        continue;

                    if (file.ContentLength <= 0) continue;

                    // check for any malicious file types
                    var invalidFiles = ".EXE .JS .JAR .VBS .VB .SFX .BAT .DLL .TMP .PY .ASP .ASPX .ASHX .ASMX .AXD .PHP .MSI .COM .CMD .VBE .LNK .ZIP .RAR .7Z";
                    var fileExtension = new FileInfo(file.FileName).Extension.ToUpper();

                    string mimeType = MimeMapping.GetMimeMapping(file.FileName);

                    if (invalidFiles.IndexOf(fileExtension, StringComparison.Ordinal) > -1 || !IsValidMimeType(mimeType))
                    {
                        throw new Exception("Invalid file extension");
                    }

                    fName = DateTime.Now.ToShortDateString().Replace("/", "-") + "--" + file.FileName;

                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        fName = fName.Replace(c, '-');
                    }

                    fName = fName.Replace("%", "-").Replace("&", "-").Replace("#", "-").Replace(";", "-").Replace("+", "-").Replace(" ", "-");

                    var viewer = new DocViewer
                    {
                        ID = "ctlDoc",
                        IncludeJQuery = false,
                        DebugMode = false,
                        BasePath = "/",
                        // You will need to change the base path if the MVC project is inside a folder; 
                        // eg. BasePath = "TestViewer"; if the Url is such:  http://localhost:xxx/TestViewer/
                        FitType = "FitWidth",
                        TimeOut = 10 // After how many minutes of idleness, does the document free up memory
                    };

                    BaseConfig config = null;

                    switch (new FileInfo(fName).Extension.ToUpper())
                    {
                        case ".DWG":
                        case ".DXF":
                            config = new CadConfig { ShowColor = false, WhiteBackground = true, ShowModel = false, ShowLayouts = true, LineWidth = 1, Check3DSolid = false };
                            break;
                        case ".DOC":
                        case ".DOCX":
                            config = new WordConfig { ConvertPdf = true };
                            break;
                        case ".TXT":
                            config = new WordConfig { PaperSize = DocPaperSize.A4 };
                            break;
                        case ".EML":
                        case ".MSG":
                            var emlConf = new EmailConfig { EmailEncoding = Encoding.UTF8 };
                            emlConf.PdfConfiguration.DefaultRender = true;
                            emlConf.PdfConfiguration.CMapPath = "";

                            config = emlConf;
                            break;
                        case ".XLS":
                        case ".XLSX":
                        case ".ODS":
                            config = new ExcelConfig { SplitWorksheets = true };
                            break;
                        case ".PDF":
                            config = new PdfConfig { DefaultRender = true, CMapPath = "" };
                            break;

                    }

                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();

                    var tempDoc = new DocViewer();
                    tempDoc.OpenDocument(data, fileExtension, config);

                    DocViewer document = null;
                    document = (Session[documentToken] is DocViewer) ? Session[documentToken] as DocViewer : null;

                    byte[] currentDocument = (document != null) ? document.ExportToPdf() : null;
                    byte[] addDocument = tempDoc.ExportToPdf();
                    byte[] newDocument = null;
                    addDocument = DoconutHelper.RemoveWatermark(addDocument);
                    if (document != null && document.TotalPages == 1 && currentDocument.Length <= 12397)
                    {
                        currentDocument = null;
                    }
                    if (tempDoc.TotalPages > 0)
                    {
                        if (currentDocument == null)
                        {
                            modeValue = (int)enUpLoadMode.Normal;
                        }

                        switch (modeValue)
                        {
                            case (int)enUpLoadMode.Normal:
                                List<byte[]> pdfList = new List<byte[]>();
                                if (currentDocument != null)
                                    pdfList.Add(currentDocument);
                                if (addDocument != null)
                                    pdfList.Add(addDocument);
                                byte[] mergedDocument = DoconutHelper.concatAndAddContent(pdfList, "");
                                newDocument = mergedDocument;
                                break;
                            case (int)enUpLoadMode.Repalce:
                                newDocument = DoconutHelper.ReplacePage(currentDocument, addDocument, pageIndex, Session["WatermarkText"].ToString());
                                break;
                            case (int)enUpLoadMode.PreSave:
                                newDocument = DoconutHelper.MovePrev(currentDocument, addDocument, pageIndex, Session["WatermarkText"].ToString());
                                break;
                            case (int)enUpLoadMode.AfterSave:
                                newDocument = DoconutHelper.MoveNext(currentDocument, addDocument, pageIndex, Session["WatermarkText"].ToString());
                                break;
                            default:
                                break;
                        }
                    }

                    var token = viewer.OpenDocument(newDocument, "pdf");

                    string printToken = "";

                    if (Session["WaterMarkMain"] != null && Session["WaterMarkMain"].ToString().ToLower() == "true")
                    {
                        viewer = new DocViewer();
                        viewer.Token = token;

                        byte[] documentByteArray = viewer.ExportToPdf(false);

                        if ((bool)Session["WatermarkPermissions"])
                        {
                            printToken = viewer.OpenDocument(documentByteArray, "pdf", config);
                        }

                        //documentByteArray = DoconutHelper.AddWatermark(documentByteArray, Session["WatermarkText"].ToString());
                        token = viewer.OpenDocument(documentByteArray, "pdf", config);
                    }

                    Session["printToken"] = string.IsNullOrEmpty(printToken) ? token : printToken;

                    if (token.IsNullOrWhiteSpace())
                    {
                        throw new Exception(viewer.InternalError);
                    }

                    // You need to store this in session if you want to 
                    // call methods like export, annotation export etc on
                    // the document

                    Session[token] = viewer;

                    return Content(token);

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }

            return Content(isSavedSuccessfully ? fName : "");
        }

        [HttpPost]
        public ActionResult ExportAnnotations(string token)
        {
            if (token.IsNullOrWhiteSpace())
            {
                throw new Exception("error");
            }

            // Get stored instance from Session

            if (!(Session[token] is DocViewer document))
            {
                throw new Exception("error");
            }

            try
            {
                var exportBytes = document.ExportToPdf(true);
                var fileName = $"{DateTime.Now.Ticks}-Export.pdf";

                System.IO.File.WriteAllBytes(Server.MapPath($"~/files/{fileName}"), exportBytes);
                return Content(fileName);
            }
            catch (Exception ex)
            {
                return Content("error " + ex.Message);
            }
        }

        [HttpPost]
        public ActionResult ExportXml(string token)
        {
            if (token.IsNullOrWhiteSpace())
            {
                throw new Exception("error");
            }

            // Get stored instance from Session

            if (!(Session[token] is DocViewer document))
            {
                throw new Exception("error");
            }

            try
            {
                var annXml = document.GetAnnotationXML();

                var fileName = $"{DateTime.Now.Ticks}-Export.xml";

                annXml.Save(Server.MapPath($"~/files/{fileName}"));

                return Content(fileName);
            }
            catch (Exception ex)
            {
                return Content("error " + ex.Message);
            }
        }
        [HttpGet]
        public ActionResult GetTransactionId()
        {
            bool isWatermarkPermission = (bool)Session["WatermarkPermissions"];
            int trxId = 0;
            if (Session["TransactionId"] != null)
            {
                trxId = int.Parse(Session["TransactionId"].ToString());
            }
            return Json(new { transactionId = trxId, printWithoutWatermark = isWatermarkPermission }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddWatermark(string token)
        {
            string returnToken = "";
            var Oldviewer = new DocViewer();
            var viewer = new DocViewer();
            try
            {
                if (!(bool)Session["WatermarkPermissions"])
                {

                    Oldviewer.Token = token;

                    byte[] documentByteArray = Oldviewer.ExportToPdf(false);

                    documentByteArray = DoconutHelper.AddWatermark(documentByteArray, Session["WatermarkText"].ToString());

                    var config = new PdfConfig { DefaultRender = true, CMapPath = "" };
                    returnToken = viewer.OpenDocument(documentByteArray, "pdf", config);
                    viewer.Token = returnToken;
                    Session[returnToken] = viewer;
                }
                else
                {

                    Oldviewer.Token = token;

                    byte[] documentByteArray = Oldviewer.ExportToPdf(false);

                    documentByteArray = DoconutHelper.RemoveWatermark(documentByteArray);

                    var config = new PdfConfig { DefaultRender = true, CMapPath = "" };
                    returnToken = viewer.OpenDocument(documentByteArray, "pdf", config);
                    viewer.Token = returnToken;
                    Session[returnToken] = viewer;
                }
            }
            catch (Exception)
            {

            }

            //return Content(isSavedSuccessfully? fName : "");
            return Content(returnToken);
        }
        public ActionResult Print()
        {
            return View();
        }

        [HttpPost]
        public ContentResult DeletePage(string token, string sCurrPageIndex)
        {
            try
            {
                var viewer = new DocViewer();
                viewer.Token = token;

                var nCurrPageIndex = int.Parse(sCurrPageIndex);
                var config = new PdfConfig { DefaultRender = true, CMapPath = "" };

                if (nCurrPageIndex > 0 && nCurrPageIndex <= viewer.TotalPages)
                {
                    if (viewer.TotalPages > 1)
                    {
                        byte[] documentByteArray = viewer.ExportToPdf(false);
                        byte[] updatedDocument = DoconutHelper.deletePage(documentByteArray, nCurrPageIndex, Session["WatermarkText"].ToString());
                        token = viewer.OpenDocument(updatedDocument, "pdf", config);
                    }
                    else
                    {
                        var configTXT = new WordConfig { PaperSize = DocPaperSize.A4 };
                        token = viewer.OpenDocument(new byte[1], "txt", configTXT);
                        viewer.Token = token;
                    }
                    Session[token] = viewer;
                }
            }
            catch (Exception)
            {

            }
            return Content(token);
        }

        [HttpPost]
        public string GetImagePath(string base64string, string imageName)
        {
            string filePath = string.Empty;
            string fileName = string.Empty;
            try
            {
                fileName = $"{imageName}.png";
                string rootApplication = AppDomain.CurrentDomain.BaseDirectory + "FileImages";
                byte[] imageByteArray = Convert.FromBase64String(base64string);
                // If directory does not exist, create it. 
                if (!Directory.Exists(rootApplication))
                {
                    Directory.CreateDirectory(rootApplication);
                }
                System.IO.File.WriteAllBytes(rootApplication + @"\" + fileName, imageByteArray);

            }
            catch (Exception)
            {
            }
            return $"FileImages/{fileName}?{DateTime.Now.Ticks}";
        }

        [HttpPost]
        public bool DeleteImageFile(string imageName)
        {
            string fileName = string.Empty;
            try
            {
                fileName = $"{imageName}.png";
                string rootApplication = AppDomain.CurrentDomain.BaseDirectory + "FileImages";
                if (Directory.Exists(rootApplication))
                {
                    System.IO.File.Delete(rootApplication + @"\" + fileName);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        [HttpPost]
        public ActionResult DeleteDocumentTemp()
        {
            string token = null;
            try
            {
                var viewer = new DocViewer();

                //display empty page on init
                var config = new WordConfig { PaperSize = DocPaperSize.A4 };
                token = viewer.OpenDocument(new byte[1], "txt", config);
                viewer.Token = token;

                Session[token] = viewer;
            }
            catch (Exception)
            {

            }

            return Content(token);
        }

        [HttpGet]
        public ActionResult DownloadFile(string token)
        {
            try
            {
                var viewer = new DocViewer();
                viewer.Token = token;

                byte[] documentByteArray = viewer.ExportToPdf(true);
                if (!((bool)Session["WatermarkPermissions"]))
                {
                    documentByteArray = DoconutHelper.AddWatermark(documentByteArray, Session["WatermarkText"].ToString());
                }
                token = viewer.OpenDocument(documentByteArray, "pdf");
                viewer.Token = token;
                documentByteArray = viewer.ExportToPdf(true);
                return File(documentByteArray, "application/pdf", Guid.NewGuid().ToString() + ".pdf");
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        private bool IsValidMimeType(string MimeType)
        {
            Dictionary<string, string> AllowedMimeTypes = new Dictionary<string, string>
            {
                { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "docx" },
                { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xlsx" },
                { "application/pdf", "pdf" },
                { "image/png", "png" },
                { "image/jpeg", "jpeg" },
                { "image/gif", "gif" },
                { "image/bmp", "bmp" },
                { "application/vnd.ms-excel", "xls" },
                { "application/msword", "doc" },
                { "image/tiff", "tif" }
            };

            if (!AllowedMimeTypes.Keys.Contains(MimeType))
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        public ContentResult ChangePageOrder(string token, string sCurrPageIndex, bool orderType)
        {
            try
            {
                var viewer = new DocViewer();
                viewer.Token = token;

                var nCurrPageIndex = int.Parse(sCurrPageIndex);
                var config = new PdfConfig { DefaultRender = true, CMapPath = "" };

                if (nCurrPageIndex > 0 && nCurrPageIndex <= viewer.TotalPages)
                {
                    if (viewer.TotalPages > 1)
                    {
                        byte[] documentByteArray = viewer.ExportToPdf(false);
                        byte[] updatedDocument = null;
                        if (orderType)
                        {
                            updatedDocument = DoconutHelper.MovePageUp(documentByteArray, nCurrPageIndex);
                        }
                        else
                        {
                            updatedDocument = DoconutHelper.MovePageDown(documentByteArray, nCurrPageIndex);
                        }


                        token = viewer.OpenDocument(updatedDocument, "pdf", config);
                    }
                    else
                    {
                        var configTXT = new WordConfig { PaperSize = DocPaperSize.A4 };
                        token = viewer.OpenDocument(new byte[1], "txt", configTXT);
                        viewer.Token = token;
                    }
                    Session[token] = viewer;
                }
            }
            catch (Exception)
            {

            }
            return Content(token);
        }

    }
}