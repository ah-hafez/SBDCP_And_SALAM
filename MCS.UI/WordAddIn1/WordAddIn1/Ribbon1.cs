using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MCS.Common.ApiControllerResults;
using System.IO;
using MCS.DTO.Word_Add_in;

//----< Word Addin >----
using Word = Microsoft.Office.Interop.Word;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using MCS.Common;
using System.Windows.Forms;
//----</ Word Addin >----


namespace WordAddIn1
{
    public partial class Ribbon1
    {


        public static string data = string.Empty;

        public static bool IsMorasalteTransaction = false;

        public static string StartKey = "Transaction";
        public static string EndKey = "Buraq";

        public static char Sperator = '_';

        public string TransactionId = string.Empty;

        public string UserName = string.Empty;

        public string FileName = string.Empty;

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }



        private void CheckDocument()
        {
            Document vstoDocument = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveDocument);
            string docName = vstoDocument.Name;

            FileName = docName;

            if (docName.StartsWith(StartKey) && (docName.EndsWith(EndKey) || docName.EndsWith(EndKey + ".doc") || docName.EndsWith(EndKey + ".docx")))
            {

                IsMorasalteTransaction = true;

                String[] DocNameData = docName.Split(Sperator);

                try
                {
                    TransactionId = DocNameData[1].ToString();

                    UserName = DocNameData[2].ToString();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);

                    IsMorasalteTransaction = false;
                }
            }

        }

        private void btnSave_Click(object sender, RibbonControlEventArgs e)
        {
            DialogResult result = MessageBox.Show("هل انت متأكد من حفظ التعديلات في نظام براق ؟", "حفظ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                CheckDocument();

                if (IsMorasalteTransaction)
                {
                    try
                    {
                        byte[] content = null;
                        content = System.Text.UTF8Encoding.UTF8.GetBytes(Globals.ThisAddIn.Application.ActiveDocument.Content.WordOpenXML);

                        WordAddinDocumentDTO dataDoc = new WordAddinDocumentDTO();

                        dataDoc.content = content;
                        dataDoc.userName = UserName;
                        dataDoc.TransactionId = TransactionId;
                        dataDoc.FileName = FileName;

                        PostResult putResult = HttpClientWrapper<PostResult>.PostRequest("api/WordAddInIntegration/PostDocumentStringObject", dataDoc).Result;

                        if (putResult.StatusCode == StatusCode.Ok)
                        {
                            System.Windows.Forms.MessageBox.Show("تم حفظ الوثيقة بنجاح ");

                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("حدث خطأ اثناء حفظ الوثيقة . يرجى التواصل مع مدير النظام");
                        }


                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("حدث خطأ اثناء حفظ الوثيقة . يرجى التواصل مع مدير النظام");

                    }
                }

            }
        }



    }
}
