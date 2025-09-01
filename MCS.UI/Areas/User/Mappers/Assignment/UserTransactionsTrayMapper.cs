using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Models.Assignment;

namespace MCS.UI.Areas.User.Mappers.Assignment
{
    public static class UserTransactionsTrayMapper
    {
        public static List<UserTransactionsTrayVM> Map(IList<UserTransactionsTrayDTO> userTransactionsTrayDTOs)
        {
            if (userTransactionsTrayDTOs == null || !userTransactionsTrayDTOs.Any())
            {
                return new List<UserTransactionsTrayVM>();
            }
            List<UserTransactionsTrayVM> userTransactionsTrayVMs = userTransactionsTrayDTOs
                .Select(userTransactionsTrayDTO => new UserTransactionsTrayVM()
                { 
                    Id = userTransactionsTrayDTO.Id,
                    ConfedentialityId = userTransactionsTrayDTO.ConfedentialityId,
                    Date = userTransactionsTrayDTO.Date,
                    DateH = userTransactionsTrayDTO.DateH,
                    DocumentNumber = userTransactionsTrayDTO.DocumentNumber,
                    FromEntity = OrgUnitMapper.Map(userTransactionsTrayDTO.FromEntity),
                    FromUser = UserProfileMapper.Map(userTransactionsTrayDTO.FromUser),
                    Islate = userTransactionsTrayDTO.Islate,
                    Number = userTransactionsTrayDTO.Number,
                    PriorityLevel = PriorityMapper.Map(userTransactionsTrayDTO.PriorityLevel),
                    RemindDate = userTransactionsTrayDTO.RemindDate,
                    RemindDateH = userTransactionsTrayDTO.RemindDateH,
                    StatusId = userTransactionsTrayDTO.StatusId,
                    ToEntity = OrgUnitMapper.Map(userTransactionsTrayDTO.ToEntity),
                    ToUser = UserProfileMapper.Map(userTransactionsTrayDTO.ToUser),
                    TransactionCategoryId = userTransactionsTrayDTO.TransactionCategoryId
                }).ToList();

            return userTransactionsTrayVMs;
        }
        public static List<UserTransactionsTrayDTO> Map(IList<UserTransactionsTrayVM> userTransactionsTrayVMs)
        {
            if (userTransactionsTrayVMs == null || !userTransactionsTrayVMs.Any())
            {
                return new List<UserTransactionsTrayDTO>();
            }
            List<UserTransactionsTrayDTO> userTransactionsTrayDTOs = userTransactionsTrayVMs
                .Select(userTransactionsTrayVM => new UserTransactionsTrayDTO()
                { 
                    Id = userTransactionsTrayVM.Id,
                    ConfedentialityId = userTransactionsTrayVM.ConfedentialityId,
                    Date = userTransactionsTrayVM.Date,
                    DateH = userTransactionsTrayVM.DateH,
                    DocumentNumber = userTransactionsTrayVM.DocumentNumber,
                    FromEntity = OrgUnitMapper.Map(userTransactionsTrayVM.FromEntity),
                    FromUser = UserProfileMapper.Map(userTransactionsTrayVM.FromUser),
                    Islate = userTransactionsTrayVM.Islate,
                    Number = userTransactionsTrayVM.Number,
                    PriorityLevel = PriorityMapper.Map(userTransactionsTrayVM.PriorityLevel),
                    RemindDate = userTransactionsTrayVM.RemindDate,
                    RemindDateH = userTransactionsTrayVM.RemindDateH,
                    StatusId = userTransactionsTrayVM.StatusId,
                    ToEntity = OrgUnitMapper.Map(userTransactionsTrayVM.ToEntity),
                    ToUser = UserProfileMapper.Map(userTransactionsTrayVM.ToUser),
                    TransactionCategoryId = userTransactionsTrayVM.TransactionCategoryId
                }).ToList();

            return userTransactionsTrayDTOs;
        }
    }
}