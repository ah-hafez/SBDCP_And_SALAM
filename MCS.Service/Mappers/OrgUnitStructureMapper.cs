using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Localization.SupportClasses;
using MCS.Business;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class OrgUnitStructureMapper
    {
        public static List<OrgStructureInfoDTO> Map(IList<OrgUnit> orgUnits, string cultureName)
        {
            if (orgUnits == null || !orgUnits.Any())
            {
                return null;
            }
            List<OrgStructureInfoDTO> orgUnitDTOs = new List<OrgStructureInfoDTO>();

            foreach (OrgUnit orgUnit in orgUnits)
            {
                if (orgUnit != null)
                {
                    OrgStructureInfoDTO orgUnitDTO = MapOrgUnit(orgUnit, cultureName);

                    if (orgUnit.Parent == null && !orgUnit.IsDeleted)
                        orgUnitDTO.ParentId = -1;
                    if (orgUnit.Parent != null)
                        orgUnitDTO.ParentId = orgUnit.Parent.Id;

                    if (orgUnit.Counter != null)
                        orgUnitDTO.Counter = MapCounter(orgUnit, cultureName);

                    if (orgUnit.AssignmentPaper != null)
                        orgUnitDTO.AssignmentPaper = MapAssignmentPaper(orgUnit.AssignmentPaper, cultureName);

                    if (orgUnit.BarcodeDesigns != null)
                        orgUnitDTO.BarcodeDesigners = BarcodeMapper.Map(orgUnit.BarcodeDesigns);

                    orgUnitDTO.LinkUnitsKeys = new List<int>();

                    if (orgUnit.Links != null)
                    {
                        foreach (OrgUnitLink orgUnitLink in orgUnit.Links)
                        {
                            int linkUnitKey = orgUnitLink.ToEntity.Id;

                            orgUnitDTO.LinkUnitsKeys.Add(linkUnitKey);
                        }
                    }

                    orgUnitDTO.Users = new List<OrgUnitUserDTO>();

                    if (orgUnit.Users != null)
                    {
                        foreach (UserProfile userProfile in orgUnit.Users)
                        {
                            OrgUnitUserDTO orgUnitUserDTO = new OrgUnitUserDTO()
                            {
                                Id = userProfile.Id,
                            };

                            if (userProfile.LocalizationIdentifier != null && userProfile.LocalizationIdentifier.Localizations != null)
                            {
                                orgUnitUserDTO.UserName = userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText();
                            }

                            orgUnitDTO.Users.Add(orgUnitUserDTO);
                        }
                    }

                    orgUnitDTOs.Add(orgUnitDTO);
                }

            }

            return orgUnitDTOs;
        }

        public static List<OrgUnit> Map(IList<OrgStructureInfoDTO> orgStructureInfoDTOs)
        {
            if (orgStructureInfoDTOs == null || !orgStructureInfoDTOs.Any())
            {
                return null;
            }
            List<OrgUnit> orgUnits = new List<OrgUnit>();

            foreach (OrgStructureInfoDTO orgStructureInfoDTO in orgStructureInfoDTOs)
            {
                //if (!orgStructureInfoDTO.IsDeleted)
                //{
                //if (orgStructureInfoDTO.Key == 34574)
                //{
                //    int x = 0;
                //}

                OrgUnit orgUnit = MapOrgUnit(orgStructureInfoDTO);

                OrgStructureInfoDTO orgStructureInfo =
                    orgStructureInfoDTOs.Where(u => u.Key == orgStructureInfoDTO.ParentId).FirstOrDefault();

                if (orgStructureInfo != null)
                    orgUnit.ParentId = orgStructureInfo.Key; //MapOrgUnit(orgStructureInfo);

                if (orgStructureInfoDTO.Counter == null)
                {
                    orgStructureInfoDTO.Counter = new CounterDTO()
                    {
                        IsGeneral = true,
                    };
                }

                bool isRootOrgUnit = (orgStructureInfo == null);

                orgUnit.Counter = MapCounter(orgStructureInfoDTO.Counter, isRootOrgUnit, orgStructureInfoDTOs);

                orgUnit.Counter.CounterDetails.ToList().ForEach(c =>
                {
                    c.Counter = orgUnit.Counter;
                });

                orgUnit.JoinToGeneralCounter = orgStructureInfoDTO.Counter.IsGeneral;

                if (orgStructureInfoDTO.AssignmentPaper != null)
                    orgUnit.AssignmentPaper = MapAssignmentPaper(orgStructureInfoDTO.AssignmentPaper, orgStructureInfoDTOs);

                orgUnit.Links = new List<OrgUnitLink>();

                foreach (int linkUnitKey in orgStructureInfoDTO.LinkUnitsKeys)
                {
                    OrgStructureInfoDTO linkUnitDTO =
                        orgStructureInfoDTOs.Where(o => o.Key == linkUnitKey).FirstOrDefault();

                    OrgUnitLink orgUnitLink = new OrgUnitLink()
                    {
                        FromEntity = orgUnit,
                        ToEntity = MapOrgUnit(linkUnitDTO)
                    };

                    orgUnit.Links.Add(orgUnitLink);
                }

                IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                orgUnit.Users = new List<UserProfile>();
                if (orgStructureInfoDTO.Users != null)
                {
                    foreach (OrgUnitUserDTO orgUnitUserDTO in orgStructureInfoDTO.Users)
                    {
                        UserProfile userProfile = userManagementBL.GetUserById(orgUnitUserDTO.Id);

                        orgUnit.Users.Add(userProfile);
                    }
                }

                if (orgStructureInfoDTO.BarcodeDesigners != null)
                {
                    orgUnit.BarcodeDesigns = BarcodeMapper.Map(orgStructureInfoDTO.BarcodeDesigners);
                }

                orgUnits.Add(orgUnit);
                //}
                //else
                //{
                //    //IOrgUnitBL orgUnitStructureBL = IoC.Resolve<IOrgUnitBL>();

                //    OrgUnit orgUnit = MapOrgUnit(orgStructureInfoDTO);  //orgUnitStructureBL.GetOrgUnitById(orgStructureInfoDTO.Key);

                //    orgUnit.IsDeleted = true;

                //    orgUnits.Add(orgUnit);
                //}
            }

            return orgUnits;
        }

        public static AssignmentPaper Map(AssignmentPaperDTO assignmentPaperDTO, OrgUnit orgUnit)
        {
            //orgunitnulls
            if (assignmentPaperDTO == null)
                return null;

            AssignmentPaper assignmentPaper = new AssignmentPaper();

            assignmentPaper.IsCreateGroupAllowed = assignmentPaperDTO.IsCreateGroupAllowed;

            //IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
            //IActionBL actionBL = IoC.Resolve<IActionBL>();

            assignmentPaper.AssignmentPaperBeneficiaries = new List<AssignmentPaperBeneficiary>();
            assignmentPaper.AssignmentPaperActions = new List<AssignmentPaperAction>();

            foreach (AssignmentPaperBeneficiaryDTO beneficiaryDTO in assignmentPaperDTO.Beneficiaries)
            {
                AssignmentPaperBeneficiary beneficiary = new AssignmentPaperBeneficiary()
                {
                    Id = beneficiaryDTO.Id,
                    OrgUnit = orgUnit
                };

                if (beneficiaryDTO.UserId.HasValue && beneficiaryDTO.UserId != 0)
                    beneficiary.UserId = beneficiaryDTO.UserId.Value; // userManagementBL.GetUserById(beneficiaryDTO.UserId.Value);

                assignmentPaper.AssignmentPaperBeneficiaries.Add(beneficiary);
            }

            foreach (AssignmentPaperActionDTO assignmentPaperActionDTO in assignmentPaperDTO.Actions)
            {
                AssignmentPaperAction assignmentPaperAction = new AssignmentPaperAction();

                assignmentPaperAction.Id = assignmentPaperActionDTO.Id;
                assignmentPaperAction.ActionId = assignmentPaperActionDTO.ActionId; // actionBL.GetActionById(assignmentPaperActionDTO.ActionId);

                assignmentPaper.AssignmentPaperActions.Add(assignmentPaperAction);
            }

            return assignmentPaper;
        }

        public static AssignmentPaperDTO Map(AssignmentPaper assignmentPaper, OrgUnit orgUnit)
        {
            if (assignmentPaper == null)
                return null;

            AssignmentPaperDTO assignmentPaperDTO = new AssignmentPaperDTO();

            assignmentPaperDTO.IsCreateGroupAllowed = assignmentPaper.IsCreateGroupAllowed;

            //IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
            //IActionBL actionBL = IoC.Resolve<IActionBL>();

            assignmentPaperDTO.Beneficiaries = new List<AssignmentPaperBeneficiaryDTO>();
            assignmentPaperDTO.Actions = new List<AssignmentPaperActionDTO>();

            foreach (AssignmentPaperBeneficiary beneficiary in assignmentPaper.AssignmentPaperBeneficiaries)
            {
                AssignmentPaperBeneficiaryDTO beneficiaryDTO = new AssignmentPaperBeneficiaryDTO()
                {
                    Id = beneficiary.Id,
                    OrgUnitName = beneficiary.OrgUnit.LocalName
                };

                if (beneficiary.UserId.HasValue && beneficiary.UserId != 0)
                    beneficiaryDTO.UserId = beneficiary.UserId.Value; // userManagementBL.GetUserById(beneficiaryDTO.UserId.Value);

                assignmentPaperDTO.Beneficiaries.Add(beneficiaryDTO);
            }

            foreach (AssignmentPaperAction assignmentPaperAction in assignmentPaper.AssignmentPaperActions)
            {
                AssignmentPaperActionDTO assignmentPaperActionDTO = new AssignmentPaperActionDTO();

                assignmentPaperActionDTO.Id = assignmentPaperAction.Id;
                assignmentPaperActionDTO.ActionId = assignmentPaperAction.ActionId; // actionBL.GetActionById(assignmentPaperActionDTO.ActionId);

                assignmentPaperDTO.Actions.Add(assignmentPaperActionDTO);
            }

            return assignmentPaperDTO;
        }

        public static AssignmentPaper MapAssignmentPaper(AssignmentPaperDTO assignmentPaperDTO, IList<OrgStructureInfoDTO> orgStructureInfoDTOs)
        {
            if (assignmentPaperDTO == null || orgStructureInfoDTOs == null || !orgStructureInfoDTOs.Any())
            {
                return null;
            }
            AssignmentPaper assignmentPaper = new AssignmentPaper();

            assignmentPaper.IsCreateGroupAllowed = assignmentPaperDTO.IsCreateGroupAllowed;

            //IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
            //IActionBL actionBL = IoC.Resolve<IActionBL>();

            assignmentPaper.AssignmentPaperBeneficiaries = new List<AssignmentPaperBeneficiary>();
            assignmentPaper.AssignmentPaperActions = new List<AssignmentPaperAction>();

            if (assignmentPaperDTO.Beneficiaries != null)//DatabaseNulls
            {
                foreach (AssignmentPaperBeneficiaryDTO beneficiaryDTO in assignmentPaperDTO.Beneficiaries)
                {
                    OrgStructureInfoDTO orgStructureInfoDTO =
                        orgStructureInfoDTOs.Where(u => u.Key == beneficiaryDTO.BeneficiaryOrgUnitId).FirstOrDefault();

                    AssignmentPaperBeneficiary beneficiary = new AssignmentPaperBeneficiary()
                    {
                        Id = beneficiaryDTO.Id,
                        OrgUnitId = orgStructureInfoDTO.Key //MapOrgUnit(orgStructureInfoDTO)
                    };

                    if (beneficiaryDTO.UserId.HasValue && beneficiaryDTO.UserId != 0)
                        beneficiary.UserId = beneficiaryDTO.UserId; //userManagementBL.GetUserById(beneficiaryDTO.UserId.Value);

                    assignmentPaper.AssignmentPaperBeneficiaries.Add(beneficiary);
                }
            }
            if (assignmentPaperDTO.Actions != null) //DatabaseNulls
            {
                foreach (AssignmentPaperActionDTO assignmentPaperActionDTO in assignmentPaperDTO.Actions)
                {
                    AssignmentPaperAction assignmentPaperAction = new AssignmentPaperAction();

                    assignmentPaperAction.Id = assignmentPaperActionDTO.Id;
                    assignmentPaperAction.ActionId = assignmentPaperActionDTO.ActionId; // actionBL.GetActionById(assignmentPaperActionDTO.ActionId);

                    assignmentPaper.AssignmentPaperActions.Add(assignmentPaperAction);
                }
            }
            return assignmentPaper;
        }

        public static List<TransactionsCountDTO> MapTransactionsCount(List<Transaction> transactions)
        {
            if (transactions == null || !transactions.Any())
            {
                return null;
            }
            List<TransactionsCountDTO> transactionsCountDTO = new List<TransactionsCountDTO>();

            foreach (var transaction in transactions)
            {
                TransactionsCountDTO transactionCount = new TransactionsCountDTO()
                {
                    //Count = transaction.Number,
                    TransactionCategoryId = (int)EnumMapper.GetTransactionCategory((TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionStatus, string.Empty))
                };

                transactionsCountDTO.Add(transactionCount);
            }

            return transactionsCountDTO;
        }

        private static AssignmentPaperDTO MapAssignmentPaper(AssignmentPaper assignmentPaper, string cultureName)
        {
            if (assignmentPaper == null)
                return null;

            AssignmentPaperDTO assignmentPaperDTO = new AssignmentPaperDTO();

            assignmentPaperDTO.IsCreateGroupAllowed = assignmentPaper.IsCreateGroupAllowed;
            assignmentPaperDTO.Beneficiaries = new List<AssignmentPaperBeneficiaryDTO>();

            foreach (AssignmentPaperBeneficiary assignmentPaperBeneficiary in assignmentPaper.AssignmentPaperBeneficiaries)
            {
                AssignmentPaperBeneficiaryDTO beneficiary = new AssignmentPaperBeneficiaryDTO()
                {
                    Id = assignmentPaperBeneficiary.Id,
                    BeneficiaryOrgUnitId = assignmentPaperBeneficiary.OrgUnit.Id,
                    OrgUnitName = assignmentPaperBeneficiary.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                };

                if (assignmentPaperBeneficiary.User != null)
                {
                    beneficiary.UserId = assignmentPaperBeneficiary.User.Id;
                    beneficiary.UserName = assignmentPaperBeneficiary.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText();
                }

                assignmentPaperDTO.Beneficiaries.Add(beneficiary);
            }

            assignmentPaperDTO.Actions = new List<AssignmentPaperActionDTO>();

            foreach (AssignmentPaperAction assignmentPaperAction in assignmentPaper.AssignmentPaperActions)
            {
                AssignmentPaperActionDTO assignmentPaperActionDTO = new AssignmentPaperActionDTO()
                {
                    ActionId = assignmentPaperAction.Action.Id,
                    Id = assignmentPaperAction.Id,
                    Name = assignmentPaperAction.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                };

                assignmentPaperDTO.Actions.Add(assignmentPaperActionDTO);
            }

            return assignmentPaperDTO;
        }

        private static Counter MapCounter(CounterDTO counterDTO, bool isRootOrgUnit, IList<OrgStructureInfoDTO> orgStructureInfoDTOs)
        {
            if (counterDTO == null || orgStructureInfoDTOs == null || !orgStructureInfoDTOs.Any())
            {
                return null;
            }
            Counter counter = null;

            if (!counterDTO.IsGeneral || isRootOrgUnit)
            {
                counter = new Counter()
                {
                    Id = counterDTO.Id,
                    IsGeneral = isRootOrgUnit,
                    CounterDetails = new List<CounterDetail>()
                };

                foreach (CounterDetailDTO counterDetailDTO in counterDTO.CounterDetails)
                {
                    CounterDetail counterDetail = new CounterDetail()
                    {
                        Id = counterDetailDTO.Id,
                        TransactionCategories = TransactionCategoryMapper.Map(counterDetailDTO.TransactionCategories),
                        InitialValue = counterDetailDTO.InitialValue
                    };
                    counterDetail.Count = counterDetail.InitialValue;
                    counter.CounterDetails.Add(counterDetail);
                }
            }
            else
            {
                OrgStructureInfoDTO orgStructureInfoDTO =
                    orgStructureInfoDTOs.Where(o => o.ParentId == -1).FirstOrDefault();

                if (orgStructureInfoDTO != null)
                {
                    counter = MapCounter(orgStructureInfoDTO.Counter, true, orgStructureInfoDTOs);
                }
            }

            return counter;
        }

        private static CounterDTO MapCounter(OrgUnit orgUnit, string cultureName)
        {
            if (orgUnit == null)
                return null;

            ILookupBL lookupBL = IoC.Resolve<ILookupBL>();
            IList<Lookup> lookups = lookupBL.GetLookupItems(LookupCategory.TransactionCategories, cultureName);
            List<LookupDTO> lookupDTOs = LookupMapper.Map(lookups);
            List<TransactionCategoryDTO> transactionCategoryDTOs = new List<TransactionCategoryDTO>();

            if (lookupDTOs != null)
            {
                foreach (LookupDTO lookupDTO in lookupDTOs)
                {
                    transactionCategoryDTOs.Add(new TransactionCategoryDTO()
                    {
                        Id = (lookupDTO.EnumReference != null) ? lookupDTO.EnumReference.Value : -1,
                        Text = lookupDTO.Text,
                    });
                }
            }

            List<CounterDetailDTO> counterDetailDTOs = new List<CounterDetailDTO>();

            CounterDetailDTO counterDetailDTO = null;

            transactionCategoryDTOs.ForEach(t =>
            {
                counterDetailDTO = new CounterDetailDTO();

                if (orgUnit.Counter.CounterDetails.ToList().Where(d => (int)d.TransactionCategories == t.Id).Count() != 0)
                {
                    counterDetailDTO.Id = orgUnit.Counter.CounterDetails.ToList().Where(d => (int)d.TransactionCategories == t.Id).FirstOrDefault().Id;
                    counterDetailDTO.InitialValue = orgUnit.Counter.CounterDetails.ToList().Where(d => (int)d.TransactionCategories == t.Id).FirstOrDefault().InitialValue;
                    counterDetailDTO.LastTransactionNumber = orgUnit.Counter.CounterDetails.ToList().Where(d => (int)d.TransactionCategories == t.Id).FirstOrDefault().Count;
                }
                counterDetailDTOs.Add(counterDetailDTO);
            });

            CounterDTO counterDTO = new CounterDTO()
            {
                Id = orgUnit.Counter.Id,
                IsGeneral = orgUnit.JoinToGeneralCounter,
                CounterDetails = counterDetailDTOs
            };

            return counterDTO;
        }

        public static OrgUnit MapOrgUnit(OrgStructureInfoDTO orgStructureInfoDTO)
        {
            if (orgStructureInfoDTO == null)
                return null;

            OrgUnit orgUnit = new OrgUnit()
            {
                Id = orgStructureInfoDTO.Key,
                IsActive = true,
                Number = orgStructureInfoDTO.Number,
                BarCode = orgStructureInfoDTO.BarCode,
                IsVirtualUnit = orgStructureInfoDTO.IsVirtualUnit,
                IsDeleted = orgStructureInfoDTO.IsDeleted,
                TransactionsProcessingPeriod = orgStructureInfoDTO.TransactionsProcessingPeriod,
                LocalizationIdentifier = orgStructureInfoDTO.Names != null ? LocalizationIdentifierMapper.Map(orgStructureInfoDTO.Names) : null,
                IsNew = orgStructureInfoDTO.IsNew,
                ManagerId = orgStructureInfoDTO.ManagerId,
                ParentId = orgStructureInfoDTO.ParentId,
                ExternalId = orgStructureInfoDTO.ExternalId,
                IoDepartment = orgStructureInfoDTO.IoDepartment,
                FollowUpDepartment = orgStructureInfoDTO.FollowUpDepartment,
                IsExecutive = orgStructureInfoDTO.IsExecutive,
                IsGeneralIoDepartment = orgStructureInfoDTO.IsGeneralIoDepartment,
                ReceiveWithAcknowled = orgStructureInfoDTO.ReceiveElcOutBoundWithAcknowled,
                SendSpecialCopy = orgStructureInfoDTO.SendSpecialCopy,
                Lineage = orgStructureInfoDTO.Lineage,
            };

            orgUnit.LocalizationIdentifier.Id = orgStructureInfoDTO.IdentifierId;

            return orgUnit;
        }

        public static OrgStructureInfoDTO MapOrgUnit(OrgUnit orgUnit, string cultureName)
        {
            if (orgUnit == null)
                return null;
            IOrgUnitBL orgUnitBL = new OrgUnitBL();
            OrgStructureInfoDTO orgStructureInfoDTO = new OrgStructureInfoDTO()
            {
                Key = orgUnit.Id,
                ManagerId = orgUnit.ManagerId,
                IsActive = orgUnit.IsActive,
                Number = orgUnit.Number,
                BarCode = orgUnit.BarCode,
                IsVirtualUnit = orgUnit.IsVirtualUnit,
                IsDeleted = orgUnit.IsDeleted,
                IsNew = false,
                HasChilds = orgUnit.HasChilds,
                TransactionsProcessingPeriod = orgUnit.TransactionsProcessingPeriod,
                Names = orgUnit.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(orgUnit.LocalizationIdentifier.Localizations) : null,
                Counter = CounterMapper.Map(orgUnit.Counter, cultureName),
                BarcodeDesigners = BarcodeMapper.Map(orgUnit.BarcodeDesigns),
                Lineage = orgUnit.Lineage,
                ExternalId = orgUnit.ExternalId,
                IoDepartment = orgUnit.IoDepartment,
                FollowUpDepartment = orgUnit.FollowUpDepartment,
                IsExecutive = orgUnit.IsExecutive,
                IsGeneralIoDepartment = orgUnit.IsGeneralIoDepartment,
                ReceiveElcOutBoundWithAcknowled = orgUnit.ReceiveWithAcknowled,
                SendSpecialCopy = orgUnit.SendSpecialCopy

            };

            if (orgUnit.LocalizationIdentifier != null)
            {
                orgStructureInfoDTO.IdentifierId = orgUnit.LocalizationIdentifier.Id;

                if (orgUnit.LocalizationIdentifier.Localizations != null)
                {
                    orgStructureInfoDTO.Name = orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText();
                }
            }

            if (orgUnit.Users != null)
            {
                orgStructureInfoDTO.Users = new List<OrgUnitUserDTO>();
                foreach (UserProfile userProfile in orgUnit.Users)
                {
                    OrgUnitUserDTO orgUnitUserDTO = new OrgUnitUserDTO()
                    {
                        Id = userProfile.Id,
                        IsActive = userProfile.IsActive,
                        Email = userProfile.Email,
                        PhoneNumber = userProfile.PhoneNumber,
                        LocalName = userProfile.LocalName,
                        RoleName = userProfile.UserGroups.Count != 0 ? userProfile.UserGroups[0].Group.GroupName.Localizations.Where(l => l.Culture.Id == 1).LocalText() : string.Empty,
                        ExternalId = userProfile.ExternalId,
                        // MainOrgUnitName = userProfile.MainOrgUnitId != 0 ? orgUnitBL.GetOrgUnitName(o => o.Id == userProfile.MainOrgUnitId, null) : string.Empty,

                    };

                    if (userProfile.LocalizationIdentifier != null && userProfile.LocalizationIdentifier.Localizations != null)
                    {
                        orgUnitUserDTO.UserName = userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText();
                    }
                    else
                    {
                        orgUnitUserDTO.UserName = userProfile.UserName;
                    }
                    orgStructureInfoDTO.Users.Add(orgUnitUserDTO);
                }
            }
            return orgStructureInfoDTO;
        }

        public static IList<OrgunitSap> Map(IList<OrgunitSapDto> orgunitSapDtos)
        {

            List<OrgunitSap> orgunitSaps = new List<OrgunitSap>();
            if (!(orgunitSapDtos != null && orgunitSapDtos.Count > 0))
            {
                return orgunitSaps;
            }
            orgunitSaps = orgunitSapDtos.Select(org => new OrgunitSap
            {
                Code = org.Code,
                SystemStatus = org.SystemStatus,
                NameAr = org.NameAr,
                ParentCode = org.ParentCode,
                NameEn = org.NameEn,


            }).ToList();

            return orgunitSaps;
        }



    }
}