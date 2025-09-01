using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class WordAddInRepository : BaseRepository<WordAddInTemp>, IWordAddInRepository
    {
        #region Attributes

        #endregion Attributes

        #region Constructors

        public WordAddInRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods  

        public void SaveTempDocument(byte[] data, string filename)
        {
            filename = ExtractFileNameWithoutExtention(filename);
            var oldWord = _oMCSDbContext.WordAddInTemps.Where(x => x.FileName == filename).FirstOrDefault();
            if (oldWord == null)
            {
                oldWord = new WordAddInTemp
                {
                    Content = data,
                    FileName = filename,
                    CreatedOn = DateTime.Now,
                    IsRead = false
                };

                _oMCSDbContext.WordAddInTemps.Add(oldWord);
                _oMCSDbContext.SaveChanges();
            }
            else
            {
                oldWord.Content = data;
                oldWord.ModefiedOn = DateTime.Now;
                oldWord.IsRead = false;
                _oMCSDbContext.SaveChanges();
            }


        }

        public WordAddInTemp GetTempDocument(string filename)
        {
            filename = ExtractFileNameWithoutExtention(filename);
            var oldWord = _oMCSDbContext.WordAddInTemps.Where(x => x.FileName == filename).FirstOrDefault();
            if (oldWord != null)
            {
                return oldWord;
            }


            return null;
        }

        public void MarkDocumentAsRead(string filename)
        {
            filename = ExtractFileNameWithoutExtention(filename);
            var oldWord = _oMCSDbContext.WordAddInTemps.Where(x => x.FileName == filename).FirstOrDefault();
            if (oldWord != null)
            {
                oldWord.IsRead = true;
                oldWord.Content = null;
                _oMCSDbContext.SaveChanges();

            }

        }

        public void UpdateTempDocument(byte[] data, byte[] pdf, string filename)
        {
            filename = ExtractFileNameWithoutExtention(filename);
            var oldWord = _oMCSDbContext.WordAddInTemps.Where(x => x.FileName == filename).FirstOrDefault();
            if (oldWord != null)
            {
                oldWord.Content = data;
                oldWord.ContentPDF = pdf;
                _oMCSDbContext.SaveChanges();

            }

        }



        public WordAddInTemp GetTempDocumentByUserId(int userId)
        {
            var wordAddInTemp = _oMCSDbContext.WordAddInTemps.Where(x => x.CreatedBy == userId).FirstOrDefault();

            return wordAddInTemp;

        }

        private string ExtractFileNameWithoutExtention(string path)
        {
            string fileName = Path.GetFileName(path);
            //int lastIndex = fileName.LastIndexOf(".");
            //if (lastIndex != -1)
            //{
            //    fileName = fileName.Substring(0, lastIndex);
            //}
            fileName = fileName.Replace(".docx", "");
            fileName = fileName.Replace(".doc", "");

            return fileName;
        }
        #endregion Methods
    }

}
