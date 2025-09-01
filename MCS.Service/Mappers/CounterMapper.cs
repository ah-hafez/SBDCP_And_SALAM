using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class CounterMapper
    {
        public static Counter Map(CounterDTO counterDTO, string cultureName)
        {
            if (counterDTO != null)
            {
                return new Counter
                {
                    Id = counterDTO.Id,
                    IsGeneral = counterDTO.IsGeneral,
                    Year = counterDTO.Year.ToString(),
                    OwnerEntityId = counterDTO.OwnerEntityId,
                    Description = LocalizationIdentifierMapper.Map(counterDTO.Description),
                    CounterDetails = Map(counterDTO.CounterDetails, cultureName)
                };
            }
            return null;
        }
        public static CounterDTO Map(Counter counter, string cultureName)
        {
            CounterDTO counterDTO = new CounterDTO();
            if (counter != null)
            {
                counterDTO.Id = counter.Id;
                counterDTO.IsGeneral = counter.IsGeneral;
                counterDTO.Year = Convert.ToInt32(counter.Year);
                counterDTO.OwnerEntityId = counter.OwnerEntityId;
                if (counter.Description != null)
                {
                    counterDTO.Description = counter.Description.Localizations != null ? LocalizationIdentifierMapper.Map(counter.Description.Localizations) : null;
                }
                counterDTO.CounterDetails = Map(counter.CounterDetails, cultureName);
            }
            return counterDTO;
        }
        public static List<CounterDetailDTO> Map(IList<CounterDetail> counterDetails, string cultureName)
        {
            if (counterDetails == null || !counterDetails.Any())
            {
                return null;
            }
            List<CounterDetailDTO> counterDetailDTOs = new List<CounterDetailDTO>();
            foreach (var item in counterDetails)
            {
                var counterDetail = new CounterDetailDTO
                {
                    Id = item.Id,
                    InitialValue = item.InitialValue,
                    Count = item.Count,
                    TransactionCategories = TransactionCategoryMapper.Map(item.TransactionCategories, cultureName)
                };
                counterDetailDTOs.Add(counterDetail);
            }
            return counterDetailDTOs;
        }
        public static IList<CounterDetail> Map(IList<CounterDetailDTO> counterDTOs, string cultureName)
        {
            if (counterDTOs == null || !counterDTOs.Any())
            {
                return null;
            }
            List<CounterDetail> counterDetails = new List<CounterDetail>();
            foreach (var item in counterDTOs)
            {
                var counterDetail = new CounterDetail
                {
                    Id = item.Id,
                    InitialValue = item.InitialValue,
                    Count = item.Count,
                    TransactionCategories = TransactionCategoryMapper.Map(item.TransactionCategories)
                };
                counterDetails.Add(counterDetail);
            }
            return counterDetails;
        }

    }
}