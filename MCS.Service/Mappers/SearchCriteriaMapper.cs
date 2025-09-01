using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class SearchCriteriaMapper
    {
        public static SearchCriteriaByBarcode Map(SearchCriteriaByBarcodeDTO searchCriteriaDTO)
        {
            if (searchCriteriaDTO == null)
            {
                return new SearchCriteriaByBarcode();
            }

            return new SearchCriteriaByBarcode
            {
                AdvancedSearch = Map(searchCriteriaDTO.AdvancedSearch),
                Ascending = searchCriteriaDTO.Ascending,
                Barcode = searchCriteriaDTO.Barcode,
                CultureName = searchCriteriaDTO.CultureName,
                OrderBy = searchCriteriaDTO.OrderBy,
                PageIndex = searchCriteriaDTO.PageIndex,
                PageSize = searchCriteriaDTO.PageSize,
                HasFullPrivilege = searchCriteriaDTO.HasFullPrivilege,
                Global = searchCriteriaDTO.Global
            };
        }

        public static SearchCriteriaByDocumentNumber Map(SearchCriteriaByDocumentNumberDTO searchCriteriaDTO)
        {
            if (searchCriteriaDTO == null)
            {
                return new SearchCriteriaByDocumentNumber();
            }

            return new SearchCriteriaByDocumentNumber
            {
                Ascending = searchCriteriaDTO.Ascending,
                DocumentNumber = searchCriteriaDTO.DocumentNumber,
                CultureName = searchCriteriaDTO.CultureName,
                OrderBy = searchCriteriaDTO.OrderBy,
                PageIndex = searchCriteriaDTO.PageIndex,
                PageSize = searchCriteriaDTO.PageSize,
                Year = searchCriteriaDTO.Year,
                OrgUnitId = searchCriteriaDTO.OrgUnitId,
                AdvancedSearch = Map(searchCriteriaDTO.AdvancedSearch),
                HasFullPrivilege = searchCriteriaDTO.HasFullPrivilege,
                Global = searchCriteriaDTO.Global

            };
        }

        public static SearchCriteriaByRecordNumber Map(SearchCriteriaByRecordNumberDTO searchCriteriaDTO)
        {
            if (searchCriteriaDTO == null)
            {
                return new SearchCriteriaByRecordNumber();
            }

            return new SearchCriteriaByRecordNumber
            {
                Ascending = searchCriteriaDTO.Ascending,
                RecordNumber = searchCriteriaDTO.RecordNumber,
                CultureName = searchCriteriaDTO.CultureName,
                OrderBy = searchCriteriaDTO.OrderBy,
                PageIndex = searchCriteriaDTO.PageIndex,
                PageSize = searchCriteriaDTO.PageSize,
                OrgUnitId = searchCriteriaDTO.OrgUnitId,
                AdvancedSearch = Map(searchCriteriaDTO.AdvancedSearch),
                HasFullPrivilege = searchCriteriaDTO.HasFullPrivilege,
                Global = searchCriteriaDTO.Global

            };
        }

        public static SearchCriteriaByInbound Map(SearchCriteriaByInboundDTO searchCriteriaDTO)
        {
            if (searchCriteriaDTO == null)
            {
                return new SearchCriteriaByInbound();
            }

            SearchCriteriaByInbound searchCriterias = new SearchCriteriaByInbound
            {
                AdvancedSearch = Map(searchCriteriaDTO.AdvancedSearch),
                Ascending = searchCriteriaDTO.Ascending,
                DateFrom = searchCriteriaDTO.FromDate,
                DateTo = searchCriteriaDTO.ToDate,
                Number = searchCriteriaDTO.Number,
                Year = searchCriteriaDTO.Year,
                CultureName = searchCriteriaDTO.CultureName,
                PageIndex = searchCriteriaDTO.PageIndex,
                PageSize = searchCriteriaDTO.PageSize,
                TransactionTypeId = searchCriteriaDTO.TransactionTypeId,
                OrderBy = searchCriteriaDTO.OrderBy,
                OrgUnitId = searchCriteriaDTO.OrgUnitId,
                TransactionCategoryId = searchCriteriaDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaDTO.FromDateTime,
                ToDateTime = searchCriteriaDTO.ToDateTime,
                UserId = searchCriteriaDTO.UserId,
                HasFullPrivilege = searchCriteriaDTO.HasFullPrivilege,
                Global = searchCriteriaDTO.Global,
                DeliveryMethodId = searchCriteriaDTO.DeliveryMethodId,
                DocumentNumber = searchCriteriaDTO.DocumentNumber,
            };

            return searchCriterias;
        }
        public static SearchCriteriaByOutboundInternal Map(SearchCriteriaByOutboundInternalDTO searchCriteriaDTO)
        {
            if (searchCriteriaDTO == null)
            {
                return new SearchCriteriaByOutboundInternal();
            }

            SearchCriteriaByOutboundInternal searchCriterias = new SearchCriteriaByOutboundInternal
            {
                AdvancedSearch = Map(searchCriteriaDTO.AdvancedSearch),
                Ascending = searchCriteriaDTO.Ascending,
                DateFrom = searchCriteriaDTO.FromDate,
                DateTo = searchCriteriaDTO.ToDate,
                Number = searchCriteriaDTO.Number,
                Year = searchCriteriaDTO.Year,
                CultureName = searchCriteriaDTO.CultureName,
                PageIndex = searchCriteriaDTO.PageIndex,
                PageSize = searchCriteriaDTO.PageSize,
                TypeId = searchCriteriaDTO.TypeId,
                OrderBy = searchCriteriaDTO.OrderBy,
                OrgUnitId = searchCriteriaDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaDTO.FromDateTime,
                ToDateTime = searchCriteriaDTO.ToDateTime,
                UserId = searchCriteriaDTO.UserId,
                HasFullPrivilege = searchCriteriaDTO.HasFullPrivilege,
                Global = searchCriteriaDTO.Global
            };

            return searchCriterias;
        }
        public static SearchCriteriaByOutbound Map(SearchCriteriaByOutboundDTO searchCriteriaDTO)
        {
            if (searchCriteriaDTO == null)
            {
                return new SearchCriteriaByOutbound();
            }

            SearchCriteriaByOutbound searchCriterias = new SearchCriteriaByOutbound
            {
                AdvancedSearch = Map(searchCriteriaDTO.AdvancedSearch),
                Ascending = searchCriteriaDTO.Ascending,
                DateFrom = searchCriteriaDTO.FromDate,
                DateTo = searchCriteriaDTO.ToDate,
                Number = searchCriteriaDTO.Number,
                Year = searchCriteriaDTO.Year,
                CultureName = searchCriteriaDTO.CultureName,
                OrderBy = searchCriteriaDTO.OrderBy,
                PageIndex = searchCriteriaDTO.PageIndex,
                PageSize = searchCriteriaDTO.PageSize,
                TypeId = searchCriteriaDTO.TypeId,
                OrgUnitId = searchCriteriaDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaDTO.FromDateTime,
                ToDateTime = searchCriteriaDTO.ToDateTime,
                UserId = searchCriteriaDTO.UserId,
                HasFullPrivilege = searchCriteriaDTO.HasFullPrivilege,
                Global = searchCriteriaDTO.Global,
                DeliveryMethodId = searchCriteriaDTO.DeliveryMethodId,
            };

            return searchCriterias;
        }
        public static SearchCriteriaByOutboundDraft Map(SearchCriteriaByOutboundDraftDTO searchCriteriaDTO)
        {
            if (searchCriteriaDTO == null)
            {
                return new SearchCriteriaByOutboundDraft();
            }

            SearchCriteriaByOutboundDraft searchCriterias = new SearchCriteriaByOutboundDraft
            {
                AdvancedSearch = Map(searchCriteriaDTO.AdvancedSearch),
                Ascending = searchCriteriaDTO.Ascending,
                DateFrom = searchCriteriaDTO.FromDate,
                DateTo = searchCriteriaDTO.ToDate,
                Number = searchCriteriaDTO.Number,
                Year = searchCriteriaDTO.Year,
                CultureName = searchCriteriaDTO.CultureName,
                OrderBy = searchCriteriaDTO.OrderBy,
                PageIndex = searchCriteriaDTO.PageIndex,
                PageSize = searchCriteriaDTO.PageSize,
                TypeId = searchCriteriaDTO.TypeId,
                OrgUnitId = searchCriteriaDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaDTO.FromDateTime,
                ToDateTime = searchCriteriaDTO.ToDateTime,
                UserId = searchCriteriaDTO.UserId,
                HasFullPrivilege = searchCriteriaDTO.HasFullPrivilege,
                Global = searchCriteriaDTO.Global
            };

            return searchCriterias;
        }
        public static SearchCriteriaBySubject Map(SearchCriteriaBySubjectDTO searchCriteriaDTO)
        {
            if (searchCriteriaDTO == null)
            {
                return new SearchCriteriaBySubject();
            }
            return new SearchCriteriaBySubject
            {
                AdvancedSearch = Map(searchCriteriaDTO.AdvancedSearch),
                Ascending = searchCriteriaDTO.Ascending,
                CultureName = searchCriteriaDTO.CultureName,
                OrderBy = searchCriteriaDTO.OrderBy,
                PageIndex = searchCriteriaDTO.PageIndex,
                PageSize = searchCriteriaDTO.PageSize,
                TypeId = searchCriteriaDTO.TypeId,
                Subject = searchCriteriaDTO.Subject,
                TransactionCategoryId = searchCriteriaDTO.TransactionCategoryId,
                OrgUnitId = searchCriteriaDTO.OrgUnitId,
                Year = searchCriteriaDTO.Year,
                UserId = searchCriteriaDTO.UserId,
                HasFullPrivilege = searchCriteriaDTO.HasFullPrivilege,
                Global = searchCriteriaDTO.Global
            };
        }
        public static SearchCriteriaByEntityName Map(SearchCriteriaByEntityNameDTO searchCriteriaDTO)
        {
            if (searchCriteriaDTO == null)
            {
                return new SearchCriteriaByEntityName();
            }

            SearchCriteriaByEntityName searchCriterias = new SearchCriteriaByEntityName
            {
                AdvancedSearch = Map(searchCriteriaDTO.AdvancedSearch),
                Ascending = searchCriteriaDTO.Ascending,
                DateFrom = searchCriteriaDTO.DateFrom,
                DateTo = searchCriteriaDTO.DateTo,
                CultureName = searchCriteriaDTO.CultureName,
                PageIndex = searchCriteriaDTO.PageIndex,
                PageSize = searchCriteriaDTO.PageSize,
                OrderBy = searchCriteriaDTO.OrderBy,
                OrgUnitId = searchCriteriaDTO.OrgUnitId,
                TransactionCategoryId = searchCriteriaDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaDTO.FromDateTime,
                ToDateTime = searchCriteriaDTO.ToDateTime,
                ExternalPartyId = searchCriteriaDTO.ExternalPartyId,
                Number = searchCriteriaDTO.Number,
                DocumentNumber = searchCriteriaDTO.DocumentNumber,  
                UserId = searchCriteriaDTO.UserId,
                HasFullPrivilege = searchCriteriaDTO.HasFullPrivilege,
                Global = searchCriteriaDTO.Global
            };

            return searchCriterias;
        }
        public static SearchCriteriaByCreator Map(SearchCriteriaByCreatorDTO searchCriteriaDTO)
        {
            if (searchCriteriaDTO == null)
            {
                return new SearchCriteriaByCreator();
            }

            SearchCriteriaByCreator searchCriterias = new SearchCriteriaByCreator
            {
                AdvancedSearch = Map(searchCriteriaDTO.AdvancedSearch),
                Ascending = searchCriteriaDTO.Ascending,
                DateFrom = searchCriteriaDTO.DateFrom,
                DateTo = searchCriteriaDTO.DateTo,
                CultureName = searchCriteriaDTO.CultureName,
                PageIndex = searchCriteriaDTO.PageIndex,
                PageSize = searchCriteriaDTO.PageSize,
                OrderBy = searchCriteriaDTO.OrderBy,
                OrgUnitId = searchCriteriaDTO.OrgUnitId,
                TransactionCategoryId = searchCriteriaDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaDTO.FromDateTime,
                ToDateTime = searchCriteriaDTO.ToDateTime,
                CreatorUserId = searchCriteriaDTO.CreatorUserId,
                UserId = searchCriteriaDTO.UserId,
                HasFullPrivilege = searchCriteriaDTO.HasFullPrivilege,
                Global = searchCriteriaDTO.Global
            };

            return searchCriterias;
        }

        public static SearchCriteriaByAssignTransaction Map(SearchCriteriaByAssignTransactionDTO searchCriteriaDTO)
        {
            if (searchCriteriaDTO == null)
            {
                return new SearchCriteriaByAssignTransaction();
            }

            SearchCriteriaByAssignTransaction searchCriterias = new SearchCriteriaByAssignTransaction
            {
                AdvancedSearch = Map(searchCriteriaDTO.AdvancedSearch),
                Ascending = searchCriteriaDTO.Ascending,
                DateFrom = searchCriteriaDTO.DateFrom,
                DateTo = searchCriteriaDTO.DateTo,
                CultureName = searchCriteriaDTO.CultureName,
                PageIndex = searchCriteriaDTO.PageIndex,
                PageSize = searchCriteriaDTO.PageSize,
                OrderBy = searchCriteriaDTO.OrderBy,
                OrgUnitId = searchCriteriaDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaDTO.FromDateTime,
                ToDateTime = searchCriteriaDTO.ToDateTime,
                FromEntity = searchCriteriaDTO.FromEntity,
                EntityId = searchCriteriaDTO.EntityId,
                UserId = searchCriteriaDTO.UserId,
                HasFullPrivilege = searchCriteriaDTO.HasFullPrivilege,
                Global = searchCriteriaDTO.Global
            };

            return searchCriterias;
        }

        public static SearchCriteriaByExternalPartyCopies Map(SearchCriteriaByExternalPartyCopiesDTO searchCriteriaDTO)
        {
            if (searchCriteriaDTO == null)
            {
                return new SearchCriteriaByExternalPartyCopies();
            }

            SearchCriteriaByExternalPartyCopies searchCriterias = new SearchCriteriaByExternalPartyCopies
            {
                Ascending = searchCriteriaDTO.Ascending,
                CultureName = searchCriteriaDTO.CultureName,
                OrderBy = searchCriteriaDTO.OrderBy,
                PageIndex = searchCriteriaDTO.PageIndex,
                PageSize = searchCriteriaDTO.PageSize,
                UserId = searchCriteriaDTO.UserId,
                HasFullPrivilege = searchCriteriaDTO.HasFullPrivilege,
                Global = searchCriteriaDTO.Global,
                DateFrom = searchCriteriaDTO.FromDate,
                DateTo = searchCriteriaDTO.ToDate,
                ExternalPartyId = searchCriteriaDTO.ExternalPartyId,
            };

            return searchCriterias;
        }


        public static OutboundAdvanced Map(OutboundAdvancedDTO advancedDTO)
        {
            if (advancedDTO == null)
            {
                return new OutboundAdvanced();
            }

            return new OutboundAdvanced
            {   ConfidentialityId = advancedDTO.ConfidentialityId,
                PriorityId = advancedDTO.PriorityId,
                StatusId = advancedDTO.StatusId,
                LetterTypeId = advancedDTO.LetterTypeId,
                SignedById =advancedDTO.SignedById,
                FromPartyId = advancedDTO.FromPartyId,
                SignedByDepartmentId = advancedDTO.SignedByDepartmentId,
                SubjectClassifications = advancedDTO.SubjectClassifications,
                DirectedToUserId = advancedDTO.DirectedToUserId,
                DestinationPartyId = advancedDTO.DestinationPartyId,
                CreatedDepartmentId = advancedDTO.CreatedDepartmentId,
                DirectedToId = advancedDTO.DirectedToId,
                 

            };
        }
        public static OutboundDraftAdvanced Map(OutboundDraftAdvancedDTO advancedDTO)
        {
            if (advancedDTO == null)
            {
                return new OutboundDraftAdvanced();
            }

            return new OutboundDraftAdvanced
            {
                ConfidentialityId = advancedDTO.ConfidentialityId,
                FromPartyId = advancedDTO.FromPartyId,
                LetterTypeId = advancedDTO.LetterTypeId,
                PriorityId = advancedDTO.PriorityId,
                SignedByDepartmentId = advancedDTO.SignedByDepartmentId,
                SignedById = advancedDTO.SignedById,
                StatusId = advancedDTO.StatusId,
                SubjectClassifications = advancedDTO.SubjectClassifications,

            };
        }
        public static BarcodeAdvanced Map(BarcodeAdvancedDTO advancedDTO)
        {
            if (advancedDTO == null)
            {
                return new BarcodeAdvanced();
            }

            return new BarcodeAdvanced
            {
                ConfidentialityId = advancedDTO.ConfidentialityId,
                FromPartyId = advancedDTO.FromPartyId,
                LetterTypeId = advancedDTO.LetterTypeId,
                PriorityId = advancedDTO.PriorityId,
                SignedByDepartmentId = advancedDTO.SignedByDepartmentId,
                SignedById = advancedDTO.SignedById,
                StatusId = advancedDTO.StatusId,
                SubjectClassifications = advancedDTO.SubjectClassifications,

            };
        }
        public static SubjectAdvanced Map(SubjectAdvancedDTO advancedDTO)
        {
            if (advancedDTO == null)
            {
                return new SubjectAdvanced();
            }

            return new SubjectAdvanced
            {
                ConfidentialityId = advancedDTO.ConfidentialityId,
                FromPartyId = advancedDTO.FromPartyId,
                LetterTypeId = advancedDTO.LetterTypeId,
                PriorityId = advancedDTO.PriorityId,
                SignedByDepartmentId = advancedDTO.SignedByDepartmentId,
                SignedById = advancedDTO.SignedById,
                StatusId = advancedDTO.StatusId,
                SubjectClassifications = advancedDTO.SubjectClassifications,

            };
        }
        public static InboundAdvanced Map(InboundAdvancedDTO advancedDTO)
        {
            if (advancedDTO == null)
            {
                return new InboundAdvanced();
            }

            return new InboundAdvanced
            {
                ConfidentialityId = advancedDTO.ConfidentialityId,
                PriorityId = advancedDTO.PriorityId,
                StatusId = advancedDTO.StatusId,
                LetterTypeId = advancedDTO.LetterTypeId,
                SignedById = advancedDTO.SignedById,
                FromPartyId = advancedDTO.FromPartyId,
                SignedByDepartmentId = advancedDTO.SignedByDepartmentId,
                SubjectClassifications = advancedDTO.SubjectClassifications,
                DirectedToUserId = advancedDTO.DirectedToUserId,
                DestinationPartyId = advancedDTO.DestinationPartyId,
                CreatedDepartmentId = advancedDTO.CreatedDepartmentId,
                DirectedToId = advancedDTO.DirectedToId,

            };
        }
        public static OutboundInternalAdvanced Map(OutboundInternalAdvancedDTO advancedDTO)
        {
            if (advancedDTO == null)
            {
                return new OutboundInternalAdvanced();
            }

            return new OutboundInternalAdvanced
            {
                ConfidentialityId = advancedDTO.ConfidentialityId,
                FromPartyId = advancedDTO.FromPartyId,
                LetterTypeId = advancedDTO.LetterTypeId,
                PriorityId = advancedDTO.PriorityId,
                SignedByDepartmentId = advancedDTO.SignedByDepartmentId,
                SignedById = advancedDTO.SignedById,
                StatusId = advancedDTO.StatusId,
                SubjectClassifications = advancedDTO.SubjectClassifications,

            };
        }




        public static SearchCriteriaByNames Map(SearchCriteriaByNamesDTO searchCriteriaByNamesDTO)
        {
            if (searchCriteriaByNamesDTO == null)
            {
                return new SearchCriteriaByNames();
            }

            SearchCriteriaByNames searchCriterias = new SearchCriteriaByNames
            {
                AdvancedSearch = Map(searchCriteriaByNamesDTO.AdvancedSearch),
                Ascending = searchCriteriaByNamesDTO.Ascending,
                DateFrom = searchCriteriaByNamesDTO.DateFrom,
                DateTo = searchCriteriaByNamesDTO.DateTo,
                CultureName = searchCriteriaByNamesDTO.CultureName,
                PageIndex = searchCriteriaByNamesDTO.PageIndex,
                PageSize = searchCriteriaByNamesDTO.PageSize,
                OrderBy = searchCriteriaByNamesDTO.OrderBy,
                OrgUnitId = searchCriteriaByNamesDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaByNamesDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaByNamesDTO.FromDateTime,
                ToDateTime = searchCriteriaByNamesDTO.ToDateTime,
                FirstName = searchCriteriaByNamesDTO.FirstName,
                SecondName = searchCriteriaByNamesDTO.SecondName,
                ThirdName = searchCriteriaByNamesDTO.ThirdName,
                FamilyName = searchCriteriaByNamesDTO.FamilyName,
                SearchNamesType = searchCriteriaByNamesDTO.SearchNamesType,
                UserId = searchCriteriaByNamesDTO.UserId,
                HasFullPrivilege = searchCriteriaByNamesDTO.HasFullPrivilege,
                Global = searchCriteriaByNamesDTO.Global
            };

            return searchCriterias;
        }
        public static SearchCriteriaByAssignmentNote Map(SearchCriteriaByAssignmentNoteDTO searchCriteriaByAssignmentNoteDTO)
        {
            if (searchCriteriaByAssignmentNoteDTO == null)
            {
                return new SearchCriteriaByAssignmentNote();
            }

            SearchCriteriaByAssignmentNote searchCriterias = new SearchCriteriaByAssignmentNote
            {
                AdvancedSearch = Map(searchCriteriaByAssignmentNoteDTO.AdvancedSearch),
                Ascending = searchCriteriaByAssignmentNoteDTO.Ascending,
                DateFrom = searchCriteriaByAssignmentNoteDTO.DateFrom,
                DateTo = searchCriteriaByAssignmentNoteDTO.DateTo,
                CultureName = searchCriteriaByAssignmentNoteDTO.CultureName,
                PageIndex = searchCriteriaByAssignmentNoteDTO.PageIndex,
                PageSize = searchCriteriaByAssignmentNoteDTO.PageSize,
                OrderBy = searchCriteriaByAssignmentNoteDTO.OrderBy,
                OrgUnitId = searchCriteriaByAssignmentNoteDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaByAssignmentNoteDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaByAssignmentNoteDTO.FromDateTime,
                ToDateTime = searchCriteriaByAssignmentNoteDTO.ToDateTime,
                AssignmentNote = searchCriteriaByAssignmentNoteDTO.AssignmentNote,
                UserId = searchCriteriaByAssignmentNoteDTO.UserId,
                HasFullPrivilege = searchCriteriaByAssignmentNoteDTO.HasFullPrivilege,
                Global = searchCriteriaByAssignmentNoteDTO.Global
            };

            return searchCriterias;
        }

        public static SearchCriteriaByCopyAssignemnt Map(SearchCriteriaByCopyAssignemntDTO searchCriteriaByCopyAssignemntDTO)
        {
            if (searchCriteriaByCopyAssignemntDTO == null)
            {
                return new SearchCriteriaByCopyAssignemnt();
            }

            SearchCriteriaByCopyAssignemnt searchCriterias = new SearchCriteriaByCopyAssignemnt
            {
                AdvancedSearch = Map(searchCriteriaByCopyAssignemntDTO.AdvancedSearch),
                Ascending = searchCriteriaByCopyAssignemntDTO.Ascending,
                DateFrom = searchCriteriaByCopyAssignemntDTO.DateFrom,
                DateTo = searchCriteriaByCopyAssignemntDTO.DateTo,
                CultureName = searchCriteriaByCopyAssignemntDTO.CultureName,
                PageIndex = searchCriteriaByCopyAssignemntDTO.PageIndex,
                PageSize = searchCriteriaByCopyAssignemntDTO.PageSize,
                OrderBy = searchCriteriaByCopyAssignemntDTO.OrderBy,
                OrgUnitId = searchCriteriaByCopyAssignemntDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaByCopyAssignemntDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaByCopyAssignemntDTO.FromDateTime,
                ToDateTime = searchCriteriaByCopyAssignemntDTO.ToDateTime,
                FromEntityId = searchCriteriaByCopyAssignemntDTO.FromEntityId,
                ToEntityId = searchCriteriaByCopyAssignemntDTO.ToEntityId,
                UserId = searchCriteriaByCopyAssignemntDTO.UserId,
                HasFullPrivilege = searchCriteriaByCopyAssignemntDTO.HasFullPrivilege,
                Global = searchCriteriaByCopyAssignemntDTO.Global
            };

            return searchCriterias;
        }
        public static SearchCriteriaByDaily Map(SearchCriteriaByDailyDTO searchCriteriaByDailyDTO)
        {
            if (searchCriteriaByDailyDTO == null)
            {
                return new SearchCriteriaByDaily();
            }

            SearchCriteriaByDaily searchCriterias = new SearchCriteriaByDaily
            {
                Ascending = searchCriteriaByDailyDTO.Ascending,
                CultureName = searchCriteriaByDailyDTO.CultureName,
                PageIndex = searchCriteriaByDailyDTO.PageIndex,
                PageSize = searchCriteriaByDailyDTO.PageSize,
                TodayDate = searchCriteriaByDailyDTO.TodayDate,
                OrderBy = searchCriteriaByDailyDTO.OrderBy,
                UserId = searchCriteriaByDailyDTO.UserId,
                Global = searchCriteriaByDailyDTO.Global
            };

            return searchCriterias;
        }

        public static SearchCriteriaByElcEmployee Map(SearchCriteriaByElcEmployeeDTO searchCriteriaByElcEmployeeDTO)
        {
            if (searchCriteriaByElcEmployeeDTO == null)
            {
                return new SearchCriteriaByElcEmployee();
            }

            SearchCriteriaByElcEmployee searchCriterias = new SearchCriteriaByElcEmployee
            {
                AdvancedSearch = Map(searchCriteriaByElcEmployeeDTO.AdvancedSearch),
                Ascending = searchCriteriaByElcEmployeeDTO.Ascending,
                DateFrom = searchCriteriaByElcEmployeeDTO.DateFrom,
                DateTo = searchCriteriaByElcEmployeeDTO.DateTo,
                CultureName = searchCriteriaByElcEmployeeDTO.CultureName,
                PageIndex = searchCriteriaByElcEmployeeDTO.PageIndex,
                PageSize = searchCriteriaByElcEmployeeDTO.PageSize,
                OrderBy = searchCriteriaByElcEmployeeDTO.OrderBy,
                OrgUnitId = searchCriteriaByElcEmployeeDTO.OrgUnitId,
                TransactionCategoryId = searchCriteriaByElcEmployeeDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaByElcEmployeeDTO.FromDateTime,
                ToDateTime = searchCriteriaByElcEmployeeDTO.ToDateTime, 
                ElcEmployeeId = searchCriteriaByElcEmployeeDTO.ElcEmployeeId,
                UserId = searchCriteriaByElcEmployeeDTO.UserId,
                HasFullPrivilege = searchCriteriaByElcEmployeeDTO.HasFullPrivilege,
                Global = searchCriteriaByElcEmployeeDTO.Global
            };

            return searchCriterias;
        }
        public static SearchCriteriaByExternalOutBoundOrManifestNumber Map(SearchCriteriaByExternalOutBoundOrManifestNumberDTO searchCriteriaByExternalOutBoundOrManifestNumberDTO)
        {
            if (searchCriteriaByExternalOutBoundOrManifestNumberDTO == null)
            {
                return new SearchCriteriaByExternalOutBoundOrManifestNumber();
            }

            SearchCriteriaByExternalOutBoundOrManifestNumber searchCriterias = new SearchCriteriaByExternalOutBoundOrManifestNumber
            {
                AdvancedSearch = Map(searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch),
                Ascending = searchCriteriaByExternalOutBoundOrManifestNumberDTO.Ascending,
                DateFrom = searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom,
                DateTo = searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo,
                CultureName = searchCriteriaByExternalOutBoundOrManifestNumberDTO.CultureName,
                PageIndex = searchCriteriaByExternalOutBoundOrManifestNumberDTO.PageIndex,
                PageSize = searchCriteriaByExternalOutBoundOrManifestNumberDTO.PageSize,
                OrderBy = searchCriteriaByExternalOutBoundOrManifestNumberDTO.OrderBy,
                OrgUnitId = searchCriteriaByExternalOutBoundOrManifestNumberDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaByExternalOutBoundOrManifestNumberDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime,
                ToDateTime = searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime,
                Number = searchCriteriaByExternalOutBoundOrManifestNumberDTO.Number,
                Year = searchCriteriaByExternalOutBoundOrManifestNumberDTO.Year,
                UserId = searchCriteriaByExternalOutBoundOrManifestNumberDTO.UserId,
                HasFullPrivilege = searchCriteriaByExternalOutBoundOrManifestNumberDTO.HasFullPrivilege,
                Global = searchCriteriaByExternalOutBoundOrManifestNumberDTO.Global
            };

            return searchCriterias;
        }
        public static SearchCriteriaByManifestNumber Map(SearchCriteriaByManifestNumberDTO searchCriteriaByManifestNumberDTO)
        {
            if (searchCriteriaByManifestNumberDTO == null)
            {
                return new SearchCriteriaByManifestNumber();
            }

            SearchCriteriaByManifestNumber searchCriterias = new SearchCriteriaByManifestNumber
            {
                AdvancedSearch = Map(searchCriteriaByManifestNumberDTO.AdvancedSearch),
                Ascending = searchCriteriaByManifestNumberDTO.Ascending,
                DateFrom = searchCriteriaByManifestNumberDTO.DateFrom,
                DateTo = searchCriteriaByManifestNumberDTO.DateTo,
                CultureName = searchCriteriaByManifestNumberDTO.CultureName,
                PageIndex = searchCriteriaByManifestNumberDTO.PageIndex,
                PageSize = searchCriteriaByManifestNumberDTO.PageSize,
                OrderBy = searchCriteriaByManifestNumberDTO.OrderBy,
                OrgUnitId = searchCriteriaByManifestNumberDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaByManifestNumberDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaByManifestNumberDTO.FromDateTime,
                ToDateTime = searchCriteriaByManifestNumberDTO.ToDateTime,
                ManifestNumber = searchCriteriaByManifestNumberDTO.ManifestNumber,
                UserId = searchCriteriaByManifestNumberDTO.UserId,
                HasFullPrivilege = searchCriteriaByManifestNumberDTO.HasFullPrivilege,
                Global = searchCriteriaByManifestNumberDTO.Global
            };
             
            return searchCriterias;
        }
        public static SearchCriteriaByMilitaryNumberOrIdentity Map(SearchCriteriaByMilitaryNumberOrIdentityDTO searchCriteriaByMilitaryNumberOrIdentityDTO)
        {
            if (searchCriteriaByMilitaryNumberOrIdentityDTO == null)
            {
                return new SearchCriteriaByMilitaryNumberOrIdentity();
            }

            SearchCriteriaByMilitaryNumberOrIdentity searchCriterias = new SearchCriteriaByMilitaryNumberOrIdentity
            {
                AdvancedSearch = Map(searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch),
                Ascending = searchCriteriaByMilitaryNumberOrIdentityDTO.Ascending,
                DateFrom = searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom,
                DateTo = searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo,
                CultureName = searchCriteriaByMilitaryNumberOrIdentityDTO.CultureName,
                PageIndex = searchCriteriaByMilitaryNumberOrIdentityDTO.PageIndex,
                PageSize = searchCriteriaByMilitaryNumberOrIdentityDTO.PageSize,
                OrderBy = searchCriteriaByMilitaryNumberOrIdentityDTO.OrderBy,
                OrgUnitId = searchCriteriaByMilitaryNumberOrIdentityDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaByMilitaryNumberOrIdentityDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime,
                ToDateTime = searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime,
                IdentificationNumber = searchCriteriaByMilitaryNumberOrIdentityDTO.IdentificationNumber,
                UserId = searchCriteriaByMilitaryNumberOrIdentityDTO.UserId,
                HasFullPrivilege = searchCriteriaByMilitaryNumberOrIdentityDTO.HasFullPrivilege,
                Global = searchCriteriaByMilitaryNumberOrIdentityDTO.Global
            };

            return searchCriterias;
        }
        public static SearchCriteriaBySubjectLetter Map(SearchCriteriaBySubjectLetterDTO searchCriteriaBySubjectLetterDTO)
        {
            if (searchCriteriaBySubjectLetterDTO == null)
            {
                return new SearchCriteriaBySubjectLetter();
            }

            SearchCriteriaBySubjectLetter searchCriterias = new SearchCriteriaBySubjectLetter
            {
                AdvancedSearch = Map(searchCriteriaBySubjectLetterDTO.AdvancedSearch),
                Ascending = searchCriteriaBySubjectLetterDTO.Ascending,
                DateFrom = searchCriteriaBySubjectLetterDTO.DateFrom,
                DateTo = searchCriteriaBySubjectLetterDTO.DateTo,
                CultureName = searchCriteriaBySubjectLetterDTO.CultureName,
                PageIndex = searchCriteriaBySubjectLetterDTO.PageIndex,
                PageSize = searchCriteriaBySubjectLetterDTO.PageSize,
                OrderBy = searchCriteriaBySubjectLetterDTO.OrderBy,
                OrgUnitId = searchCriteriaBySubjectLetterDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaBySubjectLetterDTO.TransactionCategoryId,
                 FirstLetter  = searchCriteriaBySubjectLetterDTO.FirstLetter,
                SecondLetter = searchCriteriaBySubjectLetterDTO.SecondLetter,
                ThirdLetter  = searchCriteriaBySubjectLetterDTO.ThirdLetter,
                FourthLetter = searchCriteriaBySubjectLetterDTO.FourthLetter,
                SearchTypeForFiltersId = searchCriteriaBySubjectLetterDTO.SearchTypeForFiltersId,
                FromDateTime = searchCriteriaBySubjectLetterDTO.FromDateTime,
                ToDateTime = searchCriteriaBySubjectLetterDTO.ToDateTime,
                 UserId = searchCriteriaBySubjectLetterDTO.UserId,
                HasFullPrivilege = searchCriteriaBySubjectLetterDTO.HasFullPrivilege,
                Global = searchCriteriaBySubjectLetterDTO.Global
            };

            return searchCriterias;
        }
        public static SearchCriteriaByTransactionNots Map(SearchCriteriaByTransactionNotsDTO searchCriteriaByTransactionNotsDTO)
        {
            if (searchCriteriaByTransactionNotsDTO == null)
            {
                return new SearchCriteriaByTransactionNots();
            }

            SearchCriteriaByTransactionNots searchCriterias = new SearchCriteriaByTransactionNots
            {
                AdvancedSearch = Map(searchCriteriaByTransactionNotsDTO.AdvancedSearch),
                Ascending = searchCriteriaByTransactionNotsDTO.Ascending,
                DateFrom = searchCriteriaByTransactionNotsDTO.DateFrom,
                DateTo = searchCriteriaByTransactionNotsDTO.DateTo,
                CultureName = searchCriteriaByTransactionNotsDTO.CultureName,
                PageIndex = searchCriteriaByTransactionNotsDTO.PageIndex,
                PageSize = searchCriteriaByTransactionNotsDTO.PageSize,
                OrderBy = searchCriteriaByTransactionNotsDTO.OrderBy,
                OrgUnitId = searchCriteriaByTransactionNotsDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaByTransactionNotsDTO.TransactionCategoryId,
                TransactionNots = searchCriteriaByTransactionNotsDTO.TransactionNots,
                FromDateTime = searchCriteriaByTransactionNotsDTO.FromDateTime,
                ToDateTime = searchCriteriaByTransactionNotsDTO.ToDateTime,
                 UserId = searchCriteriaByTransactionNotsDTO.UserId,
                HasFullPrivilege = searchCriteriaByTransactionNotsDTO.HasFullPrivilege,
                Global = searchCriteriaByTransactionNotsDTO.Global
            };

            return searchCriterias;
        }

        public static SearchCriteriaByTransactionNumber Map(SearchCriteriaByTransactionNumberDTO searchCriteriaByTransactionNumberDTO)
        {
            if (searchCriteriaByTransactionNumberDTO == null)
            {
                return new SearchCriteriaByTransactionNumber();
            }

            SearchCriteriaByTransactionNumber searchCriterias = new SearchCriteriaByTransactionNumber
            {
                AdvancedSearch = Map(searchCriteriaByTransactionNumberDTO.AdvancedSearch),
                Ascending = searchCriteriaByTransactionNumberDTO.Ascending,
                DateFrom = searchCriteriaByTransactionNumberDTO.DateFrom,
                DateTo = searchCriteriaByTransactionNumberDTO.DateTo,
                CultureName = searchCriteriaByTransactionNumberDTO.CultureName,
                PageIndex = searchCriteriaByTransactionNumberDTO.PageIndex,
                PageSize = searchCriteriaByTransactionNumberDTO.PageSize,
                OrderBy = searchCriteriaByTransactionNumberDTO.OrderBy,
                OrgUnitId = searchCriteriaByTransactionNumberDTO.OrgUnitId,
                TransactionTypeId = searchCriteriaByTransactionNumberDTO.TransactionCategoryId,
                FromDateTime = searchCriteriaByTransactionNumberDTO.FromDateTime,
                ToDateTime = searchCriteriaByTransactionNumberDTO.ToDateTime,
                TransactionNumber = searchCriteriaByTransactionNumberDTO.TransactionNumber,
                UserId = searchCriteriaByTransactionNumberDTO.UserId,
                HasFullPrivilege = searchCriteriaByTransactionNumberDTO.HasFullPrivilege,
                Global = searchCriteriaByTransactionNumberDTO.Global
            };

            return searchCriterias;
        }

    }
}