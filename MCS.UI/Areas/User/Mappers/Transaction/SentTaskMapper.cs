using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class SentTaskMapper
    {
        public static List<SentTaskVM> Map(IList<SentTaskDTO> sentTaskDTOs)
        {
            if (sentTaskDTOs == null || !sentTaskDTOs.Any())
            {
                return new List<SentTaskVM>();
            }
            List<SentTaskVM> sentTaskVMs = sentTaskDTOs
                .Select(sentTaskDTO => new SentTaskVM()
                { 
                    DeliveryDate = sentTaskDTO.DeliveryDate,
                    DeliveryDateH = sentTaskDTO.DeliveryDateH,
                    Id = sentTaskDTO.Id,
                    Notes = sentTaskDTO.Notes,
                    Status = sentTaskDTO.Status,
                    StatusName = sentTaskDTO.StatusName,
                    TaskDescription = sentTaskDTO.TaskDescription,
                    ToOrgUnit = sentTaskDTO.ToOrgUnit,
                    ToUser = sentTaskDTO.ToUser,
                    TransactionNumber = sentTaskDTO.TransactionNumber
                }).ToList();

            return sentTaskVMs;
        }
        public static List<SentTaskDTO> Map(IList<SentTaskVM> sentTaskVMs)
        {
            if (sentTaskVMs == null || !sentTaskVMs.Any())
            {
                return new List<SentTaskDTO>();
            }
            List<SentTaskDTO> sentTaskDTOs = sentTaskVMs
                .Select(sentTaskVM => new SentTaskDTO()
                { 
                    DeliveryDate = sentTaskVM.DeliveryDate,
                    DeliveryDateH = sentTaskVM.DeliveryDateH,
                    Id = sentTaskVM.Id.Value,
                    Notes = sentTaskVM.Notes,
                    Status = sentTaskVM.Status,
                    StatusName = sentTaskVM.StatusName,
                    TaskDescription = sentTaskVM.TaskDescription,
                    ToOrgUnit = sentTaskVM.ToOrgUnit,
                    ToUser = sentTaskVM.ToUser,
                    TransactionNumber = sentTaskVM.TransactionNumber
                }).ToList();

            return sentTaskDTOs;
        }
        public static SentTaskDTO Map(SentTaskVM sentTaskVM)
        {
            if (sentTaskVM != null)
            {
                return new SentTaskDTO()
                {
                    DeliveryDate = sentTaskVM.DeliveryDate,
                    DeliveryDateH = sentTaskVM.DeliveryDateH,
                    Id = sentTaskVM.Id.Value,
                    Notes = sentTaskVM.Notes,
                    Status = sentTaskVM.Status,
                    StatusName = sentTaskVM.StatusName,
                    TaskDescription = sentTaskVM.TaskDescription,
                    ToOrgUnit = sentTaskVM.ToOrgUnit,
                    ToUser = sentTaskVM.ToUser,
                    TransactionNumber = sentTaskVM.TransactionNumber
                };
            }
            return new SentTaskDTO();
        }
        public static SentTaskVM Map(SentTaskDTO sentTaskDTO)
        {
            if (sentTaskDTO != null)
            {
                return new SentTaskVM() 
                { 
                    DeliveryDate = sentTaskDTO.DeliveryDate,
                    DeliveryDateH = sentTaskDTO.DeliveryDateH,
                    Id = sentTaskDTO.Id,
                    Notes = sentTaskDTO.Notes,
                    Status = sentTaskDTO.Status,
                    StatusName = sentTaskDTO.StatusName,
                    TaskDescription = sentTaskDTO.TaskDescription,
                    ToOrgUnit = sentTaskDTO.ToOrgUnit,
                    ToUser = sentTaskDTO.ToUser,
                    TransactionNumber = sentTaskDTO.TransactionNumber
                };
            }
            return new SentTaskVM();
        }
    }
}