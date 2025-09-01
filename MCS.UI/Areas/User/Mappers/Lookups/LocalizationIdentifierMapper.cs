using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class LocalizationIdentifierMapper
    {
        public static List<LocalizationIdentifierVM> Map(IList<LocalizationIdentifierDTO> localizationIdentifierDTOs)
        {
            if (localizationIdentifierDTOs == null || !localizationIdentifierDTOs.Any())
            {
                return new List<LocalizationIdentifierVM>();
            }
            List<LocalizationIdentifierVM> localizationIdentifierVMs = localizationIdentifierDTOs
                .Select(localizationIdentifierDTO => new LocalizationIdentifierVM()
                { 
                    Id = localizationIdentifierDTO.Id,
                    ColumnName = localizationIdentifierDTO.ColumnName,
                    TableName = localizationIdentifierDTO.TableName
                }).ToList();
            return localizationIdentifierVMs;
        }
        public static List<LocalizationIdentifierDTO> Map(IList<LocalizationIdentifierVM> localizationIdentifierVMs)
        {
            if (localizationIdentifierVMs == null || !localizationIdentifierVMs.Any())
            {
                return new List<LocalizationIdentifierDTO>();
            }
            List<LocalizationIdentifierDTO> localizationIdentifierDTOs = localizationIdentifierVMs
                .Select(localizationIdentifierVM => new LocalizationIdentifierDTO()
                { 
                    Id = localizationIdentifierVM.Id,
                    ColumnName = localizationIdentifierVM.ColumnName,
                    TableName = localizationIdentifierVM.TableName
                }).ToList();
            return localizationIdentifierDTOs;
        }
    }
}