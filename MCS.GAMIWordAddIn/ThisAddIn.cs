using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MCS.Common.ApiControllerResults;
using System.IO;
using MCS.Common;

namespace WordAddIn1
{
    public partial class ThisAddIn
    {

        public static  string data = string.Empty;

        public static bool IsMorasalteTransaction = false;

        public static string StartKey = "Transaction";
        public static string EndKey = "GAMI";

        public static char Sperator = '_';

        public string TransactionId = string.Empty;

        public string UserName = string.Empty;

        public string FileName = string.Empty;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
                this.Application.DocumentBeforeSave += Application_DocumentBeforeSave;
                this.Application.DocumentOpen += Application_DocumentOpen;
            
        }

        private void CheckDocument()
        {
            Document vstoDocument = Globals.Factory.GetVstoObject(this.Application.ActiveDocument);
            string docName = vstoDocument.Name;

            FileName = docName;

            if (docName.StartsWith(StartKey) && (docName.EndsWith(EndKey) || docName.EndsWith(EndKey + ".doc") || docName.EndsWith(EndKey + ".docx")))
            {

                IsMorasalteTransaction = true;

                String[] DocNameData = docName.Split(Sperator);

                try
                {
                    TransactionId = DocNameData[1].ToString();

                    UserName= DocNameData[2].ToString();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);

                    IsMorasalteTransaction = false;
                }
            }

        }


        private void Application_DocumentOpen(Microsoft.Office.Interop.Word.Document Doc)
        {

        }

        private void Application_DocumentBeforeSave(Microsoft.Office.Interop.Word.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {

            //CheckDocument();

            //if (IsMorasalteTransaction)
            //{

            //    //string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            //    //String[] userNameData = userName.Split('\\');

            //    //userName = userNameData[1];

            //    try
            //    {


            //        byte[] content = null;

            //        //content = System.Text.Encoding.Default.GetBytes(Globals.ThisAddIn.Application.ActiveDocument.Content.get_XML());
            //        content = System.Text.UTF8Encoding.UTF8.GetBytes(Globals.ThisAddIn.Application.ActiveDocument.Content.WordOpenXML);

            //        WordAddinDocumentDTO dataDoc = new WordAddinDocumentDTO();

            //        dataDoc.content = content;
            //        dataDoc.userName = UserName;
            //        dataDoc.TransactionId = TransactionId;
            //        dataDoc.FileName = FileName;

            //        PostResult putResult = HttpClientWrapper<PostResult>.PostRequest("api/WordAddInIntegration/PostDocumentStringObject", dataDoc).Result;

            //        if (putResult.StatusCode == StatusCode.Ok)
            //        {
            //            System.Windows.Forms.MessageBox.Show("تم حفظ الوثيقة بنجاح ");

            //        }
            //        else
            //        {
            //            System.Windows.Forms.MessageBox.Show("حدث خطأ اثناء حفظ الوثيقة . يرجى التواصل مع مدير النظام");
            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        System.Windows.Forms.MessageBox.Show("حدث خطأ اثناء حفظ الوثيقة . يرجى التواصل مع مدير النظام");
                    
            //    }
            //}
        }



     
        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {

            //System.Windows.Forms.MessageBox.Show("Shutdown Ehab !!!");
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
            
        }
        
        #endregion
    }
}
