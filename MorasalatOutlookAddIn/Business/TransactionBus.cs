using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;

namespace MorasalatOutlookAddIn.Business
{
    public class TransactionBus
    {
        internal static List<ExternalPartyDTO> GetUnits(TransactionCategorieColor transType,int? parentId, string searchQuery)
        {
            switch (transType)
            {
                case TransactionCategorieColor.None:
                    return null;
                case TransactionCategorieColor.Inbound:
                    return ExternalUnits(parentId, searchQuery);
                case TransactionCategorieColor.Outbound:
                    return null;
                case TransactionCategorieColor.InternalOutbound:
                    return InternalUnits(parentId, searchQuery);
                case TransactionCategorieColor.DraftOutbound:
                    return null;
                default:
                    return null;
            }
        }
        private static List<MCS.DTO.ExternalPartyDTO> InternalUnits(int? parentId, string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
                return InternalUnitsAutoComplete(searchQuery);

            var servicePath = string.Format("api/OutlookIntegration/InternalUnits?cultureName={0}&parentId={1}", Helper.GetCultureName, parentId);
            parentId = parentId == -1 ? null : parentId;
            Forms.Loading loadingObj = new Forms.Loading();
            loadingObj.Show();
            var serviceCall = HttpClientWrapper<PostObjectResult<List<OrgUnitDTO>>>.PostRequest(servicePath,null).Result;
            loadingObj.Close();

            List<MCS.DTO.ExternalPartyDTO> list = new List<MCS.DTO.ExternalPartyDTO>();
          
            if (serviceCall.StatusCode != StatusCode.Ok)
            {
                return list;
            }

           
            foreach (var item in serviceCall.Result)
            {
                list.Add(new MCS.DTO.ExternalPartyDTO { Id = item.Id, LocalName = item.Name, ParentId = item.ParentId });
            }

            return list;
        }
        private static List<MCS.DTO.ExternalPartyDTO> InternalUnitsAutoComplete(string searchQuery)
        {
            Forms.Loading loadingObj = new Forms.Loading();
            loadingObj.Show();
            var servicePath = string.Format("api/OutlookIntegration/InternalUnitsAutoComplete?cultureName={0}&searchQuery={1}&pageSize={2}",
                    Helper.GetCultureName,
                    searchQuery,
                    Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AutoCompleteResultSize"].ToString())
                    );

            var serviceCall = HttpClientWrapper<PostObjectResult<List<OrgUnitDTO>>>.PostRequest(servicePath,null).Result;
            loadingObj.Close();
            List<MCS.DTO.ExternalPartyDTO> list = new List<MCS.DTO.ExternalPartyDTO>();
            foreach (var item in serviceCall.Result)
            {
                list.Add(new MCS.DTO.ExternalPartyDTO { Id = item.Id, LocalName = item.Name, ParentId = item.ParentId });
            }

            return list;
        }
        private static List<MCS.DTO.ExternalPartyDTO> ExternalUnits(int? parentId,string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
                return ExternalUnitsAutoComplete(searchQuery);

           
            parentId = parentId == -1 ? null : parentId;
            Forms.Loading loadingObj = new Forms.Loading();
            loadingObj.Show();
            var serviceCall = HttpClientWrapper<PostObjectResult<List<ExternalPartyDTO>>>.PostRequest(
                    string.Format("api/OutlookIntegration/ExternalUnits?cultureName={0}&parentId={1}", Helper.GetCultureName, parentId),null).Result;
            loadingObj.Close();

            if (serviceCall.StatusCode != StatusCode.Ok)
            {   
                return new List<MCS.DTO.ExternalPartyDTO>();
            }

           

            return serviceCall.Result;

        }
        private static List<MCS.DTO.ExternalPartyDTO> ExternalUnitsAutoComplete(string searchQuery)
        {
            var pageSize = System.Configuration.ConfigurationManager.AppSettings["AutoCompleteResultSize"].ToString();
            var servicePath = string.Format("api/OutlookIntegration/ExternalUnitsAutoComplete?cultureName={0}&searchQuery={1}&pageSize={2}",
                Helper.GetCultureName,searchQuery, pageSize);

            Forms.Loading loadingObj = new Forms.Loading();
            loadingObj.Show();
            var serviceCall = HttpClientWrapper<PostObjectResult<List<ExternalPartyDTO>>>.PostRequest(servicePath,null).Result;
            loadingObj.Close();
            
            if (serviceCall.StatusCode != StatusCode.Ok)
            {
                return new List<MCS.DTO.ExternalPartyDTO>();
            }
            return serviceCall.Result;
        }


