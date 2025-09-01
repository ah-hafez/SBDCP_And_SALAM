using System.Collections.Generic;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.Lookups;
using AdminLookupVM = MCS.UI.Areas.Admin.Models.Lookups;
using AdminLookupMapper = MCS.UI.Areas.Admin.Mappers.LookupMapper;
using MCS.UI.Areas.User.Mappers.Transaction;
using System.Linq;

namespace MCS.UI
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
                Result = LookupMapper.Map(lookups.Result),
                RowsCount = lookups.RowsCount
            };
        }
        public static GetResult<IList<LookupVM>> GetLookupItemswithoutCached(LookupCategory lookupCategory, string cultureName)
        {
            var urlLookups = string.Format("api/Lookups/GetLookupItemsWithoutCache?lookupCategory={0}&cultureName={1}", lookupCategory, SessionInfo.CultureShortName);

            GetResult<IList<LookupDTO>> lookups = HttpClientWrapper<GetResult<IList<LookupDTO>>>.GetItemRequest(urlLookups).Result;


            if (lookupCategory == LookupCategory.SearchType)
            {
                lookups.Result = CheckSearchClaims(lookups.Result, cultureName);
            }

            return new GetResult<IList<LookupVM>>
            {
                StatusCode = lookups.StatusCode,
                Result = LookupMapper.Map(lookups.Result),
                RowsCount = lookups.RowsCount
            };
        }
        public static GetResult<IList<LookupVM>> GetActiveLookupItemswithoutCached(LookupCategory lookupCategory, string cultureName)
        {
            var urlLookups = string.Format("api/Lookups/GetActiveLookupItemsWithoutCache?lookupCategory={0}&cultureName={1}", lookupCategory, SessionInfo.CultureShortName);

            GetResult<IList<LookupDTO>> lookups = HttpClientWrapper<GetResult<IList<LookupDTO>>>.GetItemRequest(urlLookups).Result;


            if (lookupCategory == LookupCategory.SearchType)
            {
                lookups.Result = CheckSearchClaims(lookups.Result, cultureName);
            }

            return new GetResult<IList<LookupVM>>
            {
                StatusCode = lookups.StatusCode,
                Result = LookupMapper.Map(lookups.Result),
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
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByNames))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByNames).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchDaily))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchDaily).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByAssignmentNote))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByAssignmentNote).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByManifestNumber))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByManifestNumber).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByMilitaryNumberOrIdentity))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByMilitaryNumberOrIdentity).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByTransactionNots))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByTransactionNots).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByELcEmployee))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByELcEmployee).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByExternalOutBoundOrManifestNumber))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByExternalOutBoundOrManifestNumber).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByCopyAssignemnt))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByCopyAssignemnt).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchBySubjectLetter))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchBySubjectLetter).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByTransactionNumber))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByTransactionNumber).SingleOrDefault());
            }
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchByExternalPartyCopies))
            {
                newList.Add(lookupList.Where(a => (SearchType)a.EnumReference == SearchType.SearchByExternalPartyCopies).SingleOrDefault());
            }
            return newList;
        }
        public static GetResult<IList<AdminLookupVM.LookupVM>> GetAdminLookupItems(LookupCategory lookupCategory, string cultureName)
        {
            var urlLookups = string.Format("api/Lookups/GetLookupItems?lookupCategory={0}&cultureName={1}", lookupCategory, SessionInfo.CultureShortName);

            GetResult<IList<LookupDTO>> lookups = HttpClientWrapper<GetResult<IList<LookupDTO>>>.GetItemRequest(urlLookups).Result;

            return new GetResult<IList<AdminLookupVM.LookupVM>>
            {
                StatusCode = lookups.StatusCode,
                Result = AdminLookupMapper.Map(lookups.Result),
                RowsCount = lookups.RowsCount
            };
        }
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
        public static GetResult<List<TransactionTypeVM>> GetTransactionTypes(TransactionCategory transactionCategory)
        {
            GetResult<List<TransactionTypeDTO>> TransactionTypeDTOs = CacheHelper.Get(CachedObjectsKey.TransactionTypes + "_" + transactionCategory.ToString(), SessionInfo.CultureShortName) as GetResult<List<TransactionTypeDTO>>;

            if (TransactionTypeDTOs == null)
            {
                TransactionTypeDTOs = HttpClientWrapper<GetResult<List<TransactionTypeDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
                CacheHelper.Insert(CachedObjectsKey.TransactionTypes, TransactionTypeDTOs, SessionInfo.CultureShortName);
            }

            return new GetResult<List<TransactionTypeVM>>
            {
                Result = TransactionTypeMapper.Map(TransactionTypeDTOs.Result),
                RowsCount = TransactionTypeDTOs.RowsCount,
                StatusCode = TransactionTypeDTOs.StatusCode
            };
        }
        public static GetResult<List<PriorityVM>> GetPriorities(TransactionCategory transactionCategory)
        {
            GetResult<List<PriorityDTO>> PriorityVMs = CacheHelper.Get(CachedObjectsKey.Priorities, SessionInfo.CultureShortName) as GetResult<List<PriorityDTO>>;

            if (PriorityVMs == null)
            {
                GetResult<List<PriorityDTO>> getResult = HttpClientWrapper<GetResult<List<PriorityDTO>>>.GetItemRequest(string.Format("api/Transaction/GetPriorities?cultureName={0}&transactionCategory={1}&OrgUnitId={2}&UserId={3}", SessionInfo.CultureShortName, transactionCategory, SessionInfo.OrgUnitInfo.Id, SessionInfo.CurrentUser.Id)).Result;
                PriorityVMs = getResult;
                CacheHelper.Insert(CachedObjectsKey.Priorities, PriorityVMs, SessionInfo.CultureShortName);
            }
            return new GetResult<List<PriorityVM>>
            {
                Result = PriorityMapper.Map(PriorityVMs.Result),
                RowsCount = PriorityVMs.RowsCount,
                StatusCode = PriorityVMs.StatusCode
            };
        }
        public static GetResult<List<LetterTypeVM>> GetLetterTypes(TransactionCategory transactionCategory)
        {
            GetResult<List<LetterTypeDTO>> LetterTypeDTOs = CacheHelper.Get(CachedObjectsKey.LetterTypes, SessionInfo.CultureShortName) as GetResult<List<LetterTypeDTO>>;

            if (LetterTypeDTOs == null)
            {
                GetResult<List<LetterTypeDTO>> getResult = HttpClientWrapper<GetResult<List<LetterTypeDTO>>>.GetItemRequest(string.Format("api/Transaction/GetLetterTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
                LetterTypeDTOs = getResult;
                CacheHelper.Insert(CachedObjectsKey.LetterTypes, LetterTypeDTOs, SessionInfo.CultureShortName);
            }

            return new GetResult<List<LetterTypeVM>>
            {
                Result = LetterTypeMapper.Map(LetterTypeDTOs.Result),
                RowsCount = LetterTypeDTOs.RowsCount,
                StatusCode = LetterTypeDTOs.StatusCode
            };
        }

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

        public static GetResult<List<SpecificLevelVM>> GetSpecificLevels(TransactionCategory transactionCategory)
        {
            GetResult<List<SpecificLevelDTO>> specificLevelDTOs = CacheHelper.Get(CachedObjectsKey.SpecificLevels, SessionInfo.CultureShortName) as GetResult<List<SpecificLevelDTO>>;

            if (specificLevelDTOs == null || specificLevelDTOs.RowsCount == 0)
            {
                GetResult<List<SpecificLevelDTO>> getResult = HttpClientWrapper<GetResult<List<SpecificLevelDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSpecificLevels?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
                specificLevelDTOs = getResult;
                CacheHelper.Insert(CachedObjectsKey.SpecificLevels, specificLevelDTOs, SessionInfo.CultureShortName);
            }

            return new GetResult<List<SpecificLevelVM>>
            {
                Result = SpecificLevelMapper.Map(specificLevelDTOs.Result),
                RowsCount = specificLevelDTOs.RowsCount,
                StatusCode = specificLevelDTOs.StatusCode
            };
        }

        public static GetResult<List<ClassificationDto>> GetClassificationTypes()
        {
            GetResult<List<ClassificationDto>> classificationTypes = CacheHelper.Get(CachedObjectsKey.ClassificationTypes, SessionInfo.CultureShortName) as GetResult<List<ClassificationDto>>;

            if (classificationTypes == null)
            {
                GetResult<List<ClassificationDto>> getResult = HttpClientWrapper<GetResult<List<ClassificationDto>>>.GetItemRequest(string.Format("api/IC/GetClassificationTypes")).Result;
                classificationTypes = getResult;
                CacheHelper.Insert(CachedObjectsKey.LetterTypes, classificationTypes, SessionInfo.CultureShortName);
            }

            return new GetResult<List<ClassificationDto>>
            {
                Result = classificationTypes.Result,
                RowsCount = classificationTypes.RowsCount,
                StatusCode = classificationTypes.StatusCode
            };
        }
    }
}
