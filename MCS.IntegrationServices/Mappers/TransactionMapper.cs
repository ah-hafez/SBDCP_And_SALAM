using MCS.Common;
using MCS.DTO;
using MCS.IntegrationServices.Models;
using MCS.IntegrationServices.Models.Sharepoints;
using MobileApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransactionCategory = MCS.Common.TransactionCategory;

namespace MCS.IntegrationServices.Mappers
{
    public class TransactionMapper
    {
        public static List<TransactionModel> Map(List<BasicTransactionDto> transactionDTOs, UserProfileDTO userProfileDTO)
        {

            if (transactionDTOs != null && transactionDTOs.Count > 0)
            {
                return transactionDTOs.Select(t => new TransactionModel
                {

                    //Subject = t.Subject,
                    Confidentiality = t.Confidentiality,
                    TransactionDate = t.CreatedDateH,
                    //TransactionDate = t.CreatedDate,
                    TransactionNumber = t.TransactionNumber.ToString(),
                    //TransactionType = t.TransactionType.ToString(),
                    TransactionUrl = SystemConfigurations.WebBaseUrl + GetUrl(t.TransactionCategoryId, userProfileDTO.IsVipUser, t.Id),




                }).ToList();

            }
            return null;
        }



        private static string GetUrl(int transactionCategory, bool isVipUser, int id)
        {

            string transactionId = StringCipher.EncryptionStringAES(id.ToString());
            string urlActionPage = "";
            switch (transactionCategory)
            {
                case (int)TransactionCategory.Inbound:


                    if (isVipUser)
                    {
                        urlActionPage = "User/VipInbound/Edit?id=" + transactionId;
                    }
                    else
                    {
                        urlActionPage = "User/Inbound/Edit?id=" + transactionId;
                    }


                    break;
                case (int)TransactionCategory.ExternalOutbound:

                    if (isVipUser)
                    {
                        urlActionPage = "User/OutboundExternal/VIPEdit?id=" + transactionId;
                    }
                    else
                    {
                        urlActionPage = "User/OutboundExternal/Edit?id=" + transactionId;
                    }
                    break;
                case (int)TransactionCategory.InternalOutbound:

                    if (isVipUser)
                    {
                        urlActionPage = "User/VipOutboundInternal/Edit?id=" + transactionId;
                    }
                    else
                    {
                        urlActionPage = "User/OutboundInternal/Edit?id=" + transactionId;


                    }
                    break;
                case (int)TransactionCategory.DraftOutbound:

                    if (isVipUser)
                    {
                        urlActionPage = "User/VipOutboundDraft/Edit?id=" + transactionId;
                    }
                    else
                    {
                        urlActionPage = "User/OutboundExternal/Edit?id=" + transactionId + "&IsFromDraft=true";
                    }
                    break;
            }
            return urlActionPage;
        }
    }
}