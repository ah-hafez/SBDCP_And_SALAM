using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocWordToPDF
{
    class Program
    {

        private   static FileSystemWatcher fileSystemWatcher;
        private static string folderToWatchFor
            = @"C:\LeadersSoftProject\MEWA\MCS.WordToPDF\Test";


        //private static string folderToWatchFor
        //   = @"C:\inetpub\wwwroot\MCS.WordToPDF\Test";


        public static  void FileInputMonitor()
        {
            fileSystemWatcher = new FileSystemWatcher(folderToWatchFor);
            fileSystemWatcher.EnableRaisingEvents = true;

            // Instruct the file system watcher to call the FileCreated method
            // when there are files created at the folder.
            fileSystemWatcher.Created += new FileSystemEventHandler(FileCreated);

            fileSystemWatcher.Changed += new FileSystemEventHandler(FileCreated);



        } // end FileInputMonitor()

        private  static void FileCreated(Object sender, FileSystemEventArgs e)
        {
            if (e.Name.EndsWith(".doc") && e.Name.StartsWith("PDF_"))
            {

                convertDOCtoPDF(e.FullPath);

            }

        }

        static void Main(string[] args)
        {

            if (args.Length > 0)
            {
                if (args[0] != null)
                {
                    Convert(args[0]);
                }
            }


            //FileInputMonitor();

            ////////////System.Diagnostics.Process.Start(@"C:\LeadersSoftProject\MEWA\MCS.WordToPDF\Tool\wordToPDF.exe ", "0e22e78a-b0ee-4232-8ed1-60302cd5cb7c.docx");

            ////////////convertDOCtoPDF();

            //Console.Read();

        }

        private  static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                //TODO
            }
            finally
            {
                GC.Collect();
            }
        }


        private static  void convertDOCtoPDF(string filePath)
        {

            object misValue = System.Reflection.Missing.Value;
            String PATH_APP_PDF = filePath.Replace(".doc" , ".pdf");



            var WORD = new Microsoft.Office.Interop.Word.Application();

            //Microsoft.Office.Interop.Word.Application WORD;
            //WORD = (Microsoft.Office.Interop.Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");


            object inputFile = filePath;    // "selected_doc" contains the document name
            object confirmConversions = false;
            object readOnly = false;
            object visible = true;
            object missing = Type.Missing;

            Document doc = WORD.Documents.Open(
                ref inputFile, ref confirmConversions, ref readOnly, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref visible,
                ref missing, ref missing, ref missing, ref missing);

            //var WORD = new Microsoft.Office.Interop.Word.Application();

            //Microsoft.Office.Interop.Word.Document doc = WORD.Documents.Open(filePath);

            doc.Activate();

            doc.SaveAs2(@PATH_APP_PDF, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF, misValue, misValue, misValue,
            misValue, misValue, misValue, misValue, misValue, misValue, misValue);

            doc.Close();
            WORD.Quit();


            releaseObject(doc);
            releaseObject(WORD);

        }

        public static  void Convert( string filePath)
        {
            Application word = new Application();

            Document doc = word.Documents.Open(filePath);
            //doc.Activate();
            filePath = filePath.Replace(".doc", ".pdf");
            doc.SaveAs2(filePath, WdSaveFormat.wdFormatPDF);
            doc.Close();
        }

    }
}
