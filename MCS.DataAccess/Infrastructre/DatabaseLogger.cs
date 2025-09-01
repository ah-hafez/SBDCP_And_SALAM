using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DataAccess
{
    public class DatabaseLogger
    {
        public void Log(string message)
        {
            try
            {
                if (!Directory.Exists("C:\\MCSLoggerLog\\"))
                {
                    Directory.CreateDirectory("C:\\MCSLoggerLog\\");
                }
                string path = "C:\\MCSLoggerLog\\EF.txt";
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine("{0} ", message);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
