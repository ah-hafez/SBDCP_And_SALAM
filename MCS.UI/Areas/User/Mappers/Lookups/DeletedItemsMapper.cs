using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class DeletedItemsMapper
    {
        public static List<DeletedItemsVM> Map(IList<DeletedItemsDTO> deletedItemsDTOs)
        {
            if (deletedItemsDTOs == null || !deletedItemsDTOs.Any())
            {
                return new List<DeletedItemsVM>();
            }
            List<DeletedItemsVM> deletedItemsVMs = deletedItemsDTOs
                .Select(deletedItemsDTO => new DeletedItemsVM()
                { 
                    DeletedList = deletedItemsDTO.DeletedList
                }).ToList();

            return deletedItemsVMs;
        }
        public static List<DeletedItemsDTO> Map(IList<DeletedItemsVM> deletedItemsVMs)
        {
            if (deletedItemsVMs == null || !deletedItemsVMs.Any())
            {
                return new List<DeletedItemsDTO>();
            }
            List<DeletedItemsDTO> deletedItemsDTOs = deletedItemsVMs
                .Select(deletedItemsVM => new DeletedItemsDTO()
                { 
                    DeletedList = deletedItemsVM.DeletedList
                }).ToList();

            return deletedItemsDTOs;
        }

    }
}