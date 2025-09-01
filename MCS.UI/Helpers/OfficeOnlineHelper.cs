using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Xceed.Words.NET;
using Xceed.Document.NET;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Net.Http;
using System.Threading.Tasks;

//using NPOI.XWPF.UserModel;

namespace MCS.UI.Helpers
{
    public class OfficeOnlineHelper
    {


        public static void StartAddGraph(string wrdFilePath, byte[] signatureByteArray, int SigntuerType, string employeeName)
        {
            System.Threading.Thread.Sleep(2000);

            //SigntuerType 1= signtuer , 2 = mark
            string SigntuerTypeName = null;
            string bookmark = "";
            if (SigntuerType == 1)
            {
                SigntuerTypeName = "leaders_soft_sig";
                bookmark = "sign";
            }
            else
            {
                SigntuerTypeName = "leaders_soft_mark";
                bookmark = "mark";
            }
            using (var document = DocX.Load(wrdFilePath))
            {
                if (document.Paragraphs.Count == 0)
                {
                    document.InsertParagraph("");
                }

                Paragraph p = document.Paragraphs[0];
                using (Stream stream = new MemoryStream(signatureByteArray))
                {
                    var image = document.AddImage(stream, "image/png");
                    var picture = image.CreatePicture(112.5f, 112.5f);
                    picture.Name = SigntuerTypeName;

                    System.Collections.Generic.List<int> findRes = document.FindAll(bookmark);

                    if (findRes.Count > 0)
                    {
                        document.ReplaceTextWithObject(bookmark, picture, false, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        p.AppendPicture(picture);
                    }

                    if (employeeName != null && employeeName != "")
                    {
                        p.AppendLine();
                        p.AppendLine(employeeName);
                    }


                    document.Save();
                }
                //Stream stream = new MemoryStream(signatureByteArray);
                //var image = document.AddImage(stream, "image/png");
                //var picture = image.CreatePicture(112.5f, 112.5f);
                //picture.Name = SigntuerTypeName;



                //if (!CheckSigntureWord(wrdFilePath, SigntuerTypeName))
                //{
                //    p.AppendPicture(picture);
                //}

                //  document.ReplaceTextWithObject(wordReplace, picture, false, RegexOptions.IgnoreCase);


                //if (employeeName != null && employeeName != "")
                //{
                //   p.AppendLine();
                //   p.AppendLine(employeeName);
                //}
                #region This Region commited can we use this code for costmais word file
                //picture.Rotation = 10;
                //picture.SetPictureShape(BasicShapes.cube);

                //// Insert a new Paragraph into the document.
                //Paragraph title = document.InsertParagraph().Append("This is a test for a picture").FontSize(20);
                //title.Alignment = Alignment.center;

                //// Insert a new Paragraph into the document.
                //Paragraph p1 = document.InsertParagraph();

                //// Append content to the Paragraph
                //p1.AppendLine("Just below there should be a picture ").Append("picture").Bold().Append(" inserted in a non-conventional way.");
                //p1.AppendLine();
                //p1.AppendLine("Check out this picture ").AppendPicture(picture).Append(" its funky don't you think?");
                //p1.AppendLine();

                //// Insert a new Paragraph into the document.
                //Paragraph p2 = document.InsertParagraph();
                //// Append content to the Paragraph.

                //p2.AppendLine("Is it correct?");
                //p2.AppendLine();

                //// Lets add another picture (without the fancy stuff)
                //Picture pictureNormal = image.CreatePicture();

                //Paragraph p3 = document.InsertParagraph();
                //p3.AppendLine("Lets add another picture (without the fancy  rotation stuff)");
                //p3.AppendLine();
                //p3.AppendPicture(pictureNormal);
                #endregion
                //document.Save();
                //if (!CheckSigntureWord(wrdFilePath, SigntuerTypeName))
                //{
                //    p.AppendPicture(picture);
                //    document.Save();
                //}

            }
        }
        public static Boolean CheckSignture(string wrdFilePath)
        {
            using (var document = DocX.Load(wrdFilePath))
            {
                if (document.Pictures.Where(img => img.Name.Equals("leaders_soft_sig")).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static Boolean CheckSigntureWord(string wrdFilePath, string SigntuerTypeName)
        {
            using (var document = DocX.Load(wrdFilePath))
            {

                if (document.Pictures.Where(img => img.Name.Equals(SigntuerTypeName)).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public async static Task<byte[]> ConvertDocxToPDF(byte[] docx)
        {
            HttpContent bytesContent = new ByteArrayContent(docx);
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(bytesContent, "file2", "file2");
                client.Timeout = TimeSpan.FromMinutes(2);
                var response = await client.PostAsync("http://localhost/MCS.IAU.WordToPDF/api/converter/", formData).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

                Stream x = await response.Content.ReadAsStreamAsync();

                using (MemoryStream ms = new MemoryStream())
                {
                    x.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
        public async static Task<byte[]> ConvertDocToPDF(byte[] docx)
        {
            HttpContent bytesContent = new ByteArrayContent(docx);
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(bytesContent, "file2", "file2");
                client.Timeout = TimeSpan.FromMinutes(2);
                var response = await client.PostAsync("http://localhost/MCS.IAU.WordToPDF/api/ConverterDocToPdf/", formData).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

                Stream x = await response.Content.ReadAsStreamAsync();

                using (MemoryStream ms = new MemoryStream())
                {
                    x.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

    }
}