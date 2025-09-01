using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.ExternalParties;

namespace MCS.UI.Areas.User.Mappers.ExternalParties
{
    public static class AddressMapper
    {
        public static List<AddressVM> Map(IList<AddressDTO> addressDTOs)
        {
            if (addressDTOs == null || !addressDTOs.Any())
            {
                return new List<AddressVM>();
            }
            List<AddressVM> addressVMs = addressDTOs
                .Select(addressDTO => new AddressVM()
                { 
                    Id = addressDTO.Id,
                    CultureId = addressDTO.CultureId,
                    CultureName = addressDTO.CultureName,
                    Text = addressDTO.Text
                }).ToList();

            return addressVMs;
        }
        public static List<AddressDTO> Map(IList<AddressVM> addressVMs)
        {
            if (addressVMs == null || !addressVMs.Any())
            {
                return new List<AddressDTO>();
            }
            List<AddressDTO> addressDTOs = addressVMs
                .Select(addressVM => new AddressDTO()
                {
                    Id = addressVM.Id,
                    CultureId = addressVM.CultureId,
                    CultureName = addressVM.CultureName,
                    Text = addressVM.Text
                }).ToList();

            return addressDTOs;
        }
    }
}