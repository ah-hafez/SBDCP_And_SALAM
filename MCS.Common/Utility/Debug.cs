using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MCS.Common.Utility
{
    public static class Debug
    {
        public static void WriteToFile(string fileName, string text)
        {
            string sErrorLogPath = "C://ErrorLog/";
            if (!Directory.Exists(sErrorLogPath))
            {
                Directory.CreateDirectory(sErrorLogPath);
            }
            string path = sErrorLogPath + fileName + ".txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format("{0} : " + text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }
        public static string CreateXML(Object iObject)
        {
            XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
                                                      // Initializes a new instance of the XmlDocument class.          
            XmlSerializer xmlSerializer = new XmlSerializer(iObject.GetType());
            // Creates a stream whose backing store is memory. 
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, iObject);
                xmlStream.Position = 0;
                //Loads the XML document from the specified string.
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }
    }
}
