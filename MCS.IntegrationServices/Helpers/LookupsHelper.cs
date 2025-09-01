using MCS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.ApiControllerResults;
using MCS.IntegrationServices.Models;
using MCS.DTO;
using MCS.IntegrationServices.Mappers;

namespace MCS.IntegrationServices.Helpers
{
    public class LookupsHelper : ILookupHelper
    {
        public static GetResult<IList<LookupVM>> GetLookupItems(LookupCategory lookupCategory, string cultureName)
        {
            var urlLookups = string.Format("api/Lookups/GetLookupItems?lookupCategory={0}&cultureName={1}", lookupCategory, SessionInfo.CultureShortName);

            GetResult<IList<LookupDTO>> lookups = HttpClientWrapper<GetResult<IList<LookupDTO>>>.GetItemRequest(urlLookups).Result;


            if (lookupCategory == LookupCategory.SearchType)
            {
                lookups.Result = CheckSearchClaims(lookups.Result, cultureName);
            }

            return new GetResult<IList<LookupVM>>
            {
                StatusCode = lookups.StatusCode,
                // Result = LookupMapper.Map(lookups.Result),
                RowsCount = lookups.RowsCount
            };
        }

        public static IList<LookupDTO> CheckSearchClaims(IList<LookupDTO> lookupList, string cultureName)
        {
            IList<LookupDTO> newList = new List<LookupDTO>();
            //if (User.HasClaim(UserClaims.Search.SearchbyTransactionNumberInboundOutbound))
            //{
            //    newList.Add(lookup.Where(a => a.Id ==SearchType.SearchbyTransactionNumberInboundOutbound.LookupIdentity(LookupCategory.SearchType, cultureName)).SingleOrDefault());
            //}
            //if (User.HasClaim(UserClaims.Search.SearchbyEncryptionCode))
            //{
            //    newList.Add(lookup.Where(a => a.Id == SearchType.SearchbyEncryptionCode.LookupIdentity(LookupCategory.SearchType, cultureName)).SingleOrDefault());
            //}
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchbySubject))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchBySubject).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByAssignTransaction))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByAssignTransaction).SingleOrDefault());
            }
            //if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByRecordNumber))
            //{
            //    newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByRecordNumber).SingleOrDefault());
            //}
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByInboundNumber))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByInboundNumber).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByOutboundNumber))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByOutboundNumber).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByOutboundInternalNumber))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByOutboundInternalNumber).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByOutboundDraftNumber))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByOutboundDraftNumber).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByEntity))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByEntity).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByCreator))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByCreator).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByDocumentNumber))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByDocumentNumber).SingleOrDefault());
            }
            return newList;
        }

        //public static GetResult<IList<AdminLookupVM.LookupVM>> GetAdminLookupItems(LookupCategory lookupCategory, string cultureName)
        //{
        //    var urlLookups = string.Format("api/Lookups/GetLookupItems?lookupCategory={0}&cultureName={1}", lookupCategory, SessionInfo.CultureShortName);

        //    GetResult<IList<LookupDTO>> lookups = HttpClientWrapper<GetResult<IList<LookupDTO>>>.GetItemRequest(urlLookups).Result;

        //    return new GetResult<IList<AdminLookupVM.LookupVM>>
        //    {
        //        StatusCode = lookups.StatusCode,
        //        Result = AdminLookupMapper.Map(lookups.Result),
        //        RowsCount = lookups.RowsCount
        //    };
        //}
        public static GetResult<LookupVM> GetLookupItem(int lookupId, string cultureName)
        {
            var urlLookups = string.Format("api/Lookups/GetLookupItem?lookupId={0}&cultureName={1}", lookupId, SessionInfo.CultureShortName);

            GetResult<LookupDTO> lookup = HttpClientWrapper<GetResult<LookupDTO>>.GetItemRequest(urlLookups).Result;
            return new GetResult<LookupVM>
            {
                StatusCode = lookup.StatusCode,
                RowsCount = lookup.RowsCount,
                Result = LookupMapper.Map(lookup.Result)
            };
        }
        //public static GetResult<List<TransactionTypeVM>> GetTransactionTypes(TransactionCategory transactionCategory)
        //{
        //    GetResult<List<TransactionTypeDTO>> TransactionTypeDTOs = CacheHelper.Get(CachedObjectsKey.TransactionTypes + "_" + transactionCategory.ToString(), SessionInfo.CultureShortName) as GetResult<List<TransactionTypeDTO>>;

        //    if (TransactionTypeDTOs == null)
        //    {
        //        TransactionTypeDTOs = HttpClientWrapper<GetResult<List<TransactionTypeDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
        //        CacheHelper.Insert(CachedObjectsKey.TransactionTypes, TransactionTypeDTOs, SessionInfo.CultureShortName);
        //    }

        //    return new GetResult<List<TransactionTypeVM>>
        //    {
        //        Result = TransactionTypeMapper.Map(TransactionTypeDTOs.Result),
        //        RowsCount = TransactionTypeDTOs.RowsCount,
        //        StatusCode = TransactionTypeDTOs.StatusCode
        //    };
        //}
        //public static GetResult<List<PriorityVM>> GetPriorities(TransactionCategory transactionCategory)
        //{
        //    GetResult<List<PriorityDTO>> PriorityVMs;
        //    //= CacheHelper.Get(CachedObjectsKey.Priorities, SessionInfo.CultureShortName) as GetResult<List<PriorityDTO>>;

        //    //// if (PriorityVMs == null)
        //    //{
        //    GetResult<List<PriorityDTO>> getResult = HttpClientWrapper<GetResult<List<PriorityDTO>>>.GetItemRequest(string.Format("api/Transaction/GetPriorities?cultureName={0}&transactionCategory={1}&OrgUnitId={2}&UserId={3}", SessionInfo.CultureShortName, transactionCategory, SessionInfo.OrgUnitInfo.Id, SessionInfo.CurrentUser.Id)).Result;
        //    PriorityVMs = getResult;
        //    CacheHelper.Insert(CachedObjectsKey.Priorities, PriorityVMs, SessionInfo.CultureShortName);
        //    // }
        //    return new GetResult<List<PriorityVM>>
        //    {
        //        Result = PriorityMapper.Map(PriorityVMs.Result),
        //        RowsCount = PriorityVMs.RowsCount,
        //        StatusCode = PriorityVMs.StatusCode
        //    };
        //}
        //public static GetResult<List<LetterTypeVM>> GetLetterTypes(TransactionCategory transactionCategory)
        //{
        //    GetResult<List<LetterTypeDTO>> LetterTypeDTOs = CacheHelper.Get(CachedObjectsKey.LetterTypes, SessionInfo.CultureShortName) as GetResult<List<LetterTypeDTO>>;

        //    if (LetterTypeDTOs == null)
        //    {
        //        GetResult<List<LetterTypeDTO>> getResult = HttpClientWrapper<GetResult<List<LetterTypeDTO>>>.GetItemRequest(string.Format("api/Transaction/GetLetterTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
        //        LetterTypeDTOs = getResult;
        //        CacheHelper.Insert(CachedObjectsKey.LetterTypes, LetterTypeDTOs, SessionInfo.CultureShortName);
        //    }

        //    return new GetResult<List<LetterTypeVM>>
        //    {
        //        Result = LetterTypeMapper.Map(LetterTypeDTOs.Result),
        //        RowsCount = LetterTypeDTOs.RowsCount,
        //        StatusCode = LetterTypeDTOs.StatusCode
        //    };
        //}

        public int GetLookupInternalID(int lookupID, LookupCategory lookupCategory, string cultureName)
        {
            IList<LookupVM> lookups = CacheHelper.Get(CachedObjectsKey.Lookups + lookupCategory.ToString(), cultureName) as IList<LookupVM>;

            if (lookups == null)
            {
                lookups = GetLookupItems(lookupCategory, cultureName).Result;

                CacheHelper.Insert(CachedObjectsKey.Lookups + lookupCategory.ToString(), lookups, cultureName);
            }

            return lookups.Where(x => x.Id == lookupID).Select(x => x.EnumReference.Value).FirstOrDefault();
        }

        public int GetLookupIdentity(int lookupInternalID, LookupCategory lookupCategory, string cultureName)
        {
            IList<LookupVM> lookups = CacheHelper.Get(CachedObjectsKey.Lookups + lookupCategory.ToString(), cultureName) as IList<LookupVM>;

            if (lookups == null)
            {
                lookups = GetLookupItems(lookupCategory, cultureName).Result;

                CacheHelper.Insert(CachedObjectsKey.Lookups + lookupCategory.ToString(), lookups, cultureName);
            }

            return lookups.Where(x => x.EnumReference == lookupInternalID).Select(x => x.Id).FirstOrDefault();
        }
    }
}