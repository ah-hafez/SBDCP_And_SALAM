using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class LetterTypeListMapper
    {
        public static LetterListType Map(IList<LetterListTypeDTO> letterListTypeDTOs)
        {
            if (letterListTypeDTOs == null || !letterListTypeDTOs.Any())
            {
                return LetterListType.None;
            }

            LetterListType letterListType = LetterListType.None;

            foreach (LetterListTypeDTO letterListTypeDTO in letterListTypeDTOs)
            {
                if (letterListTypeDTO.IsSelected)
                {
                    letterListType = letterListType ^ (LetterListType)letterListTypeDTO.Id;
                }
            }

            return letterListType;
        }

        public static List<LetterListTypeDTO> Map(LetterListType letterListType)
        {
            if (letterListType == LetterListType.None)
            {
                return null;
            }
            List<LetterListTypeDTO> letterListTypeDTOs = new List<LetterListTypeDTO>();

            foreach (LetterListType item in Enum.GetValues(typeof(LetterListType)))
            {
                LetterListTypeDTO letterListTypeDTO = new LetterListTypeDTO();

                letterListTypeDTO.Id = (int)item;

                if (Convert.ToBoolean((LetterListType)letterListType & item))
                {
                    letterListTypeDTO.IsSelected = true;
                }

                letterListTypeDTOs.Add(letterListTypeDTO);
            }

            return letterListTypeDTOs;
        }
    }
}