        internal static string CreateInbound(AddInboundDTO inboundObj,ref string transactionNo,ref string tranactionId)
        {
            if (inboundObj.InboundBasicInfo.Remarks.Length > 1000)
                return "الملاحظات يجب أن لا تزيد عن 1000 حرف";

            string servicePath = string.Format("api/OutlookIntegration/CreateInbound?cultureName={0}&orgUnitId={1}&access={2}&userId={3}",
                Helper.GetCultureName , Helper.UserOrgUnitId,Helper.AccessToken,Helper.UserId);

            Forms.Loading loadingObj = new Forms.Loading();
            loadingObj.Show();
            var postResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>.PostRequest(servicePath, inboundObj).Result;

            loadingObj.Close();
            if (postResult.StatusCode != StatusCode.Ok)
            {
                return string.Format("المعاملة لم يتم حفظها {0}.",postResult.StatusCode);
            }

            MCS.DTO.TransactionDetailsDTO details = postResult.Result as MCS.DTO.TransactionDetailsDTO;
            transactionNo = details.Number.ToString();
            tranactionId = details.Id.ToString();
            return string.Empty;
        }
        internal static string CreateOutboundInternal(AddOutboundInternalDTO tranObj, ref string transactionNo,ref string transactionId)
        {
            if (tranObj.OutboundInternalBasicInfoAdd.Remarks.Length > 1000)
                return "الملاحظات يجب أن لا تزيد عن 1000 حرف";

            var servicePath = string.Format("api/OutlookIntegration/CreateOutboundInternal?cultureName={0}&orgUnitId={1}&access={2}&userId={3}", 
                Helper.GetCultureName,Helper.UserOrgUnitId,Helper.AccessToken,Helper.UserId);
            Forms.Loading loadingObj = new Forms.Loading();
            loadingObj.Show();
            var postResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>.PostRequest(servicePath, tranObj).Result;
            loadingObj.Close();
            if (postResult.StatusCode != StatusCode.Ok)
            {
                return string.Format("المعاملة لم يتم حفظها {0}.", postResult.StatusCode);
            }

            var details = postResult.Result as MCS.DTO.TransactionDetailsDTO;
            transactionNo = details.Number.ToString();
            transactionId = details.Id.ToString();
            return string.Empty;
        }

        internal static string AssignmentCreate(TransactionAssignmentDTO dtoObj,string transactionId)
        {
            Forms.Loading loadingObj = new Forms.Loading();
            loadingObj.Show();
            var servicePath = string.Format("api/OutlookIntegration/AssignmentCreate?cultureName={0}&transactionIds={1}&access={2}&userId={3}", 
                Helper.GetCultureName, transactionId,Helper.AccessToken,Helper.UserId);
            var list = new List<TransactionAssignmentDTO>();
            list.Add(dtoObj);            
            var postResult = HttpClientWrapper<PostResult>.PostRequest(servicePath, list).Result;
            loadingObj.Close();
            if (postResult.StatusCode != StatusCode.Ok)
            {
                return string.Format("الإحاله لم يتم حفظها {0}.", postResult.StatusCode);
            }

            return string.Empty;
        }

        
    }
}
