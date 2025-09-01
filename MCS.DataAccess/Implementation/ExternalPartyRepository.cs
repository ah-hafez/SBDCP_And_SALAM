using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using System.Data.Entity;

namespace MCS.DataAccess
{
    public class ExternalPartyRepository : BaseRepository<ExternalParty>, IExternalPartyRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public ExternalPartyRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddExternalParty(ExternalParty externalParty)
        {
            try
            {
                if(externalParty.ParentId==0)
                {
                    externalParty.ParentId = null;
                }
                _oMCSDbContext.ExternalParties.Add(externalParty);

                _oMCSDbContext.SaveChanges();

                int AddedExternalPartyId = externalParty.Id;
                string Parentlineage = string.Empty;
                ExternalParty ParentExternalParty = FindBy(o => o.Id == externalParty.ParentId);
                if (ParentExternalParty != null)
                {
                    Parentlineage = ParentExternalParty.Lineage;
                }

                externalParty.Lineage = Parentlineage + AddedExternalPartyId + "/";
                _oMCSDbContext.SaveChanges();

                return externalParty.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public string GetLastNumber(int ParentId)
        {
            try
            {
               
                string maxNumber = _oMCSDbContext.ExternalParties
                    .Where (u => u.ParentId== ParentId)
                    .OrderByDescending(u => new { u.Number.Length,  u.Number })
                    .Select(u=> u.Number).FirstOrDefault();
                if (maxNumber == null)
                {
                    maxNumber = "0";
                }
                return  maxNumber;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public string GetLastNumberByCustomizeValue(string numberStartWithCustomizeValue)
        {
            try
            {

                string maxNumber = _oMCSDbContext.ExternalParties
                    .Where(u => u.Number.StartsWith(numberStartWithCustomizeValue))
                    .OrderByDescending(u => new { u.Number.Length, u.Number })
                    .Select(u => u.Number).FirstOrDefault();
                
                if (maxNumber == null)
                {
                    maxNumber = "0";
                }
                return maxNumber;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateExternalParty(ExternalParty externalParty)
        {
            try
            {
                ExternalParty externalPartyOld = FindBy(p => p.Id == externalParty.Id);

                if (externalPartyOld != null)
                {
                    externalParty.Lineage = externalPartyOld.Lineage;
                    externalParty.ParentId = externalPartyOld.ParentId;
                    //externalPartyOld.PartyType = externalParty.PartyType;
                    _oMCSDbContext.Entry(externalPartyOld).CurrentValues.SetValues(externalParty);

                    foreach (Localization name in externalParty.Name.Localizations)
                    {
                        Localization currentlocalization = externalPartyOld.Name.Localizations
                                                                        .Where(l => l.Id == name.Id)
                                                                        .FirstOrDefault();

                        if (currentlocalization != null)
                        {
                            _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(name);
                        }
                    }

                    if (externalParty.Address != null)
                    {
                        foreach (Localization address in externalParty.Address.Localizations)
                        {
                            Localization currentlocalization = externalPartyOld.Address.Localizations
                                                                               .Where(l => l.Id == address.Id)
                                                                               .FirstOrDefault();

                            if (currentlocalization != null)
                            {
                                _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(address);
                            }
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

        public void DeleteParty(int id)
        {
            try
            {
                ExternalParty externalParty = _oMCSDbContext.ExternalParties.Where(p => p.Id == id).FirstOrDefault();

                if (externalParty != null)
                {
                    RemoveChilds(externalParty);

                    foreach (ExternalPartyManager partyManager in externalParty.PartyManagers.ToList())
                    {
                        _oMCSDbContext.Entry(partyManager.Name).State = System.Data.Entity.EntityState.Deleted;
                    }

                    _oMCSDbContext.Entry(externalParty.Name).State = System.Data.Entity.EntityState.Deleted;
                    _oMCSDbContext.Entry(externalParty.Address).State = System.Data.Entity.EntityState.Deleted;
                    _oMCSDbContext.ExternalParties.Remove(externalParty);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public ExternalParty GetExternalPartyById(int externalPartyId)
        {
            try
            {
                return _oMCSDbContext.ExternalParties.Where(e => e.Id == externalPartyId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public ExternalParty GetExternalPartyInfoByNumber(string partyNumber)
        {
            try
            {
                return _oMCSDbContext.ExternalParties.Where(e => e.Number == partyNumber).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        

        public bool CheckPartyNumber(string Number, int partyId = -1)
        {
            try
            {
                ExternalParty externalParty = _oMCSDbContext.ExternalParties
                                                            .Where(e => e.Number == Number &&
                                                                   (partyId == -1 || e.Id != partyId))
                                                            .FirstOrDefault();

                if (externalParty == null)
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
        public IList<ExternalParty> GetAllExternalParties(int? parentId, string cultureName)
        {
            try
            {
                IList<ExternalParty> externalParties = _oMCSDbContext.ExternalParties
                                                                     .Select(externalParty => new
                                                                     {
                                                                         externalParty.Id,
                                                                         externalParty.Number,
                                                                         externalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                                         externalParty.PartyType,
                                                                         externalParty.ParentId,
                                                                         IsYesserRegistered = externalParty.YasserRegistered,
                                                                         HasChilds = _oMCSDbContext.ExternalParties.Any(ex => ex.ParentId == externalParty.Id)
                                                                     }).ToList().Select(p => new ExternalParty
                                                                     {
                                                                         Id = p.Id,
                                                                         Number = p.Number,
                                                                         LocalName = p.Text,
                                                                         PartyType = p.PartyType,
                                                                         ParentId = p.ParentId,
                                                                         HasChilds = p.HasChilds,
                                                                         YasserRegistered = p.IsYesserRegistered
                                                                     }).ToList();

                return externalParties;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<ExternalParty> GetExternalParties(int? parentId, string cultureName, bool getVirtual = false)
        {
            try
            {
                IList<ExternalParty> externalParties = _oMCSDbContext.ExternalParties
                                                                     .Where(p => p.ParentId == parentId && (getVirtual || (!getVirtual && !p.IsVirtual)))
                                                                     .Select(externalParty => new
                                                                     {
                                                                         externalParty.IsActive,
                                                                         externalParty.Id,
                                                                         externalParty.Number,
                                                                         externalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                                         externalParty.PartyType,
                                                                         externalParty.ParentId,
                                                                         IsYesserRegistered = externalParty.YasserRegistered,
                                                                         Email = externalParty.Email,
                                                                         HasChilds = _oMCSDbContext.ExternalParties.Any(ex => ex.ParentId == externalParty.Id),
                                                                         IsVirtual = externalParty.IsVirtual
                                                                     }).ToList().Select(p => new ExternalParty
                                                                     {
                                                                         Id = p.Id,
                                                                         Number = p.Number,
                                                                         LocalName = p.Text,
                                                                         PartyType = p.PartyType,
                                                                         ParentId = p.ParentId,
                                                                         HasChilds = p.HasChilds,
                                                                         IsActive = p.IsActive,
                                                                         YasserRegistered = p.IsYesserRegistered,
                                                                         Email=p.Email,
                                                                         IsVirtual = p.IsVirtual
                                                                     }).ToList();

                return externalParties;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<ExternalParty> GetExternalPartiesAutoComplete(string searchQuery, string cultureName, int resultSize)
        {
            try
            {
                bool isNumeric = int.TryParse(searchQuery, out int n);

                IList<ExternalParty> externalParties;

                if (isNumeric)
                {
                    externalParties = _oMCSDbContext.ExternalParties
                                                    .Where(ex => ex.Number == searchQuery && !ex.IsVirtual)
                                                    .Select(externalParty => new
                                                    {
                                                        externalParty.Id,
                                                        externalParty.Number,
                                                        externalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                        externalParty.ParentId
                                                    }).ToList().Select(p => new ExternalParty
                                                    {
                                                        Id = p.Id,
                                                        Number = p.Number,
                                                        LocalName = p.Text,
                                                        ParentId = p.ParentId
                                                    }).ToList();
                }
                else
                {
                    externalParties = _oMCSDbContext.ExternalParties
                                                    .Where(ex => ex.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.Contains(searchQuery)
                                                                        && !ex.IsVirtual)
                                                    .Select(externalParty => new
                                                    {
                                                        externalParty.Id,
                                                        externalParty.Number,
                                                        externalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                        externalParty.ParentId
                                                    }).Take(resultSize).ToList().Select(p => new ExternalParty
                                                    {
                                                        Id = p.Id,
                                                        Number = p.Number,
                                                        LocalName = p.Text,
                                                        ParentId = p.ParentId
                                                    }).ToList();
                }

                return externalParties;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public ExternalParty GetExternalPartiesByNumber(string Number)
        {
            try
            {
                ExternalParty retrivedExternalParty;
                retrivedExternalParty = _oMCSDbContext.ExternalParties
                                                    .Where(ex => ex.Number == Number)
                                                    .Select(externalParty => new
                                                    {
                                                        externalParty.Id,
                                                        externalParty.Number,
                                                        externalParty.Name,
                                                        externalParty.ParentId
                                                    }).ToList().Select(p => new ExternalParty
                                                    {
                                                        Id = p.Id,
                                                        Number = p.Number,
                                                        Name = p.Name,
                                                        ParentId = p.ParentId
                                                    }).FirstOrDefault();
                return retrivedExternalParty;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<ExternalParty> GetExternalPartyNodes(int? nodeId, string cultureName)
        {
            try
            {
                IList<ExternalParty> externalParties = _oMCSDbContext.ExternalParties
                                                        .Select(externalParty => new
                                                        {
                                                            externalParty.Id,
                                                            externalParty.Number,
                                                            externalParty.Email,
                                                            externalParty.PhoneNumber,
                                                            externalParty.Fax,
                                                            externalParty.IsVirtual,
                                                            externalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                            Address = externalParty.Address.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                            externalParty.PartyType,
                                                            externalParty.ParentId,
                                                            externalParty.YasserRegistered,
                                                            PartyManagers = externalParty.PartyManagers.Select(m =>
                                                                new
                                                                {
                                                                    m.Id,
                                                                    m.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text

                                                                }
                                                            )
                                                        }
                             ).ToList().Select(p => new ExternalParty
                             {
                                 Id = p.Id,
                                 Number = p.Number,
                                 Email = p.Email,
                                 PhoneNumber = p.PhoneNumber,
                                 Fax = p.Fax,
                                 IsVirtual = p.IsVirtual,
                                 LocalName = p.Text,
                                 LocalAddress = (p.Address != null) ? p.Address : string.Empty,
                                 PartyType = p.PartyType,
                                 ParentId = p.ParentId,
                                 HasChilds = _oMCSDbContext.ExternalParties.Count(e => e.ParentId == p.Id) > 0,
                                 YasserRegistered = p.YasserRegistered,
                                 PartyManagers = p.PartyManagers.Select(m => new ExternalPartyManager
                                 {
                                     Id = m.Id,
                                     LocalName = m.Text
                                 }).ToList()
                             }).Where(p => p.ParentId == nodeId).ToList();

                return externalParties;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<ExternalParty> GetExternalPartiesByLetterId(LetterListType letterType, int? parentId, string cultureName)
        {
            try
            {
                IList<ExternalParty> externalParties = (from externalParty in _oMCSDbContext.ExternalParties
                                                        where externalParty.ParentId == parentId
                                                        select new
                                                        {
                                                            externalParty.Id,
                                                            externalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                            externalParty.ParentId,
                                                            HasChilds = _oMCSDbContext.ExternalParties.Count(e => e.ParentId == externalParty.Id) > 0,
                                                            externalParty.YasserRegistered
                                                        }).ToList().Select(p => new ExternalParty
                                                        {
                                                            Id = p.Id,
                                                            HasChilds = p.HasChilds,
                                                            ParentId = p.ParentId,
                                                            YasserRegistered = p.YasserRegistered,
                                                            LocalName = p.Text

                                                        }).ToList();
                return externalParties;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<ExternalParty> GetExternalPartiesByParentId(int parentId)
        {
            try
            {
                IQueryable<ExternalParty> externalParties = (from externalParty in _oMCSDbContext.ExternalParties
                                                             where externalParty.ParentId == parentId
                                                             select externalParty);

                return externalParties.ToList().Select(p => new ExternalParty
                {
                    Id = p.Id,
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<ExternalParty> GetExternalPartiesByParentId(int? parentId, string cultureName)
        {
            try
            {

                IList<ExternalParty> externalParties = (from externalParty in _oMCSDbContext.ExternalParties
                                                        where externalParty.ParentId == parentId
                                                        select new
                                                        {
                                                            externalParty.Id,
                                                            externalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                            externalParty.ParentId,
                                                            externalParty.Number,
                                                            externalParty.YasserRegistered,
                                                            HasChilds = _oMCSDbContext.ExternalParties.Count(e => e.ParentId == externalParty.Id) > 0,
                                                        }).ToList().Select(p => new ExternalParty
                                                        {
                                                            Id = p.Id,
                                                            HasChilds = p.HasChilds,
                                                            ParentId = p.ParentId,
                                                            LocalName = p.Text,
                                                            Number = p.Number,
                                                            YasserRegistered = p.YasserRegistered
                                                        }).ToList();
                return externalParties;
            }

            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<ExternalParty> GetExternalParties(SearchCriteria searchCriteria)
        {
            try
            {
                IQueryable<ExternalParty> externalParties = (from externalParty in _oMCSDbContext.ExternalParties
                                                             select externalParty);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(ExternalParty).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            externalParties = SortByText(externalParties, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "ParentId")
                        {
                            int? parentId = null;
                            if (filter.Value != null)
                            {
                                parentId = Convert.ToInt32(filter.Value);
                            }

                            externalParties = externalParties.Where(e => e.ParentId == parentId);

                        }
                        else
                        {
                            externalParties = WhereQuery(externalParties, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                return externalParties.Select(p => new
                {
                    p.Id,
                    p.ParentId,
                    p.Name,
                    HasChilds = _oMCSDbContext.ExternalParties.Count(e => e.ParentId == p.Id) > 0,
                    p.YasserRegistered,
                    Parent = p.Parent != null ? new
                    {
                        p.Id,
                        p.Name,
                        HasChilds = _oMCSDbContext.ExternalParties.Count(e => e.ParentId == p.Id) > 0,
                        p.ParentId,
                    } : null,

                }).ToList().Select(p => new ExternalParty
                {
                    Id = p.Id,
                    ParentId = p.ParentId,
                    LocalName = p.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    HasChilds = p.HasChilds,
                    YasserRegistered = p.YasserRegistered,
                    Parent = p.Parent != null ? new ExternalParty
                    {
                        Id = p.Id,
                        LocalName = p.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                        HasChilds = p.HasChilds,
                        ParentId = p.ParentId,
                    } : null,

                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int AddManager(ExternalPartyManager externalPartyManager)
        {
            try
            {
                _oMCSDbContext.ExternalPartyManagers.Add(externalPartyManager);

                _oMCSDbContext.SaveChanges();

                return externalPartyManager.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateManager(ExternalPartyManager externalPartyManager)
        {
            try
            {
                ExternalPartyManager externalPartyManagerOld =
                    _oMCSDbContext.ExternalPartyManagers.Where(m => m.Id == externalPartyManager.Id).FirstOrDefault();

                if (externalPartyManagerOld != null)
                {
                    _oMCSDbContext.Entry(externalPartyManagerOld).CurrentValues.SetValues(externalPartyManager);

                    externalPartyManagerOld.ExternalParty = externalPartyManager.ExternalParty;

                    foreach (Localization name in externalPartyManager.Name.Localizations)
                    {
                        Localization currentlocalization = externalPartyManagerOld.Name.Localizations
                                                                     .Where(l => l.Id == name.Id)
                                                                     .FirstOrDefault();

                        if (currentlocalization != null)
                        {
                            _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(name);
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

        public void DeleteManager(int id)
        {
            try
            {
                ExternalPartyManager externalPartyManager = _oMCSDbContext.ExternalPartyManagers.Where(p => p.Id == id).FirstOrDefault();
                if (externalPartyManager != null)
                {
                    _oMCSDbContext.ExternalPartyManagers.Remove(externalPartyManager);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public ExternalPartyManager GetExternalPartyManagerById(int externalPartyManagerId)
        {
            try
            {
                return _oMCSDbContext.ExternalPartyManagers.Where(m => m.Id == externalPartyManagerId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<ExternalPartyManager> GetExternalPartyManagers(int externalPartyId, SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<ExternalPartyManager> externalPartyManagers = (from manager in _oMCSDbContext.ExternalPartyManagers
                                                                          where manager.ExternalParty.Id == externalPartyId
                                                                          select manager);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(ExternalPartyManager).GetProperty(filter.ColumnName).PropertyType))
                        {
                            externalPartyManagers = SortByManagerText(externalPartyManagers, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "Id")
                        {
                            int id = Convert.ToInt32(filter.Value);

                            switch (filter.Type)
                            {
                                case FilterType.Equals:
                                    externalPartyManagers = externalPartyManagers.Where(e => e.Id == id);
                                    break;
                                case FilterType.GreaterThan:
                                    externalPartyManagers = externalPartyManagers.Where(e => e.Id > id);
                                    break;
                                case FilterType.LessThan:
                                    externalPartyManagers = externalPartyManagers.Where(e => e.Id < id);
                                    break;
                                case FilterType.GreaterThanOrEquals:
                                    externalPartyManagers = externalPartyManagers.Where(e => e.Id >= id);
                                    break;
                                case FilterType.LessThanOrEquals:
                                    externalPartyManagers = externalPartyManagers.Where(e => e.Id <= id);
                                    break;
                            }

                        }
                    }
                }

                rowsCount = externalPartyManagers.Count();

                if (searchCriteria.Ascending)
                {
                    externalPartyManagers = externalPartyManagers.OrderBy(p => p.Name.Localizations
                                   .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text)
                                   .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                   .Take(searchCriteria.PageSize);
                }
                else
                {
                    externalPartyManagers = externalPartyManagers.OrderByDescending(p => p.Name.Localizations
                                   .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text)
                                   .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                   .Take(searchCriteria.PageSize);
                }

                return externalPartyManagers.ToList().Select(a => new ExternalPartyManager
                {
                    Id = a.Id,
                    LocalName = a.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    EmailAddress = a.EmailAddress
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<ExternalPartyManager> GetAllExternalPartyManagers(int externalPartyId, string cultureName)
        {
            try
            {
                IList<ExternalPartyManager> externalPartyManagers = (from externalPartyManager in _oMCSDbContext.ExternalPartyManagers
                                                                     where externalPartyManager.ExternalParty.Id == externalPartyId
                                                                     select new
                                                                     {
                                                                         externalPartyManager.Id,
                                                                         externalPartyManager.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text

                                                                     }
                                                                     ).ToList().Select(m => new ExternalPartyManager
                                                                     {
                                                                         Id = m.Id,
                                                                         LocalName = m.Text
                                                                     }).ToList();

                return externalPartyManagers;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<ExternalPartyManager> SortByManagerText(IQueryable<ExternalPartyManager> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from manager in _oMCSDbContext.ExternalPartyManagers.Where(p => p.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue))
                            select manager);
                case FilterType.EndsWidth:
                    return (from manager in _oMCSDbContext.ExternalPartyManagers.Where(p => p.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue))
                            select manager);
                case FilterType.StartsWith:
                    return (from manager in _oMCSDbContext.ExternalPartyManagers.Where(p => p.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue))
                            select manager);
                case FilterType.Equals:
                    return (from manager in _oMCSDbContext.ExternalPartyManagers.Where(p => p.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue))
                            select manager);
            }

            return source;
        }

        private IQueryable<ExternalParty> SortByText(IQueryable<ExternalParty> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from externalParty in source.Where(p => p.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue))
                            select externalParty);
                case FilterType.EndsWidth:
                    return (from externalParty in source.Where(p => p.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue))
                            select externalParty);
                case FilterType.StartsWith:
                    return (from externalParty in source.Where(p => p.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue))
                            select externalParty);
                case FilterType.Equals:
                    return (from externalParty in source.Where(p => p.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue))
                            select externalParty);
            }

            return source;
        }

        private void RemoveChilds(ExternalParty externalParty)
        {
            try
            {

                IList<ExternalParty> childs = _oMCSDbContext.ExternalParties.Where(p => p.Parent.Id == externalParty.Id).ToList();

                if (childs.Count == 0)
                {
                    return;
                }

                foreach (ExternalParty child in childs)
                {
                    RemoveChilds(child);

                    foreach (ExternalPartyManager partyManager in child.PartyManagers.ToList())
                    {
                        _oMCSDbContext.Entry(partyManager.Name).State = System.Data.Entity.EntityState.Deleted;
                    }

                    _oMCSDbContext.Entry(child.Name).State = System.Data.Entity.EntityState.Deleted;
                    _oMCSDbContext.ExternalParties.Remove(child);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region MobileApi

        public IList<ExternalParty> UserMobileGetExternalParties(int? parentId, string cultureName)
        {
            IList<ExternalParty> externalParties = _oMCSDbContext.ExternalParties
                                                          .Where(p => p.ParentId == parentId)
                                                          .Select(externalParty => new
                                                          {
                                                              externalParty.IsActive,
                                                              externalParty.Id,
                                                              externalParty.Number,
                                                              Name = externalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                              externalParty.PartyType,
                                                              externalParty.ParentId,
                                                              HasChilds = _oMCSDbContext.ExternalParties.Any(ex => ex.ParentId == externalParty.Id)
                                                          }).ToList().Select(p => new ExternalParty
                                                          {
                                                              Id = p.Id,
                                                              Number = p.Number,
                                                              LocalName = p.Name,
                                                              PartyType = p.PartyType,
                                                              ParentId = p.ParentId,
                                                              HasChilds = p.HasChilds,
                                                              IsActive = p.IsActive,
                                                          }).ToList();

            return externalParties;
        }

        public IList<ExternalParty> UserMobileGetExternalPartiesAC(string searchQuery, string cultureName, int resultSize)
        {
            try
            {
                bool isNumeric = int.TryParse(searchQuery, out int n);

                IList<ExternalParty> externalParties;

                if (isNumeric)
                {
                    externalParties = _oMCSDbContext.ExternalParties
                                                    .Where(ex => ex.Number == searchQuery)
                                                    .Select(externalParty => new
                                                    {
                                                        externalParty.Id,
                                                        externalParty.Number,
                                                        Name = externalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                        externalParty.ParentId,
                                                        externalParty.PartyManagers
                                                    }).ToList().Select(p => new ExternalParty
                                                    {
                                                        Id = p.Id,
                                                        Number = p.Number,
                                                        LocalName = p.Name,
                                                        ParentId = p.ParentId,
                                                        PartyManagers = p.PartyManagers
                                                    }).ToList();
                }
                else
                {
                    externalParties = _oMCSDbContext.ExternalParties
                                                    .Where(ex => ex.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.Contains(searchQuery))
                                                    .Select(externalParty => new
                                                    {
                                                        externalParty.Id,
                                                        externalParty.Number,
                                                        Name = externalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                        externalParty.ParentId,
                                                        externalParty.PartyManagers,
                                                    }).Take(resultSize).ToList().Select(p => new ExternalParty
                                                    {
                                                        Id = p.Id,
                                                        Number = p.Number,
                                                        LocalName = p.Name,
                                                        ParentId = p.ParentId,
                                                        PartyManagers = p.PartyManagers
                                                    }).ToList();
                }

                return externalParties;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion

        #endregion
    }
}