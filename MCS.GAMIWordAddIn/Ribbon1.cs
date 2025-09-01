using Microsoft.Office.Tools.Ribbon;
using System;
using System.Net;
using MCS.Common.ApiControllerResults;
//----< Word Addin >----
using Word = Microsoft.Office.Interop.Word;
using MCS.Common;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Office.Tools.Word;
using MCS.DTO;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Windows.Forms;
//----</ Word Addin >----

namespace WordAddIn1
{
    public partial class Ribbon1
    {
        public static char Sperator = '_';
        public static string StartKey = "Transaction";
        public static string EndKey = "GAMI";
        public string UserName = string.Empty;
        public static string AprroveTrasnactionKey = "AppriveSign$$$$$#############@@@@@@@%%%%%!!!!!!*$#@!";
        public static string data = string.Empty;
        public static bool IsMorasalteTransaction = false;
        public string TransactionId = string.Empty;
        public string FileName = string.Empty;


        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            try
            {
                CheckDocument();

                if (!string.IsNullOrWhiteSpace(FileName))
                {
                    try
                    {

                        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
                        var result = HttpClientWrapper<PostResult>.PostRequest("User/WordAddIn/MarkDocumentAsRead?fileName=" + FileName, "").Result;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.ToString());
                        System.Windows.Forms.MessageBox.Show("MarkDocumentAsRead حدث خطأ اثناء حفظ الوثيقة . يرجى التواصل مع مدير النظام");

                    }

                }


            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
                //System.Windows.Forms.MessageBox.Show("حدث خطأ اثناء حفظ الوثيقة . يرجى التواصل مع مدير النظام");
            }
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


            //DialogResult result = MessageBox.Show("هل انت متأكد من حفظ التعديلات في نظام وثيق ؟", "حفظ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            //if (result.Equals(DialogResult.OK))
            //{
            CheckDocument();

            if (IsMorasalteTransaction)
            {
                try
                {
                    byte[] content = null;
                    Word._Document document = Globals.ThisAddIn.Application.ActiveDocument;
                    content = System.Text.UTF8Encoding.UTF8.GetBytes(document.Content.WordOpenXML);
                    WordAddinDocumentDTO dataDoc = new WordAddinDocumentDTO();

                    dataDoc.content = content;
                    dataDoc.userName = UserName;
                    dataDoc.TransactionId = TransactionId;
                    dataDoc.FileName = FileName.Replace(".docx", ".doc");




                    var thread = new Thread(
        () =>
        {
            AutoClosingMessageBox.Show("الرجاء الانتظار ... سيتم اغلاق النافذة تلقائيا عند الانتهاء من عملية الحفظ .", "Caption", 3000);

        });
                    thread.Start();
                    bool isSuucess = TrySend(dataDoc);
                    if (isSuucess)
                    {

                        Object saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
                        Object originalFormat = Type.Missing;
                        Object routeDocument = Type.Missing;
                        int iTotInstance = document.Application.Documents.Count;
                        if (iTotInstance > 1)
                        {
                            document.Application.ActiveDocument.Close(ref saveChanges, ref originalFormat, document);
                        }
                        else
                        {
                            document.Application.Quit(ref saveChanges, ref originalFormat, document);
                        }


                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("حدث خطأ اثناء حفظ الوثيقة . يرجى التواصل مع مدير النظام");

                    }


                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                    System.Windows.Forms.MessageBox.Show("حدث خطأ اثناء حفظ الوثيقة . يرجى التواصل مع مدير النظام");

                }
            }

            //}
        }

        private bool TrySend(WordAddinDocumentDTO dataDoc)
        {
            int tryCount = 0;
            while (true)
            {
                tryCount++;
                try

                {


                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
                    PostResult putResult = HttpClientWrapper<PostResult>.PostRequest("User/WordAddIn/UpdateDocument", dataDoc).Result;

                    if (putResult.StatusCode == StatusCode.Ok)
                    {

                        return true;
                    }

                }
                catch (Exception ex)
                {
                    if (tryCount > 5)
                    {
                        throw ex;
                    }
                }

            }



        }






    }



    public class AutoClosingMessageBox
    {
        System.Threading.Timer _timeoutTimer;
        string _caption;
        AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            using (_timeoutTimer)
                MessageBox.Show(text, caption);
        }
        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }
        void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        public void OnTimerElapsed()
        {
            IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}
