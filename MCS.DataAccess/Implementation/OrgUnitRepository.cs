using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework.ObjectExtensions;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class OrgUnitRepository : BaseRepository<OrgUnit>, IOrgUnitRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public OrgUnitRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddOrgUnit(OrgUnit orgUnit)
        {
            try
            {
                _oMCSDbContext.OrgUnits.Add(orgUnit);

                _oMCSDbContext.SaveChanges();

                return orgUnit.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public OrgUnitLink GetOrgUnitLink(int orgUnitFromId, int orgUnitToId)
        {
            try
            {
                return _oMCSDbContext.OrgUnitLinks.Where(l => l.FromEntity.Id == orgUnitFromId && l.ToEntity.Id == orgUnitToId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateOrgUnit(OrgUnit orgUnit)
        {
            try
            {

                OrgUnit orgUnitOld = FindBy(o => o.Id == orgUnit.Id);

                if (orgUnitOld != null)
                {
                    if (orgUnit.Users != null)
                    {
                        IList<UserProfile> users = new List<UserProfile>();

                        foreach (UserProfile user in orgUnit.Users)
                        {
                            users.Add(user);
                        }

                        orgUnitOld.Users.ToList().ForEach(u => orgUnitOld.Users.Remove(u));

                        orgUnitOld.Users = users;
                    }

                    IList<BarcodeDesign> barcodeDesigns = new List<BarcodeDesign>();
                    //DatabaseNulls
                    if (orgUnit.BarcodeDesigns != null)
                    {
                        orgUnit.BarcodeDesigns.ToList().ForEach(b => barcodeDesigns.Add(b.ShallowCopy<BarcodeDesign>()));

                        orgUnitOld.BarcodeDesigns.ToList().ForEach(u => _oMCSDbContext.BarcodeDesigns.Remove(u));
                    }
                    orgUnitOld.BarcodeDesigns = barcodeDesigns;

                    _oMCSDbContext.Entry(orgUnitOld).CurrentValues.SetValues(orgUnit);

                    foreach (Localization localization in orgUnit.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = orgUnitOld.LocalizationIdentifier.Localizations
                                                                    .Where(l => l.Id == localization.Id)
                                                                    .FirstOrDefault();

                        if (currentlocalization != null)
                        {
                            _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                        }
                    }

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public OrgUnit GetOrgUnitById(int orgUnitId)
        {
            try
            {
                return FindBy(o => o.Id == orgUnitId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckOrgUnitHasAssignmentPaper(int orgUnitId)
        {
            try
            {
                AssignmentPaper assignmentPaper =
                    _oMCSDbContext.OrgUnits.Where(o => o.Id == orgUnitId).Select(o => o.AssignmentPaper).FirstOrDefault();

                return (assignmentPaper != null);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public BarcodeDesign GetBarcodeDesignByOrgUnitId(int orgUnitId, int typeId)
        {
            try
            {
                OrgUnit orgUnit = _oMCSDbContext.OrgUnits.Where(o => o.Id == orgUnitId).FirstOrDefault();

                if (orgUnit != null)
                {
                    return orgUnit.BarcodeDesigns.Where(b => b.TypeId == typeId).FirstOrDefault();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckOrgUnitIsAllowedToCreateGroup(int orgUnitId)
        {
            try
            {
                AssignmentPaper assignmentPaper =
                    _oMCSDbContext.OrgUnits.Where(o => o.Id == orgUnitId).Select(o => o.AssignmentPaper).FirstOrDefault();

                if (assignmentPaper != null)
                {
                    return assignmentPaper.IsCreateGroupAllowed;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public OrgUnit GetOrgUnitById(int orgUnitId, string cultureName)
        {
            try
            {
                IList<OrgUnit> orgUnits = (from orgUnit in _oMCSDbContext.OrgUnits
                                           where orgUnit.Id == orgUnitId &&
                                           !orgUnit.IsDeleted && orgUnit.IsActive
                                           select new
                                           {
                                               orgUnit.Id,
                                               orgUnit.Number,
                                               orgUnit.IsVirtualUnit,

                                               localizationIdentifierText = new
                                               {
                                                   orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                               },
                                               orgUnit.LocalizationIdentifier,

                                               //orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                               orgUnit.Parent,
                                               orgUnit.Lineage,
                                               Links = orgUnit.Links.Select(k => new
                                               {
                                                   FromEntityId = k.FromEntity.Id,
                                                   ToEntityId = k.ToEntity.Id,
                                                   FromEntityParent = k.FromEntity.Parent,
                                                   ToEntityParent = k.ToEntity.Parent,
                                                   FromEntityName = k.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                   FromEntityIsVirtualUnit = k.FromEntity.IsVirtualUnit,
                                                   ToEntityName = k.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                   ToEntityIsVirtualUnit = k.ToEntity.IsVirtualUnit
                                               })
                                           }).ToList().Select(o => new OrgUnit
                                           {
                                               Id = o.Id,
                                               Parent = o.Parent,
                                               Lineage = o.Lineage,
                                               Number = o.Number,
                                               IsVirtualUnit = o.IsVirtualUnit,
                                               //LocalName = o.Text,
                                               LocalizationIdentifier = o.LocalizationIdentifier,
                                               LocalName = o.localizationIdentifierText.Text,
                                               Links = o.Links.Select(k => new OrgUnitLink
                                               {
                                                   FromEntity = new OrgUnit
                                                   {
                                                       Id = k.FromEntityId,
                                                       Parent = k.FromEntityParent,
                                                       LocalName = k.FromEntityName,
                                                       IsVirtualUnit = k.FromEntityIsVirtualUnit
                                                   },

                                                   ToEntity = new OrgUnit
                                                   {
                                                       Id = k.ToEntityId,
                                                       Parent = k.ToEntityParent,
                                                       LocalName = k.ToEntityName,
                                                       IsVirtualUnit = k.ToEntityIsVirtualUnit
                                                   }
                                               }).ToList()
                                           }).ToList();

                return orgUnits.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<OrgUnit> GetOrgUnitStructure()
        {
            try
            {
                return _oMCSDbContext.OrgUnits.Where(a => a.IsDeleted == false && a.IsActive == true && a.IsVirtualUnit == false).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<OrgUnit> GetOrgUnits(string cultureName)
        {
            try
            {
                IList<OrgUnit> orgUnits = (from orgUnit in _oMCSDbContext.OrgUnits
                                           where orgUnit.IsDeleted == false
                                           && orgUnit.IsActive == true
                                           && orgUnit.IsVirtualUnit == false
                                           select new
                                           {
                                               orgUnit.Id,
                                               orgUnit.Parent,
                                               orgUnit.Number,
                                               orgUnit.Links,
                                               orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                               HasChilds = _oMCSDbContext.OrgUnits.Any(ou => ou.ParentId == orgUnit.Id & !ou.IsDeleted & ou.IsActive),
                                               orgUnit.ParentId,
                                               orgUnit.Lineage,
                                           }).ToList().Select(o => new OrgUnit
                                           {
                                               Id = o.Id,
                                               Parent = o.Parent,
                                               LocalName = o.Text,
                                               Number = o.Number,
                                               Links = o.Links,
                                               HasChilds = o.HasChilds,
                                               ParentId = o.ParentId,
                                               Lineage = o.Lineage,
                                           }).ToList();
                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<OrgUnit> GetOrgUnits(int? parentId, string cultureName, int? UserId)
        {
            try
            {
                IList<OrgUnit> orgUnits = _oMCSDbContext.OrgUnits
                                                        .Where(ou =>
                                                        !ou.IsDeleted &&
                                                        ou.IsActive &&
                                                        //!ou.IsVirtualUnit &&
                                                        (UserId != null || ou.ParentId == parentId) &&
                                                        (UserId == null || ou.Users.Any(u => u.Id == UserId.Value))
                                                        )
                                                        .Select(orgUnit => new
                                                        {
                                                            orgUnit.Id,
                                                            orgUnit.Parent,
                                                            orgUnit.Number,
                                                            orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                            HasChilds = _oMCSDbContext.OrgUnits.Any(ou => ou.ParentId == orgUnit.Id & !ou.IsDeleted & ou.IsActive),
                                                            IsVirtual = orgUnit.IsVirtualUnit
                                                        }).ToList().Select(ou => new OrgUnit
                                                        {
                                                            Id = ou.Id,
                                                            Parent = ou.Parent,
                                                            LocalName = ou.Text,
                                                            Number = ou.Number,
                                                            HasChilds = ou.HasChilds,
                                                            IsVirtualUnit = ou.IsVirtual
                                                        }).ToList();

                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<OrgUnit> GetOrgUnitsAutoComplete(string searchQuery, string cultureName, int resultSize, int orgUnitId, bool isAllModules, bool isParentModule, bool isAllChildsModules)
        {
            try
            {
                bool isNumeric = int.TryParse(searchQuery, out int n);
                IList<OrgUnit> orgUnits;
                var orgUnitParentId = _oMCSDbContext.OrgUnits
                                                    .Where(ou => ou.Id == orgUnitId)
                                                    .FirstOrDefault().ParentId;

                if (isNumeric)
                {
                    string numberToSearch = searchQuery;
                    orgUnits = _oMCSDbContext.OrgUnits
                                             .Where(ou => ou.Number == numberToSearch && !ou.IsDeleted && ou.IsActive && !ou.IsVirtualUnit)
                                             .Select(orgUnit => new
                                             {
                                                 orgUnit.Id,
                                                 orgUnit.Number,
                                                 orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                 orgUnit.ParentId
                                             }).ToList().Select(o => new OrgUnit
                                             {
                                                 Id = o.Id,
                                                 Number = o.Number,
                                                 LocalName = o.Text,
                                                 ParentId = o.ParentId
                                             }).ToList();
                }
                else
                {
                    orgUnits = _oMCSDbContext.OrgUnits
                                             .Where(ou => ou.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.Contains(searchQuery)
                                                    && !ou.IsDeleted && ou.IsActive && !ou.IsVirtualUnit)
                                             .Select(orgUnit => new
                                             {
                                                 orgUnit.Id,
                                                 orgUnit.Number,
                                                 orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                 orgUnit.ParentId
                                             }).Take(resultSize).ToList()
                                             .Select(p => new OrgUnit
                                             {
                                                 Id = p.Id,
                                                 Number = p.Number,
                                                 LocalName = p.Text,
                                                 ParentId = p.ParentId
                                             }).ToList();
                }

                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<UserProfile> GetUsersByOrgUnitId(int orgUnitId, string cultureName)
        {
            try
            {
                IList<UserProfile> users = (from UserProfile in _oMCSDbContext.UserProfiles
                                            where UserProfile.OrgUnits.Any(o => (o.Id == orgUnitId | orgUnitId == -1) && o.IsActive && !o.IsVirtualUnit && !o.IsDeleted)
                                            select new
                                            {
                                                UserProfile.Id,
                                                UserProfile.Email,
                                                UserProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                            }).ToList().Select(a => new UserProfile
                                            {
                                                Id = a.Id,
                                                LocalName = a.Text,
                                                Email = a.Email,
                                            }).ToList();
                return users;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public OrgUnit GetOrgUnit(int orgUnitId, string cultureName)
        {
            try
            {
                OrgUnit orgUnits = _oMCSDbContext.OrgUnits
                                                .Where(orgUnit => orgUnit.Id == orgUnitId &&
                                                !orgUnit.IsDeleted && orgUnit.IsActive)
                                                .Select(ou => new
                                                {
                                                    ou.Id,
                                                    ou.Number,
                                                    ou.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                    ou.ParentId,
                                                    ou.Parent,
                                                    ou.Lineage,
                                                    HasChilds = _oMCSDbContext.OrgUnits.Any(c => c.ParentId == ou.Id & !ou.IsDeleted & ou.IsActive),
                                                    IsVirtualUnit = ou.IsVirtualUnit
                                                }).ToList().Select(o => new OrgUnit
                                                {
                                                    Id = o.Id,
                                                    Number = o.Number,
                                                    LocalName = o.Text,
                                                    ParentId = o.ParentId,
                                                    Parent = o.Parent,
                                                    Lineage = o.Lineage,
                                                    HasChilds = o.HasChilds,
                                                    IsVirtualUnit = o.IsVirtualUnit
                                                }).FirstOrDefault();

                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public OrgUnit GetParentOrgUnit(int orgUnitId, string cultureName)
        {
            try
            {

                int? parentid = _oMCSDbContext.OrgUnits.Where(x => x.Id == orgUnitId).FirstOrDefault()?.ParentId;
                OrgUnit orgUnits = _oMCSDbContext.OrgUnits
                                                .Where(orgUnit => orgUnit.Id == parentid &&
                                                !orgUnit.IsDeleted && orgUnit.IsActive)
                                                .Select(ou => new
                                                {
                                                    ou.Id,
                                                    ou.Number,
                                                    ou.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                    ou.ParentId,
                                                    ou.Parent,
                                                    ou.Lineage,
                                                    HasChilds = _oMCSDbContext.OrgUnits.Any(c => c.ParentId == ou.Id & !ou.IsDeleted & ou.IsActive),
                                                    IsVirtualUnit = ou.IsVirtualUnit
                                                }).ToList().Select(o => new OrgUnit
                                                {
                                                    Id = o.Id,
                                                    Number = o.Number,
                                                    LocalName = o.Text,
                                                    ParentId = o.ParentId,
                                                    Parent = o.Parent,
                                                    Lineage = o.Lineage,
                                                    HasChilds = o.HasChilds,
                                                    IsVirtualUnit = o.IsVirtualUnit
                                                }).FirstOrDefault();

                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public OrgUnit GetInternalPartyInfoByNumber(string partyNumber, string cultureName)
        {
            try
            {
                string Number = partyNumber;
                OrgUnit orgUnits = _oMCSDbContext.OrgUnits
                                                .Where(orgUnit => orgUnit.Number == Number &&
                                                !orgUnit.IsDeleted && orgUnit.IsActive)
                                                .Select(ou => new
                                                {
                                                    ou.Id,
                                                    ou.Number,
                                                    ou.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                    IsVirtualUnit = ou.IsVirtualUnit
                                                }).ToList().Select(o => new OrgUnit
                                                {
                                                    Id = o.Id,
                                                    Number = o.Number,
                                                    LocalName = o.Text,
                                                    IsVirtualUnit = o.IsVirtualUnit
                                                }).FirstOrDefault();

                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<OrgUnit> GetOrgUnits(List<int> orgUnitIds, string cultureName)
        {
            try
            {
                List<OrgUnit> orgUnits = _oMCSDbContext.OrgUnits
                                                .Where(orgUnit => orgUnitIds.Contains(orgUnit.Id) &&
                                                !orgUnit.IsDeleted && orgUnit.IsActive)
                                                .Select(ou => new
                                                {
                                                    ou.Id,
                                                    ou.Number,
                                                    ou.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                }).ToList().Select(o => new OrgUnit
                                                {
                                                    Id = o.Id,
                                                    Number = o.Number,
                                                    LocalName = o.Text
                                                }).ToList();

                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public IList<OrgUnit> GetOrgUnitsLight(string cultureName)
        {
            try
            {
                IList<OrgUnit> orgUnits = (from orgUnit in _oMCSDbContext.OrgUnits
                                           where orgUnit.IsDeleted == false && orgUnit.IsActive == true
                                           && orgUnit.IsVirtualUnit == false
                                           select new
                                           {
                                               orgUnit.Id,
                                               orgUnit.Parent,
                                               localizationIdentifierText = new
                                               {
                                                   orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                               },
                                               orgUnit.LocalizationIdentifier,
                                               orgUnit.Number,
                                               orgUnit.BarCode,
                                               orgUnit.TransactionsProcessingPeriod,
                                               orgUnit.IsVirtualUnit,
                                               orgUnit.ManagerId,
                                               orgUnit.Lineage,
                                               orgUnit.ExternalId,
                                               orgUnit.IoDepartment,
                                               orgUnit.FollowUpDepartment,
                                               orgUnit.IsExecutive,
                                               orgUnit.IsGeneralIoDepartment,
                                               orgUnit.ReceiveWithAcknowled,
                                               orgUnit.SendSpecialCopy
                                           }).ToList().Select(o => new OrgUnit
                                           {
                                               Id = o.Id,
                                               Parent = o.Parent,
                                               LocalizationIdentifier = o.LocalizationIdentifier,
                                               LocalName = o.localizationIdentifierText.Text,
                                               Number = o.Number,
                                               BarCode = o.BarCode,
                                               TransactionsProcessingPeriod = o.TransactionsProcessingPeriod,
                                               IsVirtualUnit = o.IsVirtualUnit,
                                               ManagerId = o.ManagerId,
                                               Lineage = o.Lineage,
                                               ExternalId = o.ExternalId,
                                               IoDepartment = o.IoDepartment,
                                               FollowUpDepartment = o.FollowUpDepartment,
                                               IsExecutive = o.IsExecutive,
                                               IsGeneralIoDepartment = o.IsGeneralIoDepartment,
                                               ReceiveWithAcknowled = o.ReceiveWithAcknowled,
                                               SendSpecialCopy = o.SendSpecialCopy,
                                           }).ToList();

                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<OrgUnit> GetOrgUnitsNew(string cultureName)
        {
            try
            {
                IList<OrgUnit> orgUnits = (from orgUnit in _oMCSDbContext.OrgUnits
                                           where orgUnit.IsDeleted == false
                                           select new
                                           {
                                               orgUnit.Id,
                                               orgUnit.Parent,
                                               localizationIdentifierText = new
                                               {
                                                   orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                               },
                                               orgUnit.LocalizationIdentifier,
                                               orgUnit.Number,
                                               orgUnit.BarCode,
                                               orgUnit.TransactionsProcessingPeriod,
                                               orgUnit.IsVirtualUnit,
                                               orgUnit.ManagerId,
                                               orgUnit.Lineage,
                                               orgUnit.ExternalId,
                                               orgUnit.IoDepartment,
                                               orgUnit.FollowUpDepartment,
                                               orgUnit.IsExecutive,
                                               orgUnit.IsGeneralIoDepartment,
                                               orgUnit.ReceiveWithAcknowled,
                                               orgUnit.SendSpecialCopy
                                           }).ToList().Select(o => new OrgUnit
                                           {
                                               Id = o.Id,
                                               Parent = o.Parent,
                                               LocalizationIdentifier = o.LocalizationIdentifier,
                                               LocalName = o.localizationIdentifierText.Text,
                                               Number = o.Number,
                                               BarCode = o.BarCode,
                                               TransactionsProcessingPeriod = o.TransactionsProcessingPeriod,
                                               IsVirtualUnit = o.IsVirtualUnit,
                                               ManagerId = o.ManagerId,
                                               Lineage = o.Lineage,
                                               ExternalId = o.ExternalId,
                                               IoDepartment = o.IoDepartment,
                                               FollowUpDepartment = o.FollowUpDepartment,
                                               IsExecutive = o.IsExecutive,
                                               IsGeneralIoDepartment = o.IsGeneralIoDepartment,
                                               ReceiveWithAcknowled = o.ReceiveWithAcknowled,
                                               SendSpecialCopy = o.SendSpecialCopy,
                                           }).ToList();

                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<int> GetAllOrgUnitsId(string cultureName)
        {
            try
            {
                List<int> orgUnits = _oMCSDbContext.OrgUnits.Where
                                            (a => a.IsDeleted == false && a.IsActive == true && a.IsVirtualUnit == false).Select(a => a.Id).ToList();

                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<OrgUnit> GetOrgUnitsWithCounter(string cultureName)
        {
            try
            {
                IList<OrgUnit> orgUnits = (from orgUnit in _oMCSDbContext.OrgUnits
                                           where orgUnit.IsDeleted == false
                                           && orgUnit.IsActive == true
                                           && orgUnit.IsVirtualUnit == false
                                           && orgUnit.IsVirtualUnit == false
                                           select new
                                           {
                                               orgUnit.Id,
                                               orgUnit.Parent,
                                               localizationIdentifierText = new
                                               {
                                                   orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                               },
                                               orgUnit.LocalizationIdentifier,
                                               orgUnit.Number,
                                               orgUnit.BarCode,
                                               orgUnit.TransactionsProcessingPeriod,
                                               orgUnit.IsVirtualUnit,
                                               orgUnit.ManagerId,
                                               orgUnit.Counter
                                           }).ToList().Select(o => new OrgUnit
                                           {
                                               Id = o.Id,
                                               Parent = o.Parent,
                                               LocalizationIdentifier = o.LocalizationIdentifier,
                                               LocalName = o.localizationIdentifierText.Text,
                                               Number = o.Number,
                                               BarCode = o.BarCode,
                                               TransactionsProcessingPeriod = o.TransactionsProcessingPeriod,
                                               IsVirtualUnit = o.IsVirtualUnit,
                                               ManagerId = o.ManagerId,
                                               Counter = o.Counter
                                           }).ToList();

                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<OrgUnit> GetOrgUnitsWithUser(string cultureName)
        {
            try
            {
                IList<OrgUnit> orgUnits = (from orgUnit in _oMCSDbContext.OrgUnits
                                           where orgUnit.IsDeleted == false && orgUnit.IsActive == true && orgUnit.IsVirtualUnit == false
                                           && orgUnit.IsVirtualUnit == false
                                           select new
                                           {
                                               orgUnit.Id,
                                               orgUnit.Parent,
                                               localizationIdentifierText = new
                                               {
                                                   orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                               },
                                               orgUnit.LocalizationIdentifier,
                                               orgUnit.Number,
                                               orgUnit.BarCode,
                                               orgUnit.TransactionsProcessingPeriod,
                                               orgUnit.IsVirtualUnit,
                                               orgUnit.ManagerId,
                                               orgUnit.Users
                                           }).ToList().Select(o => new OrgUnit
                                           {
                                               Id = o.Id,
                                               Parent = o.Parent,
                                               LocalizationIdentifier = o.LocalizationIdentifier,
                                               LocalName = o.localizationIdentifierText.Text,
                                               Number = o.Number,
                                               BarCode = o.BarCode,
                                               TransactionsProcessingPeriod = o.TransactionsProcessingPeriod,
                                               IsVirtualUnit = o.IsVirtualUnit,
                                               ManagerId = o.ManagerId,
                                               Users = o.Users
                                           }).ToList();
                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<OrgUnit> GetOrgUnitsWithLinks(string cultureName)
        {
            try
            {
                IList<OrgUnit> orgUnits = (from orgUnit in _oMCSDbContext.OrgUnits
                                           where orgUnit.IsDeleted == false && orgUnit.IsActive == true && orgUnit.IsVirtualUnit == false
                                           select new
                                           {
                                               orgUnit.Id,
                                               orgUnit.Parent,
                                               localizationIdentifierText = new
                                               {
                                                   orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                               },
                                               orgUnit.LocalizationIdentifier,
                                               orgUnit.Number,
                                               orgUnit.BarCode,
                                               orgUnit.TransactionsProcessingPeriod,
                                               orgUnit.IsVirtualUnit,
                                               orgUnit.ManagerId,
                                               orgUnit.Links
                                           }).ToList().Select(o => new OrgUnit
                                           {
                                               Id = o.Id,
                                               Parent = o.Parent,
                                               LocalizationIdentifier = o.LocalizationIdentifier,
                                               LocalName = o.localizationIdentifierText.Text,
                                               Number = o.Number,
                                               BarCode = o.BarCode,
                                               TransactionsProcessingPeriod = o.TransactionsProcessingPeriod,
                                               IsVirtualUnit = o.IsVirtualUnit,
                                               ManagerId = o.ManagerId,
                                               Links = o.Links
                                           }).ToList();
                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteOrgUnitLinks(int orgUnitId)
        {
            try
            {
                OrgUnit orgUnit =
                    _oMCSDbContext.OrgUnits.Where(o => o.Id == orgUnitId).Where(o => !o.IsDeleted).FirstOrDefault();

                orgUnit.Links.ToList().ForEach(item => _oMCSDbContext.OrgUnitLinks.Remove(item));

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public string GetOrgUnitName(Expression<Func<OrgUnit, bool>> @where, string cultureName)
        {
            try
            {
                OrgUnit orgUnit =
                    _oMCSDbContext.OrgUnits.Where(@where).Where(o => !o.IsDeleted).FirstOrDefault();

                if (orgUnit != null)
                {
                    return orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public string GetOrgUnitSymbol(int OrgUnitId)
        {
            try
            {
                string OrgUnitSymbol =
                    _oMCSDbContext.OrgUnits.Where(x => x.Id == OrgUnitId).FirstOrDefault().Lineage;

                return OrgUnitSymbol;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<AssignmentPaperBeneficiary> GetOrgUnitBeneficiaries(int orgUnitId, string cultureName)
        {
            try
            {
                IList<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries = new List<AssignmentPaperBeneficiary>();

                AssignmentPaper assignmentPaper =
                    _oMCSDbContext.OrgUnits.Where(o => o.Id == orgUnitId).Select(o => o.AssignmentPaper).FirstOrDefault();

                if (assignmentPaper != null)
                {
                    return assignmentPaper.AssignmentPaperBeneficiaries
                                          .Select(a => new
                                          {
                                              orgUnitId = a.OrgUnit.Id,
                                              orgUnitName = a.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                              userId = a.User?.Id,
                                              userName = a.User?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                              userObject = a.User ?? null,
                                              AssignmentGroup = a.AssignmentPaperGroup
                                          }).ToList().Select(assignment => new AssignmentPaperBeneficiary
                                          {
                                              OrgUnit = new OrgUnit
                                              {
                                                  Id = assignment.orgUnitId,
                                                  LocalName = assignment.orgUnitName
                                              },
                                              User = (assignment.userObject != null) ? new UserProfile
                                              {
                                                  Id = assignment.userId.Value,
                                                  LocalName = assignment.userName
                                              } : null,
                                              AssignmentPaperGroup = (assignment.AssignmentGroup != null) ? new AssignmentPaperGroup
                                              {
                                                  Id = assignment.AssignmentGroup.Id,
                                                  OrderNo = assignment.AssignmentGroup.OrderNo,
                                                  DefaultActionId = assignment.AssignmentGroup.DefaultActionId,
                                              } : null


                                          }).ToList();
                }

                return assignmentPaperBeneficiaries;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Domain.Action> GetOrgUnitActions(int orgUnitId, string cultureName)
        {
            try
            {
                IList<Domain.Action> actions = new List<Domain.Action>();

                AssignmentPaper assignmentPaper =
                    _oMCSDbContext.OrgUnits.Where(o => o.Id == orgUnitId).Select(o => o.AssignmentPaper).FirstOrDefault();

                if (assignmentPaper != null)
                {
                    assignmentPaper.AssignmentPaperActions.ToList().ForEach(a =>
                    {
                        actions.Add(a.Action = new Domain.Action
                        {
                            Id = a.Action.Id,
                            LocalName = a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                            Type = a.Action.Type
                        });
                    });
                }

                return actions.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<UserProfile> GetOrgUnitsManagers(string cultureName)
        {
            try
            {
                IList<UserProfile> userProfiles = new List<UserProfile>();

                userProfiles = _oMCSDbContext.OrgUnits.Where(o => !o.IsDeleted)
                    .Select(o => o.Users.Where(u => u.Id == o.ManagerId)
                        .Select(u => new
                        {
                            u.Id,
                            u.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                        })
                        .FirstOrDefault()).ToList().Where(up => up != null).Select(u => new UserProfile
                        {
                            Id = u.Id,
                            LocalName = u.Text
                        }).ToList();

                return userProfiles;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public UserProfile GetOrgUnitManager(int orgUnitId, string cultureName)
        {
            try
            {
                UserProfile userProfiles = new UserProfile();

                userProfiles = _oMCSDbContext.OrgUnits.Where(o => !o.IsDeleted && o.Id == orgUnitId)
                    .Select(o => o.Users.Where(u => u.Id == o.ManagerId)
                        .Select(u => new
                        {
                            u.Id,
                            u.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                        })
                        .FirstOrDefault()).ToList().Where(up => up != null).Select(u => new UserProfile
                        {
                            Id = u.Id,
                            LocalName = u.Text
                        }).FirstOrDefault();

                return userProfiles;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public bool ValidateManagerCanAssign(int orgUnitId, int managerId, int transactionId, int transactionUserId, bool isManager)
        {
            try
            {
                bool isValid = _oMCSDbContext.OrgUnits
                                                      .Any(o => !o.IsDeleted
                                                                && o.Id == orgUnitId
                                                                && _oMCSDbContext.TransactionAssignments.Any(tr => tr.TransactionId == transactionId && tr.ToEntityId == orgUnitId));
                return isValid && isManager;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Domain.Action> GetOrgUnitActions(int orgUnitId)
        {
            try
            {
                AssignmentPaper assignmentPaper =
                    _oMCSDbContext.OrgUnits.Where(o => o.Id == orgUnitId).Select(o => o.AssignmentPaper).FirstOrDefault();

                if (assignmentPaper != null && assignmentPaper.AssignmentPaperActions != null)
                {
                    return assignmentPaper.AssignmentPaperActions.Select(a => a.Action).ToList();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public AssignmentPaper GetAssignmentPaperByOrgUnitId(int orgUnitId, string cultureName)
        {
            try
            {
                AssignmentPaper selectedAssignmentPaper = null;

                IQueryable<AssignmentPaper> assignmentPaper = (from assPaper in _oMCSDbContext.AssignmentPapers
                                                               join orgUnit in _oMCSDbContext.OrgUnits on
                                                               assPaper.Id equals orgUnit.AssignmentPaper.Id
                                                               where orgUnit.Id == orgUnitId
                                                               select assPaper);

                selectedAssignmentPaper = assignmentPaper.FirstOrDefault();

                if (selectedAssignmentPaper != null)
                {
                    selectedAssignmentPaper.AssignmentPaperActions.ToList().ForEach(ap => ap.Action.LocalName =
                        ap.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                   );

                    selectedAssignmentPaper.AssignmentPaperBeneficiaries.ToList().ForEach(pb =>
                    {
                        pb.OrgUnit.LocalName = pb.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;

                        if (pb.User != null)
                        {
                            pb.User.LocalName = pb.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                        }
                    });
                }

                return selectedAssignmentPaper;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateAssignmentPaper(AssignmentPaper assignmentPaper)
        {
            try
            {
                AssignmentPaper assignmentPaperOld =
                    _oMCSDbContext.AssignmentPapers.Where(a => a.Id == assignmentPaper.Id).FirstOrDefault();

                if (assignmentPaperOld != null)
                {
                    assignmentPaperOld.AssignmentPaperActions.ToList().ForEach(a => _oMCSDbContext.AssignmentPaperActions.Remove(a));
                    assignmentPaperOld.AssignmentPaperBeneficiaries.ToList().ForEach(a => _oMCSDbContext.AssignmentPaperBeneficies.Remove(a));

                    assignmentPaperOld.AssignmentPaperActions = assignmentPaper.AssignmentPaperActions;
                    assignmentPaperOld.AssignmentPaperBeneficiaries = assignmentPaper.AssignmentPaperBeneficiaries;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public AssignmentPaper GetAssignmentPaperById(int assignmentPaperId)
        {
            try
            {
                return _oMCSDbContext.AssignmentPapers.Where(a => a.Id == assignmentPaperId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public AssignmentPaperAction GetAssignmentPaperActionById(int AssignmentPaperActionId)
        {
            try
            {
                return _oMCSDbContext.AssignmentPaperActions.Where(a => a.Id == AssignmentPaperActionId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public AssignmentPaperBeneficiary GetAssignmentPaperBeneficiaryById(int assignmentPaperBeneficiaryId)
        {
            try
            {
                return _oMCSDbContext.AssignmentPaperBeneficies.Where(b => b.Id == assignmentPaperBeneficiaryId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Transaction GetYearTransactionsCount(Expression<Func<Transaction, bool>> @where)
        {
            try
            {
                return _oMCSDbContext.Transactions.Where(@where).OrderByDescending(t => t.Number).First();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public DateTime GetFirstTransactionDate()
        {
            try
            {
                return _oMCSDbContext.Transactions.OrderBy(t => t.CreatedOn).First().CreatedOn;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int GetAllOrgUnitsCount()
        {
            try
            {
                return _oMCSDbContext.OrgUnits.Count();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateOrgUnitWithUsers(OrgUnit orgUnit, string cultureName)
        {
            try
            {

                OrgUnit orgUnitOld = FindBy(o => o.Id == orgUnit.Id && o.IsActive == true && o.IsDeleted == false);
                if (orgUnitOld != null && orgUnit.Users != null)
                {
                    IList<UserProfile> users = new List<UserProfile>();

                    foreach (UserProfile user in orgUnit.Users)
                    {
                        users.Add(user);
                    }

                    orgUnitOld.Users.ToList().ForEach(u => orgUnitOld.Users.Remove(u));

                    orgUnitOld.Users = users;
                }
                else
                {
                    _oMCSDbContext.OrgUnits.Add(orgUnit);
                }
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateOrgUnitWithCounter(OrgUnit orgUnit, string cultureName)
        {
            try
            {
                OrgUnit orgUnitOld = _oMCSDbContext.OrgUnits.FirstOrDefault(o => o.Id == orgUnit.Id && o.IsActive == true && o.IsDeleted == false);
                if (orgUnitOld != null && orgUnit.Counter != null)
                {
                    if (orgUnit.Counter.IsGeneral)
                    {
                        orgUnitOld.BarCode = BarCode(orgUnitOld.Number, orgUnitOld.ParentId);
                    }
                    if (orgUnit.Counter.Id == 0)
                    {
                        orgUnitOld.Counter = new Counter() { ResetByYear = true, OwnerEntityId = orgUnit.Counter.OwnerEntityId };
                        orgUnitOld.Counter.CounterDetails = new List<CounterDetail>();
                    }
                    orgUnitOld.Counter.Id = orgUnit.Counter.Id;
                    orgUnitOld.Counter.Year = orgUnit.Counter.Year;
                    orgUnitOld.Counter.IsGeneral = orgUnit.Counter.IsGeneral;
                    orgUnitOld.Counter.Description = orgUnit.Counter.Description;
                    var newCounterDetails = orgUnit.Counter.CounterDetails.FirstOrDefault(a => a.Id == 0);
                    if (newCounterDetails != null)
                    {
                        orgUnitOld.Counter.CounterDetails.Add(newCounterDetails);
                    }
                    else
                    {
                        if (orgUnitOld.Counter.CounterDetails != null && orgUnitOld.Counter.CounterDetails.Count > 0 &&
                            orgUnit.Counter.CounterDetails != null && orgUnit.Counter.CounterDetails.Count > 0)
                        {
                            foreach (var item in orgUnitOld.Counter.CounterDetails)
                            {
                                var result = orgUnit.Counter.CounterDetails.FirstOrDefault(a => a.Id == item.Id);
                                if (result != null)
                                {
                                    item.InitialValue = result.InitialValue;
                                    item.Count = result.Count;
                                    item.TransactionCategories = result.TransactionCategories;
                                }
                            }
                        }
                    }
                }
                else
                {
                    _oMCSDbContext.OrgUnits.Add(orgUnit);
                }
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int UpdateOrgUnitInfo(OrgUnit orgUnit)
        {
            try
            {
                int Id = 0;
                OrgUnit orgUnitOld = FindBy(o => o.Id == orgUnit.Id && o.IsActive == true && o.IsDeleted == false);

                if (orgUnitOld != null)//Update
                {
                    orgUnitOld.Lineage = orgUnit.Lineage;
                    orgUnit.ParentId = orgUnit.ParentId == -1 ? null : orgUnit.ParentId;
                    if (orgUnitOld.Number != orgUnit.Number)
                    {
                        UpdateAllOrgUnitChild(orgUnit);
                    }
                    _oMCSDbContext.Entry(orgUnitOld).CurrentValues.SetValues(orgUnit);
                    foreach (Localization localization in orgUnit.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = orgUnitOld.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Id == localization.Id);
                        if (currentlocalization != null)
                        {
                            _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                            _oMCSDbContext.Entry(currentlocalization).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    _oMCSDbContext.SaveChanges();
                    Id = orgUnitOld.Id;
                }
                else//Add
                {
                    //  orgUnit.Number = GetLastNumber();
                    orgUnit.ParentId = orgUnit.ParentId == -1 ? null : orgUnit.ParentId;
                    _oMCSDbContext.OrgUnits.Add(orgUnit);

                    _oMCSDbContext.SaveChanges();

                    int AddedOrgUnitId = orgUnit.Id;
                    Id = AddedOrgUnitId;
                    string Parentlineage = string.Empty;
                    OrgUnit ParentOrgUnit = FindBy(o => o.Id == orgUnit.ParentId && o.IsActive == true && o.IsDeleted == false);
                    if (ParentOrgUnit != null)
                    {
                        Parentlineage = ParentOrgUnit.Lineage;
                    }
                    orgUnit.Lineage = orgUnit.Lineage;
                    //orgUnit.Lineage = Parentlineage + AddedOrgUnitId + "/";
                    _oMCSDbContext.SaveChanges();
                }
                return Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateOrgUnitWithLink(OrgUnit orgUnit, string cultureName)
        {
            try
            {
                OrgUnit orgUnitOld = FindBy(o => o.Id == orgUnit.Id && o.IsActive == true && o.IsDeleted == false);
                if (orgUnitOld != null)
                {
                    IList<OrgUnitLink> OrgUnitLinks = new List<OrgUnitLink>();
                    if (orgUnit.Links != null && orgUnit.Links.Count > 0)
                    {
                        foreach (var link in orgUnit.Links)
                        {
                            OrgUnitLinks.Add(link);
                        }
                    }
                    orgUnitOld.Links.ToList().ForEach(u => orgUnitOld.Links.Remove(u));
                    orgUnitOld.Links = OrgUnitLinks;
                }
                else
                {
                    _oMCSDbContext.OrgUnits.Add(orgUnit);
                }
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateOrgUnitWithBarcodeDesign(OrgUnit orgUnit, string cultureName)
        {
            try
            {
                OrgUnit orgUnitOld = FindBy(o => o.Id == orgUnit.Id && o.IsActive == true && o.IsDeleted == false);
                if (orgUnitOld != null)
                {
                    IList<BarcodeDesign> barcodeDesigns = new List<BarcodeDesign>();
                    if (orgUnit.BarcodeDesigns != null)
                    {
                        orgUnit.BarcodeDesigns.ToList().ForEach(b => barcodeDesigns.Add(b.ShallowCopy()));

                        orgUnitOld.BarcodeDesigns.ToList().ForEach(u => _oMCSDbContext.BarcodeDesigns.Remove(u));
                    }
                    orgUnitOld.BarcodeDesigns = barcodeDesigns;
                }
                else
                {
                    _oMCSDbContext.OrgUnits.Add(orgUnit);
                }
                _oMCSDbContext.SaveChanges();
                CacheHelper.Remove(CachedObjectsKey.OrgUnits, cultureName);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool DeleteOrgUnit(int orgUnitKey)
        {
            try
            {
                OrgUnit orgUnitOld = FindBy(o => o.Id == orgUnitKey && o.IsActive == true && o.IsDeleted == false);
                if (orgUnitOld != null)
                {
                    orgUnitOld.IsDeleted = true;
                }
                _oMCSDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<OrgUnit> GetOrgUnitStructureRoot(int? parentId)
        {
            try
            {
                IQueryable<OrgUnit> orgUnits = from orgUnit in _oMCSDbContext.OrgUnits
                                               where orgUnit.IsDeleted == false &&
                                               orgUnit.IsActive == true &&
                                               orgUnit.ParentId == parentId
                                               select orgUnit;

                var result = orgUnits.ToList().Select(p => new OrgUnit
                {
                    Id = p.Id,
                    Parent = p.Parent,
                    LocalizationIdentifier = p.LocalizationIdentifier,
                    Number = p.Number,
                    BarCode = p.BarCode,
                    TransactionsProcessingPeriod = p.TransactionsProcessingPeriod,
                    IsVirtualUnit = p.IsVirtualUnit,
                    ManagerId = p.ManagerId,
                    ExternalId = p.ExternalId,
                    HasChilds = _oMCSDbContext.OrgUnits.Count(e => e.ParentId == p.Id && e.IsDeleted == false && e.IsActive == true) > 0
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private string BarCode(string number, int? parentId)
        {
            OrgUnit orgUnit = new OrgUnit { ParentId = parentId, Number = number };
            while (orgUnit.ParentId != null)
            {
                orgUnit = _oMCSDbContext.OrgUnits.FirstOrDefault(a => a.Id == orgUnit.ParentId);
            }
            return $"{number}/{orgUnit.Number}";
        }

        private void UpdateAllOrgUnitChild(OrgUnit orgUnit)
        {
            try
            {

                _oMCSDbContext.OrgUnits.Where(o => o.IsActive && !o.IsDeleted && o.ParentId == orgUnit.Id).ToList()
                    .ForEach(child => child.BarCode = child.Number + "/" + orgUnit.Number);
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion Methods

        #region MobileApi
        public IList<OrgUnit> UserMobileGetOrgHierarchy(int? parentId, string cultureName)
        {
            IList<OrgUnit> orgUnits = null;
            orgUnits = _oMCSDbContext.OrgUnits
                                     .Where(ou => ou.ParentId == parentId && !ou.IsDeleted && ou.IsActive && !ou.IsVirtualUnit)
                                     .Select(orgUnit => new
                                     {
                                         orgUnit.Id,
                                         Name = orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         orgUnit.ParentId,
                                         orgUnit.Users,
                                         orgUnit.IsVirtualUnit,
                                         orgUnit.Number,
                                         HasChilds = _oMCSDbContext.OrgUnits.Any(ou => ou.ParentId == orgUnit.Id & !ou.IsDeleted & ou.IsActive)
                                     }).ToList().Select(o => new OrgUnit
                                     {
                                         Id = o.Id,
                                         LocalName = o.Name,
                                         ParentId = o.ParentId,
                                         Users = o.Users,
                                         IsVirtualUnit = o.IsVirtualUnit,
                                         Number = o.Number,
                                         HasChilds = o.HasChilds
                                     }).ToList();

            return orgUnits;
        }

        public void UpdateOrgUnitToJoinGeneralCounter(int orgUnitId)
        {
            try
            {
                OrgUnit orgUnitOld = _oMCSDbContext.OrgUnits.FirstOrDefault(o => o.Id == orgUnitId && o.IsActive == true && o.IsDeleted == false);
                var counter = _oMCSDbContext.Counters.Where(c => c.IsGeneral == true).FirstOrDefault();
                if (counter.Id != orgUnitOld.Id)
                {
                    orgUnitOld.Counter = counter;
                    _oMCSDbContext.SaveChanges();

                    var oldCounter = _oMCSDbContext.Counters.FirstOrDefault(a => a.OwnerEntityId == orgUnitId);
                    if (oldCounter != null)
                    {
                        var oldCounterDetail = _oMCSDbContext.CounterDetails.Where(a => a.Counter.Id == oldCounter.Id).ToList();
                        if (oldCounterDetail != null)
                        {
                            _oMCSDbContext.CounterDetails.RemoveRange(oldCounterDetail);
                        }
                        _oMCSDbContext.Counters.Remove(oldCounter);
                        _oMCSDbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<OrgUnit> UserMobileGetOrgHierarchyAC(string searchQuery, string cultureName, int resultSize)
        {
            try
            {
                bool isNumeric = int.TryParse(searchQuery, out int n);

                IList<OrgUnit> orgUnits;

                if (isNumeric)
                {
                    string numberToSearch = searchQuery;
                    orgUnits = _oMCSDbContext.OrgUnits
                                             .Where(ou => ou.Number == numberToSearch)
                                             .Select(orgUnit => new
                                             {
                                                 orgUnit.Id,
                                                 orgUnit.Number,
                                                 Name = orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                 orgUnit.ParentId,
                                                 orgUnit.Users
                                             }).ToList().Select(o => new OrgUnit
                                             {
                                                 Id = o.Id,
                                                 Number = o.Number,
                                                 LocalName = o.Name,
                                                 ParentId = o.ParentId,
                                                 Users = o.Users
                                             }).ToList();
                }
                else
                {
                    orgUnits = _oMCSDbContext.OrgUnits
                                             .Where(ou => ou.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.Contains(searchQuery))
                                             .Select(orgUnit => new
                                             {
                                                 orgUnit.Id,
                                                 orgUnit.Number,
                                                 Name = orgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                 orgUnit.ParentId,
                                                 orgUnit.Users
                                             }).Take(resultSize).ToList()
                                             .Select(p => new OrgUnit
                                             {
                                                 Id = p.Id,
                                                 Number = p.Number,
                                                 LocalName = p.Name,
                                                 ParentId = p.ParentId,
                                                 Users = p.Users
                                             }).ToList();
                }

                return orgUnits;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion


        public string GetLastNumber()
        {
            try
            {
                int number;
                OrgUnit obj = _oMCSDbContext.OrgUnits.OrderByDescending(u => u.Number).FirstOrDefault();
                number = Convert.ToInt32(obj.Number) + 1;
                return number.ToString();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckOrgUnitNumber(string Number, int OrgUnitKey)
        {
            try
            {
                OrgUnit orgUnit = _oMCSDbContext.OrgUnits.Where(e => e.Number == Number && (OrgUnitKey == -1 || e.Id != OrgUnitKey)).FirstOrDefault();

                if (orgUnit == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int? getIoDepartment(int orgunitID)
        {
            try
            {
                return _oMCSDbContext.OrgUnits.Where(e => e.Id == orgunitID).FirstOrDefault()?.IoDepartment;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int? getGeneralIoDepartment()
        {
            try
            {
                return _oMCSDbContext.OrgUnits.Where(e => e.IsGeneralIoDepartment == true).FirstOrDefault()?.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int? getFollowUpDepartment(int orgunitID)
        {
            try
            {
                return _oMCSDbContext.OrgUnits.Where(e => e.Id == orgunitID).FirstOrDefault()?.FollowUpDepartment;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool ReceiveElcOutBoundWithAcknowled(int orgunitID)
        {
            try
            {
                bool receiveWithAcknowled = _oMCSDbContext.OrgUnits.Where(e => e.Id == orgunitID).FirstOrDefault().ReceiveWithAcknowled;
                return receiveWithAcknowled;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public bool CheckIfOrgunitSendSpecialCopy(int orgunitID)
        {
            try
            {
                return _oMCSDbContext.OrgUnits.Where(e => e.Id == orgunitID).FirstOrDefault().SendSpecialCopy;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetAllUsers()
        {
            try
            {
                IList<UserProfile> userProfiles = new List<UserProfile>();

                userProfiles = _oMCSDbContext.UserProfiles.Select(u => new
                {
                    u.Id,
                    u.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == "ar").FirstOrDefault().Text
                }).ToList().Where(up => up != null).Select(u => new UserProfile
                {
                    Id = u.Id,
                    LocalName = u.Text
                }).ToList();

                return userProfiles;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateOrgFromService(IList<OrgunitSap> orgunitSaps)
        {
            try
            {
                int currentOrgUnitId = 0;
                int mainParentId = _oMCSDbContext.OrgUnits.Where(org => !org.ParentId.HasValue).SingleOrDefault().Id;
                var allorgWithoutParent = orgunitSaps.Where(x => string.IsNullOrWhiteSpace(x.ParentCode)).ToList();
                var allorgWithParent = orgunitSaps.Where(x => !string.IsNullOrWhiteSpace(x.ParentCode)).OrderBy(x => x.ParentCode).ToList();
                //var currentOrgUnitHierarchiesCode = orgUnitHierarchies.Select(x => x.ExternalCode).ToList();
                var allOrgunitSaps = _oMCSDbContext.OrgunitSaps.ToList();
                var neworgUnitHierarchies = orgunitSaps.Where(org => !allOrgunitSaps.Any(old => old.Code == org.Code)).ToList();
                var counter = _oMCSDbContext.Counters.FirstOrDefault();
                
                foreach (var orgunitSap in allorgWithoutParent)
                {


                    var oldOrgUnit = _oMCSDbContext.OrgUnits.Where(org => org.Number == orgunitSap.Code).FirstOrDefault();

                    bool isActive = orgunitSap.SystemStatus.ToLower() == "a";
                    if (oldOrgUnit != null)
                    {

                        var localizations = oldOrgUnit.LocalizationIdentifier.Localizations;
                        var localizationAr = localizations.Where(x => x.CultureId == 1).FirstOrDefault();
                        var localizationEn = localizations.Where(x => x.CultureId == 2).FirstOrDefault();
                        localizationAr.Text = orgunitSap.NameAr;
                        localizationEn.Text = orgunitSap.NameEn;
                        oldOrgUnit.IsDeleted = false;
                        oldOrgUnit.IsVirtualUnit = !isActive;
                        oldOrgUnit.IsActive = true;
                        oldOrgUnit.ParentId = mainParentId;
                        _oMCSDbContext.Entry(oldOrgUnit).State = System.Data.Entity.EntityState.Modified;
                        _oMCSDbContext.SaveChanges();
                        currentOrgUnitId = oldOrgUnit.Id;

                    }
                    else
                    {

                        OrgUnit orgUnit = new OrgUnit
                        {
                            LocalizationIdentifier = new LocalizationIdentifier
                            {
                                Localizations = new Localization[] { new Localization {CultureId= 1,Text= orgunitSap.NameAr},
                                    new Localization { Text= orgunitSap.NameEn,CultureId=2} },
                            },
                            IsNew = true,
                            ManagerId = 0,
                            IsActive = true,
                            Number = orgunitSap.Code,
                            IsVirtualUnit = !isActive,
                            TransactionsProcessingPeriod = 0,
                            JoinToGeneralCounter = false,
                            Counter = counter,
                            SendSpecialCopy = false,
                            IsExecutive = false,
                            IsGeneralIoDepartment = false,
                            ReceiveWithAcknowled = false,
                            ParentId = mainParentId,



                        };
                        _oMCSDbContext.OrgUnits.Add(orgUnit);
                        currentOrgUnitId = orgUnit.Id;
                        _oMCSDbContext.SaveChanges();
                    }

                    _oMCSDbContext.SaveChanges();
                }



                foreach (var orgunitSap in allorgWithParent)
                {
                    var oldOrgUnit = _oMCSDbContext.OrgUnits.Where(org => org.Number == orgunitSap.Code).FirstOrDefault();
                    int parentId = _oMCSDbContext.OrgUnits.Where(org => org.Number == orgunitSap.ParentCode).FirstOrDefault().Id;
                    bool isActive = orgunitSap.SystemStatus.ToLower() == "a";

                    if (oldOrgUnit != null)
                    {

                        var localizations = oldOrgUnit.LocalizationIdentifier.Localizations;
                        var localizationAr = localizations.Where(x => x.CultureId == 1).FirstOrDefault();
                        var localizationEn = localizations.Where(x => x.CultureId == 2).FirstOrDefault();
                        localizationAr.Text = orgunitSap.NameAr;
                        localizationEn.Text = orgunitSap.NameEn;
                        oldOrgUnit.IsDeleted = false;
                        oldOrgUnit.IsActive = true;
                        oldOrgUnit.IsVirtualUnit = !isActive;
                        oldOrgUnit.ParentId = parentId;
                        _oMCSDbContext.Entry(oldOrgUnit).State = System.Data.Entity.EntityState.Modified;
                        _oMCSDbContext.SaveChanges();
                        currentOrgUnitId = oldOrgUnit.Id;

                    }
                    else
                    {

                        OrgUnit orgUnit = new OrgUnit
                        {
                            LocalizationIdentifier = new LocalizationIdentifier
                            {
                                Localizations = new Localization[] { new Localization {CultureId= 1,Text= orgunitSap.NameAr },
                                    new Localization { Text= orgunitSap.NameEn,CultureId=2} },
                            },
                            IsNew = true,
                            ManagerId = 0,
                            IsActive = true,
                            Number = orgunitSap.Code,
                            IsVirtualUnit = !isActive,
                            TransactionsProcessingPeriod = 0,
                            JoinToGeneralCounter = false,
                            Counter = counter,
                            SendSpecialCopy = false,
                            IsExecutive = false,
                            IsGeneralIoDepartment = false,
                            ReceiveWithAcknowled = false,
                            ParentId = parentId,



                        };
                        _oMCSDbContext.OrgUnits.Add(orgUnit);
                        currentOrgUnitId = orgUnit.Id;
                        _oMCSDbContext.SaveChanges();
                    }


                    _oMCSDbContext.SaveChanges();
                }
                if (neworgUnitHierarchies != null && neworgUnitHierarchies.Count > 0)
                {
                    _oMCSDbContext.OrgunitSaps.AddRange(neworgUnitHierarchies);
                }
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }




    }
}
