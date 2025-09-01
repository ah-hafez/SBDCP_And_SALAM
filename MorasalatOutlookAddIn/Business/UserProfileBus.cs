using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;

namespace MorasalatOutlookAddIn.Business
{
    public class UserProfileBus
    {
        private static List<PriorityDTO> _priorities;

        private static List<PermissionDTO> _ConfidentialityLevel;

        private static List<TransactionTypeDTO> _transactionType;
        internal static void Login()
        {
            if (Helper.AccessToken != null)
                return;

            //Forms.Loading loadingObj = new Forms.Loading();
            //loadingObj.Show();
            //string servicePath = string.Format("api/Login/LoginByEmail?emailAddress={0}&cultureName={1}", Helper.GetEmailAddress, Helper.GetLanguageTag);
            //PostObjectResult<UserDTO> postObj =
            //       HttpClientWrapper<PostObjectResult<UserDTO>>.PostRequest(servicePath, null).Result;

            //if (postObj.Result != null)
            //{
            //    Helper.UserObj = postObj.Result;
            //}
            //loadingObj.Close();


            Forms.Loading loadingObj = new Forms.Loading();
            loadingObj.Show();
            string servicePath = string.Format("api/OutlookIntegration/Login?emailAddress={0}&cultureName={1}", Helper.GetEmailAddress, Helper.GetCultureName);
            PostObjectResult<UserDTO> service =
                   HttpClientWrapper<PostObjectResult<UserDTO>>.PostRequest(servicePath, null).Result;
            if (service.Result != null)
            {
                Helper.UserObj = service.Result;
            }
            loadingObj.Close();
        }

        internal static List<PriorityDTO> GetPriorities()
        {
            if (_priorities == null)
            {
                GetResult<List<PriorityDTO>> priorityDTOs =
               HttpClientWrapper<GetResult<List<PriorityDTO>>>.GetItemRequest(
                   string.Format("api/OutlookIntegration/GetPriorities?cultureName={0}&access={1}&userId={2}",
               Helper.GetCultureName,
               Helper.AccessToken,
                   Helper.UserId)).Result;
                _priorities = priorityDTOs.Result;
            }

            return _priorities;

           
        }

        internal static List<PermissionDTO> GetConfidentialityLevel()
        {
            if (_ConfidentialityLevel == null)
            {
                 var urlPermission = string.Format("api/OutlookIntegration/GetConfidentialityLevel?cultureName={0}&access={1}&userId={2}",
                     Helper.GetCultureName, Helper.AccessToken, Helper.UserId);
                GetResult<List<PermissionDTO>> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;
                _ConfidentialityLevel = permissionDTOs.Result;
            }

            return _ConfidentialityLevel;


        }

        internal static List<TransactionTypeDTO> GetSourceTypes(MCS.Common.TransactionCategory transactionCategory)
        {
         //   if(_transactionType == null)
         //  {
                GetResult<List<TransactionTypeDTO>> service =
               HttpClientWrapper<GetResult<List<TransactionTypeDTO>>>.GetItemRequest(
                   string.Format("api/OutlookIntegration/GetSourceTypes?cultureName={0}&transactionCategory={1}&access={2}&userId={3}",
                   Helper.GetCultureName,
                   transactionCategory,
                   Helper.AccessToken,
                   Helper.UserId)).Result;
                _transactionType = service.Result;
         //   }

            return _transactionType;
        }

        internal static List<UserProfileDTO> GetUsersByOrgUnitId(int orgUnitId)
        {
            var servicePath = string.Format("api/OutlookIntegration/GetUsersByOrgUnitId?cultureName={0}&orgUnitId={1}", Helper.GetCultureName, orgUnitId);
            var userProfileDTOs =  HttpClientWrapper<PostObjectResult<List<UserProfileDTO>>>.PostRequest(servicePath,null).Result;

            List<UserProfileDTO> list = userProfileDTOs.Result;
            if(list == null)
            {
                list = new List<UserProfileDTO>();
            }

            UserProfileDTO dtoObj = new UserProfileDTO();
            dtoObj.Id =-1;
            dtoObj.LocalName = string.Empty;
            list.Insert(0, dtoObj);
            return list;
        }
    }
}
