using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;
using MCS.Domain.IC;

namespace MCS.Service.Mappers
{
    public static class IC_SUBJECTMapper
    {
        public static IC_SUBJECT Map(IC_SUBJECTDTO addIC_SUBJECTDTO)
        {
            if (addIC_SUBJECTDTO == null)
                return null;

            IC_SUBJECT icSubject = new IC_SUBJECT()
            {
                ACTIVE = addIC_SUBJECTDTO.ACTIVE,
                ITEM_CODE = addIC_SUBJECTDTO.ITEM_CODE,
                ITEM_DESCRIPTION_AR = addIC_SUBJECTDTO.ITEM_DESCRIPTION_AR,
                ITEM_DISPLAY = addIC_SUBJECTDTO.ITEM_DISPLAY,
                PARENT_ID = addIC_SUBJECTDTO.PARENT_ID,
                Id = addIC_SUBJECTDTO.Id,
                Number = addIC_SUBJECTDTO.DirectoryNum,
                //ClassificationId = addIC_SUBJECTDTO.ClassificationId,
                CONFID_ID = addIC_SUBJECTDTO.CONFID_ID,
                FULL_CODE = addIC_SUBJECTDTO.FULL_CODE,
                IcIndexId = addIC_SUBJECTDTO.IcIndexId,
                IS_USED = addIC_SUBJECTDTO.IS_USED,
                Closed = addIC_SUBJECTDTO.Closed

            };

            return icSubject;
        }

        public static IC_SUBJECTDTO Map(IC_SUBJECT icSubject)
        {
            if (icSubject == null)
                return null;

            IC_SUBJECTDTO icSubjectDto = new IC_SUBJECTDTO()
            {
                ACTIVE = icSubject.ACTIVE,
                ITEM_CODE = icSubject.ITEM_CODE,
                ITEM_DESCRIPTION_AR = icSubject.ITEM_DESCRIPTION_AR,
                ITEM_DISPLAY = icSubject.ITEM_DISPLAY,
                PARENT_ID = icSubject.PARENT_ID,
                Id = icSubject.Id,
                HasChilds = icSubject.HasChilds,
                DirectoryNum = icSubject.Number

            };

            return icSubjectDto;
        }
        public static List<IC_SUBJECTDTO> Map(IList<IC_SUBJECT> icSubjects)
        {
            if (icSubjects == null)
                return null;

            List<IC_SUBJECTDTO> icSubjectsResult = new List<IC_SUBJECTDTO>();

            foreach (var item in icSubjects)
            {
                icSubjectsResult.Add(IC_SUBJECTMapper.Map(item));
            }

            return icSubjectsResult;
        }


        public static IC_SUBJECTTransactionDTO Map(IC_SUBJECTS_TRANSACTION input)
        {
            if (input == null)
                return null;

            return new IC_SUBJECTTransactionDTO
            {
                Description = input.Description,
                IcId = input.Id,
                Number = input.Number,
                TransactionId = input.TransactionId,
                Part=input.Part,
            };
        }

        public static List<ClassificationDto> Map(IList<IC_CLASSIFICATION> iC_CLASSIFICATIONs)
        {
            if (iC_CLASSIFICATIONs == null)
                return null;

            List<ClassificationDto> classificationDtos = new List<ClassificationDto>();
            classificationDtos = iC_CLASSIFICATIONs.Select(c => new ClassificationDto
            {
                Id = c.Id,
                Name = c.DESCRIPTION_AR


            }).ToList();

            return classificationDtos;
        }





    }
}