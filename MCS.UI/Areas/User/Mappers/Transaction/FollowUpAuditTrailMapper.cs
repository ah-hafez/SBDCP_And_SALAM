using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.DTO;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public class FollowUpAuditTrailMapper
    {
        public static List<FollowUpAuditTrailVM> Map(List<FollowUpAuditTrailDTO> oListFollowUpAuditTrailDTO)
        {
            if (oListFollowUpAuditTrailDTO == null || !oListFollowUpAuditTrailDTO.Any())
            {
                return new List<FollowUpAuditTrailVM>();
            }

            List<FollowUpAuditTrailVM> oListFollowUpAuditTrailVM = oListFollowUpAuditTrailDTO
                .Select(fdto => new FollowUpAuditTrailVM()
                {
                    FollowupId = fdto.FollowupId,
                    ProccessId = fdto.ProccessId,
                    ProccessDescription = fdto.ProccessDescription,
                    ProccessDate = fdto.ProccessDate,
                    ProccessDateHj = DateTimeUtility.ConvertToUmAlQuraCalendar(fdto.ProccessDate),
                    EntityId = fdto.EntityId,
                    UserId = fdto.UserId,
                    UserName = fdto.User.LocalName,
                    EntityName =fdto.Entity.Name,
                    UserEntityName = fdto.User.LocalName + '/' + fdto.Entity.Name,

                }).ToList();

            FollowUpAuditTrailVM oFollowUpAuditTrailVM = new FollowUpAuditTrailVM();

            return oListFollowUpAuditTrailVM;
        }

        public static List<FollowUpAuditTrailDTO> Map(IList<FollowUpAuditTrailVM> oListFollowUpAuditTrailVM)
        {
            if (oListFollowUpAuditTrailVM == null || !oListFollowUpAuditTrailVM.Any())
            {
                return new List<FollowUpAuditTrailDTO>();
            }

            List<FollowUpAuditTrailDTO> oListFollowUpAuditTrailDTO = new List<FollowUpAuditTrailDTO>();
            foreach (var item in oListFollowUpAuditTrailVM)
            {
                oListFollowUpAuditTrailDTO.Add(Map(item));
            }

            return oListFollowUpAuditTrailDTO;
        }

        public static FollowUpAuditTrailDTO Map(FollowUpAuditTrailVM oFollowUpAuditTrailVM)
        {
            if (oFollowUpAuditTrailVM == null)
            {
                return new FollowUpAuditTrailDTO();
            }

            FollowUpAuditTrailDTO oFollowUpAuditTrailDTO = new FollowUpAuditTrailDTO
            {
                Id = oFollowUpAuditTrailVM.FollowupId,
                FollowupId = oFollowUpAuditTrailVM.FollowupId,
                ProccessId = oFollowUpAuditTrailVM.ProccessId,
                ProccessDescription = oFollowUpAuditTrailVM.ProccessDescription,
                ProccessDate = oFollowUpAuditTrailVM.ProccessDate,
                EntityId = oFollowUpAuditTrailVM.EntityId,
                UserId = oFollowUpAuditTrailVM.UserId,
                

            };


            return oFollowUpAuditTrailDTO;
        }

     }
}