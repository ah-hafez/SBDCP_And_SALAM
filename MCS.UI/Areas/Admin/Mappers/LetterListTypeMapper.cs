using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class LetterListTypeMapper
    {
        public static LetterListTypeDTO Map(LetterListTypeVM letterListTypeVM)
        {
            if (letterListTypeVM != null)
            {
                return new LetterListTypeDTO()
                {
                    Id = letterListTypeVM.Id,
                    IsSelected = letterListTypeVM.IsSelected,
                    Text = letterListTypeVM.Text
                };
            }
            return null;
        }
        public static LetterListTypeVM Map(LetterListTypeDTO letterListTypeDTO)
        {
            if (letterListTypeDTO != null)
            {
                return new LetterListTypeVM()
                {
                    Id = letterListTypeDTO.Id,
                    IsSelected = letterListTypeDTO.IsSelected,
                    Text = letterListTypeDTO.Text
                };
            }
            return null;
        }
        public static List<LetterListTypeDTO> Map(IList<LetterListTypeVM> letterListTypeVMs)
        {
            if (letterListTypeVMs == null || !letterListTypeVMs.Any())
            { return null; }
            List<LetterListTypeDTO> letterListTypeDTOs = letterListTypeVMs
                .Select(b => new LetterListTypeDTO
                { 
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    Text = b.Text

                }).ToList();
            return letterListTypeDTOs;
        }
        public static List<LetterListTypeVM> Map(IList<LetterListTypeDTO> letterListTypeDTOs)
        {
            if (letterListTypeDTOs == null || !letterListTypeDTOs.Any())
            {
                return null;
            }
            List<LetterListTypeVM> letterListTypeVMs = letterListTypeDTOs
                .Select(b => new LetterListTypeVM
                { 
                    
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    Text = b.Text

                }).ToList();
            return letterListTypeVMs;
        }

    }
}