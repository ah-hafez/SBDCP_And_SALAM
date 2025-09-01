using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using MCS.DTO;

namespace MCS.Business
{
    public class WordAddInBL : BaseBL, IWordAddInBL
    {


        public void SaveTempDocument(byte[] data, string filename)
        {
            try
            {

                IWordAddInRepository wordAddInRepository = IoC.Resolve<WordAddInRepository>();
                wordAddInRepository.SaveTempDocument(data, filename);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public WordAddinDocumentDTO GetTempDocument(string filename)
        {
            try
            {
                WordAddinDocumentDTO wordAddinDocumentDTO = new WordAddinDocumentDTO();
                IWordAddInRepository wordAddInRepository = IoC.Resolve<WordAddInRepository>();

                var wordTemp = wordAddInRepository.GetTempDocument(filename);
                wordAddinDocumentDTO.contentAsPDF = wordTemp.ContentPDF;
                wordAddinDocumentDTO.content = wordTemp.Content;
                return wordAddinDocumentDTO;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void MarkDocumentAsRead(string filename)
        {
            try
            {

                IWordAddInRepository wordAddInRepository = IoC.Resolve<WordAddInRepository>();
                wordAddInRepository.MarkDocumentAsRead(filename);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }



        public void UpdateTempDocument(WordAddinDocumentDTO data, string filename)
        {
            try
            {

                IWordAddInRepository wordAddInRepository = IoC.Resolve<WordAddInRepository>();
                wordAddInRepository.UpdateTempDocument(data.content, data.contentAsPDF, filename);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }


        public WordAddInTemp GetTempDocumentByUserId(int userId)
        {
            try
            {

                IWordAddInRepository wordAddInRepository = IoC.Resolve<WordAddInRepository>();
                return wordAddInRepository.GetTempDocumentByUserId(userId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
    }
}
