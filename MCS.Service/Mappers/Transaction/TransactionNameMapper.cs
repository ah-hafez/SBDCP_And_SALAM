using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionNameMapper
    {
        public static Name Map(TransactionNameDTO nameDTO)
        {
            if (nameDTO != null)
            {


                Name name = new Name()
                {
                    Id = (nameDTO.Id.HasValue) ? nameDTO.Id.Value : 0,
                    FirstName = nameDTO.FirstName,
                    Phone = nameDTO.Phone,
                    Email = nameDTO.Email,
                    Address = nameDTO.Address,
                    CivilID = nameDTO.CivilID,
                    MobileNumber = nameDTO.MobileNumber,
                    OtherInformation = nameDTO.OtherInformation,
                    City = nameDTO.City,
                    Gender = nameDTO.Gender,
                    RelativeRelation = nameDTO.RelativeRelation,
                };

                if (nameDTO.NationalityId.HasValue)
                {
                    name.NationalityId = nameDTO.NationalityId.Value;
                }
                if (nameDTO.TitleId.HasValue)
                {
                    name.TitleId = nameDTO.TitleId.Value;
                }
                return name;
            }

            return null;
        }
        public static List<Name> Map(List<TransactionNameDTO> transactionNameDTOs)
        {
            if (transactionNameDTOs == null || !transactionNameDTOs.Any())
            {
                return null;
            }


            List<Name> names = transactionNameDTOs
                .Select(transactionNameDTO => new Name()
                {
                    Id = (transactionNameDTO.Id.HasValue) ? transactionNameDTO.Id.Value : 0,
                    FirstName = transactionNameDTO.FirstName,
                    Phone = transactionNameDTO.Phone,
                    Email = transactionNameDTO.Email,
                    Address = transactionNameDTO.Address,
                    CivilID = transactionNameDTO.CivilID,
                    MobileNumber = transactionNameDTO.MobileNumber,
                    OtherInformation = transactionNameDTO.OtherInformation,
                    NationalityId = transactionNameDTO.NationalityId.Value,
                    TitleId=transactionNameDTO.TitleId.Value,
                    City = transactionNameDTO.City,
                    Gender = transactionNameDTO.Gender,
                    RelativeRelation = transactionNameDTO.RelativeRelation,
                }).ToList();

            return names;
        }
        public static TransactionNameDTO Map(Name name)
        {
            if (name != null)
            {
                TransactionNameDTO transactionNameDTO = new TransactionNameDTO()
                {
                    Id = name.Id,
                    FirstName = name.FirstName,
                    Phone = name.Phone,
                    Email = name.Email,
                    MobileNumber = name.MobileNumber,
                    CivilID = name.CivilID,
                    Address = name.Address,
                    OtherInformation = name.OtherInformation,
                    City = name.City,
                    Gender = name.Gender,
                    RelativeRelation = name.RelativeRelation,

                };

                if (name.Nationality != null)
                {
                    transactionNameDTO.NationalityId = name.Nationality.Id;
                }
                if (name.Title != null)
                {
                    transactionNameDTO.TitleId = name.Title.Id;
                }

                return transactionNameDTO;
            }
            return null;
        }
        public static List<TransactionNameDTO> Map(IList<Name> names)
        {
            if (names == null || !names.Any())
            {
                return null;
            }

            List<TransactionNameDTO> transactionNameDTO = names
                .Select(name => new TransactionNameDTO()
                {
                    Id = name.Id,
                    FirstName = name.FirstName,
                    Phone = name.Phone,
                    Email = name.Email,
                    MobileNumber = name.MobileNumber,
                    CivilID = name.CivilID,
                    Address = name.Address,
                    OtherInformation = name.OtherInformation,
                    NationalityId = name.Nationality?.Id,
                    TitleId=name.Title?.Id,
                    City = name.City,
                    Gender = name.Gender,
                    RelativeRelation = name.RelativeRelation,

                }).ToList();

            return transactionNameDTO;
        }
    }
}