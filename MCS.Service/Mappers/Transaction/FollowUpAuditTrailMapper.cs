using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class FollowUpAuditTrailMapper
    {
        public static List<FollowUpAuditTrailDTO> Map(IList<FollowUpAuditTrail> oListFollowUpAuditTrail)
        {
            if (oListFollowUpAuditTrail == null || !oListFollowUpAuditTrail.Any())
            {
                return new List<FollowUpAuditTrailDTO>();
            }

            List<FollowUpAuditTrailDTO> oListFollowUpAuditTrailDTO = oListFollowUpAuditTrail
                .Select(fdomain => new FollowUpAuditTrailDTO()
                {
                    Id = fdomain.Id,
                    FollowupId = fdomain.FollowupId,
                    ProccessId = fdomain.ProccessId,
                    ProccessDescription = fdomain.ProccessDescription,
                    ProccessDate = fdomain.ProccessDate,
                    EntityId = fdomain.EntityId,
                    UserId = fdomain.UserId,
                    User = new UserProfileDTO { LocalName = fdomain.User.LocalName },
                    Entity = new OrgUnitDTO { Name = fdomain.Entity.LocalName },

                }).ToList();

            return oListFollowUpAuditTrailDTO;
        }

        public static IList<FollowUpAuditTrail> Map(IList<FollowUpAuditTrailDTO> oListFollowUpAuditTrailDTO)
        {
            if (oListFollowUpAuditTrailDTO == null || !oListFollowUpAuditTrailDTO.Any())
            {
                return new List<FollowUpAuditTrail>();
            }

            List<FollowUpAuditTrail> list = oListFollowUpAuditTrailDTO
                .Select(fdto => new FollowUpAuditTrail()
                {
                    Id = fdto.Id,
                    FollowupId = fdto.FollowupId,
                    ProccessId = fdto.ProccessId,
                    ProccessDescription = fdto.ProccessDescription,
                    ProccessDate = fdto.ProccessDate,
                    EntityId = fdto.EntityId,
                    UserId = fdto.UserId,
                    User = new UserProfile { LocalName = fdto.User.LocalName },
                    Entity = new OrgUnit { LocalName = fdto.Entity.Name },
                }).ToList();

            return list;
        }

        public static FollowUpAuditTrail Map(FollowUpAuditTrailDTO FollowUpAuditTrailDTO)
        {
            if (FollowUpAuditTrailDTO == null)
            {
                return new FollowUpAuditTrail();
            }

            FollowUpAuditTrail oFollowUpAuditTrail = new FollowUpAuditTrail()
            {
                Id = FollowUpAuditTrailDTO.Id,
                FollowupId = FollowUpAuditTrailDTO.FollowupId,
                ProccessId = FollowUpAuditTrailDTO.ProccessId,
                ProccessDescription = FollowUpAuditTrailDTO.ProccessDescription,
                ProccessDate = FollowUpAuditTrailDTO.ProccessDate,
                EntityId = FollowUpAuditTrailDTO.EntityId,
                UserId = FollowUpAuditTrailDTO.UserId,

            };

            return oFollowUpAuditTrail;
        }

        public static IList<FollowUpAuditTrail> Map(List<TransactionFollowUp> oListFollowUpAuditTrailDTO, Dictionary<int, string> proccessDescriptions)
        {
            if (oListFollowUpAuditTrailDTO == null || !oListFollowUpAuditTrailDTO.Any())
            {
                return new List<FollowUpAuditTrail>();
            }
            string publicDescription = proccessDescriptions.Where(x => x.Key == (int)FollowupType.Public).FirstOrDefault().Value;
            string privateDescription = proccessDescriptions.Where(x => x.Key == (int)FollowupType.Privet).FirstOrDefault().Value;
            List<FollowUpAuditTrail> list = oListFollowUpAuditTrailDTO
                .Select(fdto => new FollowUpAuditTrail()
                {
                    FollowupId = fdto.Id,
                    ProccessId = fdto.FollowUpTypeId == (int)FollowupType.Privet ? (int)FollowupAuditProcess.AddPrivetFollowup : (int)FollowupAuditProcess.AddPublicFollowup,
                    ProccessDescription = fdto.FollowUpTypeId == (int)FollowupType.Privet ? privateDescription : publicDescription,
                    ProccessDate = DateTime.Now,
                    EntityId = fdto.CreatingEntityId,
                    UserId = fdto.CreatingUserId,

                }).ToList();

            return list;
        }

    }
}