using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public static class SearchBL
    {
        public static IList<CreatorSearchResult> SearchCreator(SearchCriteriaByCreator searchCriteriaByCreator)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<CreatorSearchResult> entitySearchResults = searchWrapper.CreatorSearch(searchCriteriaByCreator, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (CreatorSearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;


            }

            return entitySearchResults;
        }
        public static IList<AssignTransactionSearchResult> SearchAssignTransaction(SearchCriteriaByAssignTransaction searchCriteriaByAssignTransaction)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<AssignTransactionSearchResult> entitySearchResults = searchWrapper.AssignTransactionSearch(searchCriteriaByAssignTransaction, out TotalCount);
            ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();

            foreach (var item in entitySearchResults)
            {
                item.TransactionAssignment = transactionAssignmentBL.GetTransactionAssignments(item.Id, "ar").FirstOrDefault();
            }

            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (AssignTransactionSearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }

        public static IList<EntitySearchResult> SearchEntity(SearchCriteriaByEntityName searchCriteriaByEntity)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<EntitySearchResult> entitySearchResults = searchWrapper.EntitySearch(searchCriteriaByEntity, out TotalCount);
            ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();

            foreach (var item in entitySearchResults)
            {
                item.TransactionAssignment = transactionAssignmentBL.GetTransactionAssignments(item.Id, "ar").FirstOrDefault();
            }

            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (EntitySearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }
        public static IList<InboundSearchResult> SearchInbound(SearchCriteriaByInbound searchCriteriaByInbound)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<InboundSearchResult> inboundSearchResults = searchWrapper.InboundSearch(searchCriteriaByInbound, out TotalCount);

            ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();

            foreach (var item in inboundSearchResults)
            {
                item.TransactionAssignment = transactionAssignmentBL.GetTransactionAssignments(item.Id, "ar").FirstOrDefault();
            }

            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (InboundSearchResult inboundSearchResult in inboundSearchResults)
            {
                inboundSearchResult.TotalCount = TotalCount;
                if (inboundSearchResult.Weight <= userWeigth)
                {
                    inboundSearchResult.HasPermission = true;
                }
                inboundSearchResult.HasPermission = inboundSearchResult.HasPermission || inboundSearchResult.IsView;
            }

            return inboundSearchResults;
        }

        public static IList<InboundSearchResult> SearchDocumentNumber(SearchCriteriaByDocumentNumber searchCriteriaByDocumentNumber)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<InboundSearchResult> inboundSearchResults = searchWrapper.SearchDocumentNumber(searchCriteriaByDocumentNumber, out TotalCount);
            //ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();

            //foreach (var item in inboundSearchResults)
            //{
            //    item.TransactionAssignment = transactionAssignmentBL.GetTransactionAssignments(item.Id, "ar").FirstOrDefault();
            //}
            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (InboundSearchResult inboundSearchResult in inboundSearchResults)
            {
                inboundSearchResult.TotalCount = TotalCount;
                if (inboundSearchResult.Weight <= userWeigth)
                {
                    inboundSearchResult.HasPermission = true;
                }
                inboundSearchResult.HasPermission = inboundSearchResult.HasPermission || inboundSearchResult.IsView;
            }

            return inboundSearchResults;
        }


        public static IList<InboundSearchResult> SearchRecordNumber(SearchCriteriaByRecordNumber searchCriteriaByRecordNumber)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<InboundSearchResult> inboundSearchResults = searchWrapper.SearchRecordNumber(searchCriteriaByRecordNumber, out TotalCount);

            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (InboundSearchResult inboundSearchResult in inboundSearchResults)
            {
                inboundSearchResult.TotalCount = TotalCount;
                if (inboundSearchResult.Weight <= userWeigth)
                {
                    inboundSearchResult.HasPermission = true;
                }
                inboundSearchResult.HasPermission = inboundSearchResult.HasPermission || inboundSearchResult.IsView;
            }

            return inboundSearchResults;
        }

        public static IList<OutboundInternalSearchResult> SearchOutboundInternal(SearchCriteriaByOutboundInternal searchCriteriaByOutboundInternal)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<OutboundInternalSearchResult> OutboundInternalSearchResults = searchWrapper.OutboundInternalSearch(searchCriteriaByOutboundInternal, out TotalCount);
            ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();

            foreach (var item in OutboundInternalSearchResults)
            {
                item.TransactionAssignment = transactionAssignmentBL.GetTransactionAssignments(item.Id, "ar").FirstOrDefault();
            }
            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (OutboundInternalSearchResult OutboundInternalSearchResult in OutboundInternalSearchResults)
            {
                OutboundInternalSearchResult.TotalCount = TotalCount;
                if (!OutboundInternalSearchResult.Weight.HasValue || OutboundInternalSearchResult.Weight <= userWeigth)
                {
                    OutboundInternalSearchResult.HasPermission = true;
                }
                OutboundInternalSearchResult.HasPermission = OutboundInternalSearchResult.HasPermission || OutboundInternalSearchResult.IsView;
            }

            return OutboundInternalSearchResults;
        }
        public static IList<OutboundSearchResult> SearchOutbound(SearchCriteriaByOutbound searchCriteriaByOutbound)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<OutboundSearchResult> OutboundSearchResults = searchWrapper.OutboundSearch(searchCriteriaByOutbound, out TotalCount);
            ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();

            foreach (var item in OutboundSearchResults)
            {
                item.TransactionAssignment = transactionAssignmentBL.GetTransactionAssignments(item.Id, "ar").FirstOrDefault();
            }
            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (OutboundSearchResult OutboundSearchResult in OutboundSearchResults)
            {
                OutboundSearchResult.TotalCount = TotalCount;

                if (OutboundSearchResult.Weight <= userWeigth)
                {
                    OutboundSearchResult.HasPermission = true;
                }
                OutboundSearchResult.HasPermission = OutboundSearchResult.HasPermission || OutboundSearchResult.IsView;
            }

            return OutboundSearchResults;
        }
        public static IList<OutboundDraftSearchResult> SearchOutboundDraft(SearchCriteriaByOutboundDraft searchCriteriaByOutbound)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<OutboundDraftSearchResult> OutboundDraftSearchResults = searchWrapper.OutboundDraftSearch(searchCriteriaByOutbound, out TotalCount);
            ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();

            foreach (var item in OutboundDraftSearchResults)
            {
                item.TransactionAssignment = transactionAssignmentBL.GetTransactionAssignments(item.Id, "ar").FirstOrDefault();
            }
            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (OutboundDraftSearchResult OutboundDraftSearchResult in OutboundDraftSearchResults)
            {
                OutboundDraftSearchResult.TotalCount = TotalCount;

                if (OutboundDraftSearchResult.Weight <= userWeigth)
                {
                    OutboundDraftSearchResult.HasPermission = true;
                }
                OutboundDraftSearchResult.HasPermission = OutboundDraftSearchResult.HasPermission || OutboundDraftSearchResult.IsView;
            }

            return OutboundDraftSearchResults;
        }
        public static IList<SubjectSearchResult> SearchSubject(SearchCriteriaBySubject searchCriteriaBySubject)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            IList<SubjectSearchResult> subjectSearchResults = searchWrapper.SubjectSearch(searchCriteriaBySubject, out int TotalCount);
            ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();

            foreach (var item in subjectSearchResults)
            {
                item.TransactionAssignment = transactionAssignmentBL.GetTransactionAssignments(item.Id, "ar").FirstOrDefault();
            }
            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (SubjectSearchResult subjectSearchResult in subjectSearchResults)
            {
                subjectSearchResult.TotalCount = TotalCount;

                if (subjectSearchResult.Weight <= userWeigth)
                {
                    subjectSearchResult.HasPermission = true;
                }
                subjectSearchResult.HasPermission = subjectSearchResult.HasPermission || subjectSearchResult.IsView;

            }

            return subjectSearchResults;
        }
        public static IList<BaseSearchResult> SearchBarcode(SearchCriteriaByBarcode searchCriteriaByBarcode)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();
            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

            IList<BaseSearchResult> baseSearchResults = searchWrapper.BarcodeSearch(searchCriteriaByBarcode, out int TotalCount);
            ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();

            foreach (var item in baseSearchResults)
            {
                item.TransactionAssignment = transactionAssignmentBL.GetTransactionAssignments(item.Id, "ar").FirstOrDefault();
            }
            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (BaseSearchResult baseSearchResult in baseSearchResults)
            {
                baseSearchResult.TotalCount = TotalCount;

                if (baseSearchResult.Weight <= userWeigth)
                {
                    baseSearchResult.HasPermission = true;
                }
                baseSearchResult.HasPermission = baseSearchResult.HasPermission || baseSearchResult.IsView;
            }

            return baseSearchResults;
        }

        public static IList<NamesSearchResult> SearchNames(SearchCriteriaByNames searchCriteriaByNames)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<NamesSearchResult> entitySearchResults = searchWrapper.SearchNames(searchCriteriaByNames, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (NamesSearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }
        public static IList<DailySearchResult> SearchDaily(SearchCriteriaByDaily searchCriteriaByDaily)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<DailySearchResult> entitySearchResults = searchWrapper.SearchDaily(searchCriteriaByDaily, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (DailySearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }

        public static IList<AssignmentNoteSearchResult> SearchAssignmentNote(SearchCriteriaByAssignmentNote searchCriteriaByAssignmentNote)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<AssignmentNoteSearchResult> entitySearchResults = searchWrapper.SearchAssignmentNote(searchCriteriaByAssignmentNote, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (AssignmentNoteSearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }
        public static IList<ManifestNumberSearchResult> SearchManifestNumber(SearchCriteriaByManifestNumber searchCriteriaByManifestNumber)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<ManifestNumberSearchResult> entitySearchResults = searchWrapper.SearchManifestNumber(searchCriteriaByManifestNumber, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (ManifestNumberSearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }
        public static IList<ExternalPartyCopiesSearchResult> SearchExternalPartyCopies(SearchCriteriaByExternalPartyCopies searchCriteriaByExternalPartyCopies)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<ExternalPartyCopiesSearchResult> copiesSearchResults = searchWrapper.SearchExternalPartyCopies(searchCriteriaByExternalPartyCopies, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (ExternalPartyCopiesSearchResult entirtSearchResult in copiesSearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return copiesSearchResults;
        }

        public static IList<MilitaryNumberOrIdentitySearchResult> SearchMilitaryNumberOrIdentity(SearchCriteriaByMilitaryNumberOrIdentity searchCriteriaByMilitaryNumberOrIdentity)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<MilitaryNumberOrIdentitySearchResult> entitySearchResults = searchWrapper.SearchMilitaryNumberOrIdentity(searchCriteriaByMilitaryNumberOrIdentity, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (MilitaryNumberOrIdentitySearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }
        public static IList<TransactionNumberSearchResult> SearchTransactionNumber(SearchCriteriaByTransactionNumber searchCriteriaByTransactionNumber)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<TransactionNumberSearchResult> entitySearchResults = searchWrapper.SearchTransactionNumber(searchCriteriaByTransactionNumber, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (TransactionNumberSearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }
        public static IList<SubjectLetterSearchResult> SearchSubjectLetter(SearchCriteriaBySubjectLetter searchCriteriaBySubjectLetter)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<SubjectLetterSearchResult> entitySearchResults = searchWrapper.SearchSubjectLetter(searchCriteriaBySubjectLetter, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (SubjectLetterSearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }

        public static IList<CopyAssignemntSearchResult> SearchCopyAssignemnt(SearchCriteriaByCopyAssignemnt searchCriteriaByCopyAssignemnt)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<CopyAssignemntSearchResult> entitySearchResults = searchWrapper.SearchCopyAssignemnt(searchCriteriaByCopyAssignemnt, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (CopyAssignemntSearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }
        public static IList<ElcEmployeeSearchResult> SearchElcEmployee(SearchCriteriaByElcEmployee searchCriteriaByElcEmployee)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<ElcEmployeeSearchResult> entitySearchResults = searchWrapper.SearchELcEmployee(searchCriteriaByElcEmployee, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (ElcEmployeeSearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }
        public static IList<ExternalOutBoundOrManifestNumberSearchResult> SearchExternalOutBoundOrManifestNumber(SearchCriteriaByExternalOutBoundOrManifestNumber searchCriteriaByExternalOutBoundOrManifestNumber)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<ExternalOutBoundOrManifestNumberSearchResult> entitySearchResults = searchWrapper.SearchExternalOutBoundOrManifestNumber(searchCriteriaByExternalOutBoundOrManifestNumber, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (ExternalOutBoundOrManifestNumberSearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }


        public static IList<TransactionNotsSearchResult> SearchTransactionNots(SearchCriteriaByTransactionNots searchCriteriaByTransactionNots)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int TotalCount;
            IList<TransactionNotsSearchResult> entitySearchResults = searchWrapper.SearchTransactionNots(searchCriteriaByTransactionNots, out TotalCount);


            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (TransactionNotsSearchResult entirtSearchResult in entitySearchResults)
            {
                entirtSearchResult.TotalCount = TotalCount;

                if (entirtSearchResult.Weight <= userWeigth)
                {
                    entirtSearchResult.HasPermission = true;
                }
                entirtSearchResult.HasPermission = entirtSearchResult.HasPermission || entirtSearchResult.IsView;
            }

            return entitySearchResults;
        }


        public static IList<ICSearchResult> ICSearch(int year, string transNumber, int orgId, int type, int userId, string culutre)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

            IList<ICSearchResult> socialNumberSearchResults = searchWrapper.ICSearch(year, transNumber, orgId, type, userId, culutre);

            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (ICSearchResult socialNumberSearchResult in socialNumberSearchResults)
            {

                if (socialNumberSearchResult.Weight <= userWeigth)
                {
                    socialNumberSearchResult.HasPermission = true;
                }
                socialNumberSearchResult.HasPermission = socialNumberSearchResult.HasPermission || socialNumberSearchResult.IsView;
            }

            return socialNumberSearchResults;
        }

        public static IList<ICSearchResult> ICSearchByTransactionID(int transactionID, int userId, string culutre)
        {
            ISearchWrapper searchWrapper = IoC.Resolve<ISearchWrapper>();

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

            IList<ICSearchResult> socialNumberSearchResults = searchWrapper.ICSearchByTransactionID(transactionID, userId, culutre);

            int? userWeigth = permissions.Max(s => s.Weight);

            foreach (ICSearchResult socialNumberSearchResult in socialNumberSearchResults)
            {

                if (socialNumberSearchResult.Weight <= userWeigth)
                {
                    socialNumberSearchResult.HasPermission = true;
                }
            }

            return socialNumberSearchResults;
        }
    }
}
