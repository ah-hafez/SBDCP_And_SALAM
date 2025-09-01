using Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MobileAPIs.UtilityClasses
{
    public  class Pdf
    {
        public Pdf()
        {  
            string key = "JwEAqs+49xYH9JRaADQBK8clme6iYLi6Ki/uAGlrgB0S/vBTdA5SNHz2DY2+pKkfSr1GE0xrTNHqVNUukethFNbkQPRze/oyJT3k9ai1zvt7qCmQP9JgQJgF5TjTaf2HheI/paUnoOTrRDfAP3c4M52gvvAbRmkh3mpXDnykBfFF1Q89wf1eQqaIODc30cvtU2dW/rvFiPhhgpTK2BBN/gprk+zB5sjC2DHZC2nKtMzOGNn3Jxwbd8XJPRKMH7nnM9cP5O8TRLpaPLs6dYfbCH4bnXc07biJ13FiNPjYP0MuiBuSmWjohxTMRmdgmPyY3hQVquRIJomME+s3jcRJ6vLV1exsEOqtUAF2JJTwrQgrrPW3O99QgQJpoXIA8MUL9HYnDuQtnaPTHu9bqGvNx/+AUcm9RfcnEWRK3lH19Tc2lZMz0NEYXEC6WR7717TLSO1NvHRgPWQAb6NeLkRIiQReR6Cmjchu9dsYJcwdMQc2dIlaUOOnUw3Rxo8yd4lm1UconeAdE87kkEaICKSXmlNRwJC2JoGoDdSLUmitkw4oIgtqZUViqhppKbZgZBUxW8TJS49BRjGZprqcteuIe5PrX8oZyttUnHsytCv3OTwExeQSXcTA0ONGvoT9iKOJRdeOpqKU8lNcHEt6UeR5rWtk1/F4kdusiJNATpx2d6l2gvF+xZnNnZ40yrQXTwBNiNCN299hAoY10/do8BJFQYfCVlVArMTqcwOdVxsMcyWLQWUmKd4nnmZSV5QwFFFsGPBedygmXFdDvgGGSp3xxPg22SPS63q3l52Qnd+JH1CVO08eoOwhMUqB5e1i0QbSrIh4pkiDiBGaj7/Wd5+k+7f0qxDToZz4cNfigFOxHU6FsDLOzQEgY7LTfBItwuFGZUjgFYdEoYGxcB+miHExGxNC9drcjtHIPaKr9/AG1RipYJtvIJfwAoHY9BUdZdpM7mKTvBOASUJCU3Ib8GZxUAprWuO3Qe1kpoxzYHPaKM5itjXsYnAdl+PdjISK0crM1IGTuo9AsF/W72WE1kig0152deHeq66q0hUOaJGifK/iUyJnyD+sf2MLBmKzFOyeTd9pIssuk4X0M3asv9ZZFwZrUeH/mGda1fOanxVYSqtEvaqFSBeSDFA4JRyTNU/WN9+EjS78DUgS8mCcUZ/8E4R8E21mi7GGc34EdEr/ZEHPrtr0csbrCYF18jNvV/r1B7H4ETLptFM5v0ALsPR1HGjJjJtB0iVO0yg0D+q0cUvqieSRHDx53PbEObwgQgOg10IfwiBwUL6hynoqRhyIZHfYMsoGtwxBTIfGLsZXChpnMMF8hcuqHbTC71cBkTJIf853zWRDQ9zS2FzU/tfEaTXoFqF91YSbEAxEi3VA1gtN5r5bBA5/d8PtwpJovPI4qpVOIyEi0DIOq1NvxtP6a4ENuvV32RaURlQkvWbK0clJ+knQ1OK3EyjeMtBmMwd6yN9ct5CTV7rOjyJ+z4nCb/k4OvG7aOmc6HY3Jy1ZUjY=";
            Spire.License.LicenseProvider.SetLicenseKey(key);

        }

        public static Stream convertPdfToWord(byte[] wordBytes, string userName)
        {
            string key = "JwEAqs+49xYH9JRaADQBK8clme6iYLi6Ki/uAGlrgB0S/vBTdA5SNHz2DY2+pKkfSr1GE0xrTNHqVNUukethFNbkQPRze/oyJT3k9ai1zvt7qCmQP9JgQJgF5TjTaf2HheI/paUnoOTrRDfAP3c4M52gvvAbRmkh3mpXDnykBfFF1Q89wf1eQqaIODc30cvtU2dW/rvFiPhhgpTK2BBN/gprk+zB5sjC2DHZC2nKtMzOGNn3Jxwbd8XJPRKMH7nnM9cP5O8TRLpaPLs6dYfbCH4bnXc07biJ13FiNPjYP0MuiBuSmWjohxTMRmdgmPyY3hQVquRIJomME+s3jcRJ6vLV1exsEOqtUAF2JJTwrQgrrPW3O99QgQJpoXIA8MUL9HYnDuQtnaPTHu9bqGvNx/+AUcm9RfcnEWRK3lH19Tc2lZMz0NEYXEC6WR7717TLSO1NvHRgPWQAb6NeLkRIiQReR6Cmjchu9dsYJcwdMQc2dIlaUOOnUw3Rxo8yd4lm1UconeAdE87kkEaICKSXmlNRwJC2JoGoDdSLUmitkw4oIgtqZUViqhppKbZgZBUxW8TJS49BRjGZprqcteuIe5PrX8oZyttUnHsytCv3OTwExeQSXcTA0ONGvoT9iKOJRdeOpqKU8lNcHEt6UeR5rWtk1/F4kdusiJNATpx2d6l2gvF+xZnNnZ40yrQXTwBNiNCN299hAoY10/do8BJFQYfCVlVArMTqcwOdVxsMcyWLQWUmKd4nnmZSV5QwFFFsGPBedygmXFdDvgGGSp3xxPg22SPS63q3l52Qnd+JH1CVO08eoOwhMUqB5e1i0QbSrIh4pkiDiBGaj7/Wd5+k+7f0qxDToZz4cNfigFOxHU6FsDLOzQEgY7LTfBItwuFGZUjgFYdEoYGxcB+miHExGxNC9drcjtHIPaKr9/AG1RipYJtvIJfwAoHY9BUdZdpM7mKTvBOASUJCU3Ib8GZxUAprWuO3Qe1kpoxzYHPaKM5itjXsYnAdl+PdjISK0crM1IGTuo9AsF/W72WE1kig0152deHeq66q0hUOaJGifK/iUyJnyD+sf2MLBmKzFOyeTd9pIssuk4X0M3asv9ZZFwZrUeH/mGda1fOanxVYSqtEvaqFSBeSDFA4JRyTNU/WN9+EjS78DUgS8mCcUZ/8E4R8E21mi7GGc34EdEr/ZEHPrtr0csbrCYF18jNvV/r1B7H4ETLptFM5v0ALsPR1HGjJjJtB0iVO0yg0D+q0cUvqieSRHDx53PbEObwgQgOg10IfwiBwUL6hynoqRhyIZHfYMsoGtwxBTIfGLsZXChpnMMF8hcuqHbTC71cBkTJIf853zWRDQ9zS2FzU/tfEaTXoFqF91YSbEAxEi3VA1gtN5r5bBA5/d8PtwpJovPI4qpVOIyEi0DIOq1NvxtP6a4ENuvV32RaURlQkvWbK0clJ+knQ1OK3EyjeMtBmMwd6yN9ct5CTV7rOjyJ+z4nCb/k4OvG7aOmc6HY3Jy1ZUjY=";
            Spire.License.LicenseProvider.SetLicenseKey(key);

            string tempFolderPath = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\\") + "TempFiles" + "\\";
             var path = tempFolderPath + userName; 
            Document doc = new Document();
            Stream wordstream = new MemoryStream(wordBytes);
            //Pass path of Word Document in LoadFromFile method  
            doc.LoadFromStream(wordstream,FileFormat.Docx2013);
            //Pass Document Name and FileFormat of Document as Parameter in SaveToFile Method  
            doc.SaveToFile(path+".PDF", FileFormat.PDF);
            byte[] bPDF = System.IO.File.ReadAllBytes(path+".PDF");
            System.IO.File.Delete(path + ".PDF");
            Stream stream = new MemoryStream(bPDF);

            return stream;
        }
    }
}