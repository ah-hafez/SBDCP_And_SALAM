using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction.Outbound
{
    public static class TransactionNameMapper
    {
        public static List<TransactionNameVM> Map(IList<TransactionNameDTO> transactionNameDTOs)
        {
            if (transactionNameDTOs == null || !transactionNameDTOs.Any())
            {
                return new List<TransactionNameVM>();
            }
            List<TransactionNameVM> transactionNameVMs = transactionNameDTOs
                .Select(transactionNameDTO => new TransactionNameVM()
                {
                    Address = transactionNameDTO.Address,
                    CivilID = transactionNameDTO.CivilID,
                    OtherInformation = transactionNameDTO.OtherInformation,
                    Email = transactionNameDTO.Email,
                    FirstName = transactionNameDTO.FirstName,
                    Id = transactionNameDTO.Id,
                    MobileNumber = transactionNameDTO.MobileNumber,
                    NationalityId = transactionNameDTO.NationalityId,
                    TitleId=transactionNameDTO.TitleId,
                    Phone = transactionNameDTO.Phone,
                    City = transactionNameDTO.City,
                    Gender = transactionNameDTO.Gender,
                    RelativeRelation = transactionNameDTO.RelativeRelation,
                }).ToList();

            return transactionNameVMs;
        }
        public static List<TransactionNameDTO> Map(IList<TransactionNameVM> transactionNameVMs)
        {
            if (transactionNameVMs == null || !transactionNameVMs.Any())
            {
                return new List<TransactionNameDTO>();
            }
            List<TransactionNameDTO> transactionNameDTOs = transactionNameVMs
                .Select(transactionNameVM => new TransactionNameDTO()
                {
                    Address = transactionNameVM.Address,
                    CivilID = transactionNameVM.CivilID,
                    OtherInformation = transactionNameVM.OtherInformation,
                    Email = transactionNameVM.Email,
                    FirstName = transactionNameVM.FirstName,
                    Id = transactionNameVM.Id,
                    MobileNumber = transactionNameVM.MobileNumber,
                    NationalityId = transactionNameVM.NationalityId,
                    Phone = transactionNameVM.Phone,
                    City = transactionNameVM.City,
                    Gender = transactionNameVM.Gender,
                    TitleId = transactionNameVM.TitleId,
                    RelativeRelation = transactionNameVM.RelativeRelation,
                    SendSMS = transactionNameVM.SendSMS
                }).ToList();

            return transactionNameDTOs;
        }
        public static TransactionNameDTO Map(TransactionNameVM transactionNameVM)
        {
            if (transactionNameVM != null)
            {
                TransactionNameDTO transactionNameDTO = new TransactionNameDTO()
                {
                    Address = transactionNameVM.Address,
                    CivilID = transactionNameVM.CivilID,
                    OtherInformation = transactionNameVM.OtherInformation,
                    Email = transactionNameVM.Email,
                    FirstName = transactionNameVM.FirstName,
                    Id = transactionNameVM.Id,
                    MobileNumber = transactionNameVM.MobileNumber,
                    NationalityId = transactionNameVM.NationalityId,
                    Phone = transactionNameVM.Phone,
                    City = transactionNameVM.City,
                    Gender = transactionNameVM.Gender,
                    TitleId = transactionNameVM.TitleId,
                    RelativeRelation = transactionNameVM.RelativeRelation,
                };

                return transactionNameDTO;
            }
            return new TransactionNameDTO();
        }
        public static TransactionNameVM Map(TransactionNameDTO transactionNameDTO)
        {
            if (transactionNameDTO != null)
            {
                TransactionNameVM transactionNameVM = new TransactionNameVM()
                {
                    Address = transactionNameDTO.Address,
                    CivilID = transactionNameDTO.CivilID,
                    OtherInformation = transactionNameDTO.OtherInformation,
                    Email = transactionNameDTO.Email,
                    FirstName = transactionNameDTO.FirstName,
                    Id = transactionNameDTO.Id,
                    MobileNumber = transactionNameDTO.MobileNumber,
                    NationalityId = transactionNameDTO.NationalityId,
                    Phone = transactionNameDTO.Phone,
                    City = transactionNameDTO.City,
                    Gender = transactionNameDTO.Gender,
                    TitleId = transactionNameDTO.TitleId,
                    RelativeRelation = transactionNameDTO.RelativeRelation,
                };

                return transactionNameVM;
            }
            return new TransactionNameVM();
        }
    }
}