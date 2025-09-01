using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class UserPreferenceRepository : BaseRepository<UserPreference>, IUserPreferenceRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public UserPreferenceRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public void AddUserPreference(UserPreference userPreference)
        {
            try
            {
                userPreference.ThemeId = _oMCSDbContext.Themes.OrderByDescending(x => x.Id).FirstOrDefault().Id;

                _oMCSDbContext.UserPreference.Add(userPreference);

                _oMCSDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateUserPreference(UserPreference userPreference)
        {
            try
            {
                UserPreference UserPreferenceOld = GetUserPreference(userPreference.Id);

                if (UserPreferenceOld != null)
                {
                    if (string.IsNullOrEmpty(userPreference.SignaturePasswordText) == false)
                    {
                        UserPreferenceOld.SignaturePasswordText = userPreference.SignaturePasswordText;
                    }
                    userPreference.NotificationSubscriptions = userPreference.NotificationSubscriptions;
                    userPreference.PhoneNumber = UserPreferenceOld.PhoneNumber;
                    _oMCSDbContext.Entry(UserPreferenceOld).CurrentValues.SetValues(userPreference);

                    UserPreferenceOld.UserTrayPreferences.ToList().ForEach(p =>
                      UserPreferenceOld.UserTrayPreferences.Remove(p));

                    if (userPreference.UserDelegations != null && userPreference.UserDelegations.Count > 0)
                    {
                        foreach (var item in userPreference.UserDelegations)
                        {
                            UserPreferenceOld.UserDelegations.Add(item);
                        }
                    }

                    UserPreferenceOld.UserTrayPreferences = userPreference.UserTrayPreferences;
                    UserPreferenceOld.AssignmentPaper = userPreference.AssignmentPaper;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void AddUserDelegation(UserDelegation userDelegation, int userId)
        {
            try
            {
                if (userDelegation != null)
                {
                    UserPreference userPreference = _oMCSDbContext.UserPreference.Where(a => a.UserProfileId == userId).FirstOrDefault();
                    userDelegation.UserPreferenceId = userPreference.Id;
                    userDelegation.StatusId = DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, string.Empty);
                    userPreference.IsDelegationEnabled = true;

                    _oMCSDbContext.UserDelegations.Add(userDelegation);
                    _oMCSDbContext.SaveChanges();

                    _oMCSDbContext.Entry(userPreference).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateUserDelegations(int userId, IList<UserDelegation> userDelegations)
        {
            try
            {
                UserPreference userPreference = _oMCSDbContext.UserPreference.Where(a => a.UserProfileId == userId).FirstOrDefault();

                if (userPreference != null && userDelegations != null && userDelegations.Count > 0)
                {
                    userPreference.IsDelegationEnabled = true;
                    _oMCSDbContext.Entry(userPreference).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                }

                //Add all delegation if there is no delegation added before
                if (userPreference != null && (userPreference.UserDelegations == null || userPreference.UserDelegations.Count == 0))
                {
                    foreach (var item in userDelegations)
                    {
                        item.UserPreferenceId = userPreference.Id;
                        item.StatusId = DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, string.Empty);
                        _oMCSDbContext.UserDelegations.Add(item);
                    }
                    _oMCSDbContext.SaveChanges();
                    return;
                }

                //There are delegations added before
                if (userPreference != null && userPreference.UserDelegations != null && userPreference.UserDelegations.Count > 0)
                {
                    foreach (var item in userDelegations)
                    {
                        var originalDelegation = userPreference.UserDelegations
                                                                .Where(c => c.Id == item.Id && c.Id != 0)
                                                                .SingleOrDefault();
                        //Updated Item
                        if (originalDelegation != null)
                        {
                            item.UserPreferenceId = userPreference.Id;
                            var delegationEntry = _oMCSDbContext.Entry(originalDelegation);
                            delegationEntry.CurrentValues.SetValues(item);
                        }
                        //Added item
                        else
                        {
                            item.UserPreferenceId = userPreference.Id;
                            item.StatusId = DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, string.Empty);
                            _oMCSDbContext.UserDelegations.Add(item);
                        }
                    }
                    foreach (var originalDelegation in userPreference.UserDelegations.Where(c => c.Id != 0).ToList())
                    {
                        if (!userDelegations.Any(c => c.Id == originalDelegation.Id))
                        {
                            originalDelegation.StatusId = DelegationStatus.Disabled.LookupIdentity(LookupCategory.DelegationStatus, string.Empty);
                            //_oMCSDbContext.SaveChanges();
                        }
                    }
                }
                _oMCSDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateUserDelegation(UserDelegation userDelegation)
        {
            try
            {
                UserDelegation UserPreferenceOld = _oMCSDbContext.UserDelegations.Where(d => d.Id == userDelegation.Id).FirstOrDefault();

                _oMCSDbContext.Entry(UserPreferenceOld).CurrentValues.SetValues(userDelegation);

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteDelegation(int id)
        {
            try
            {
                UserDelegation userdelegation = _oMCSDbContext.UserDelegations.Where(u => u.Id == id).FirstOrDefault();

                if (userdelegation != null)
                {
                    //_oMCSDbContext.UserDelegations.Remove(userdelegation);
                    userdelegation.StatusId = DelegationStatus.Disabled.LookupIdentity(LookupCategory.DelegationStatus, string.Empty);
                }

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public AssignmentPaper GetAssignmentPaperByUserId(int userId, string cultureName)
        {
            try
            {
                AssignmentPaper selectedAssignmentPaper = _oMCSDbContext.UserPreference.Include(x => x.AssignmentPaper.AssignmentPaperBeneficiaries)
                    .Include("AssignmentPaper.AssignmentPaperBeneficiaries.AssignmentPaperGroup").FirstOrDefault(up => up.UserProfileId == userId).AssignmentPaper;

                if (selectedAssignmentPaper != null)
                {
                    selectedAssignmentPaper.AssignmentPaperActions.ToList().ForEach(ap => ap.Action.LocalName =
                        ap.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                   );

                    selectedAssignmentPaper.AssignmentPaperBeneficiaries.ToList().ForEach(pb =>
                    {
                        pb.OrgUnit.LocalName = pb.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText();

                        if (pb.User != null)
                        {
                            pb.User.LocalName = pb.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText();
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

        public void AddAssignmentPaper(AssignmentPaper assignmentPaper, int userId)
        {
            try
            {
                if (assignmentPaper != null)
                {
                    _oMCSDbContext.AssignmentPapers.Add(assignmentPaper);
                    _oMCSDbContext.SaveChanges();

                    UserPreference userPreference = _oMCSDbContext.UserPreference.Where(a => a.UserProfileId == userId).FirstOrDefault();
                    userPreference.AssignmentPaperId = assignmentPaper.Id;
                    _oMCSDbContext.Entry(userPreference).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void AddAssignmentPaperBeneficiary(List<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries)
        {
            try
            {

                if (assignmentPaperBeneficiaries != null && assignmentPaperBeneficiaries.Count > 0)
                {
                    var firstOne = assignmentPaperBeneficiaries.FirstOrDefault();

                    int? userId = _oMCSDbContext.AssignmentPaperGroups.Where(x => x.Id == firstOne.AssignmentPaperGroupId)?.FirstOrDefault()?.UserId;
                    int? assignmentPaperId = null;
                    if (userId.HasValue)
                    {
                        assignmentPaperId = _oMCSDbContext.UserPreference.Where(x => x.UserProfileId == userId.Value).FirstOrDefault()?.AssignmentPaperId;
                        assignmentPaperBeneficiaries.ForEach(x => x.AssignmentPaperId = assignmentPaperId);
                    }


                    _oMCSDbContext.AssignmentPaperBeneficies.AddRange(assignmentPaperBeneficiaries);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateAssignmentPaperBeneficiary(List<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries)
        {
            try
            {
                if (assignmentPaperBeneficiaries != null && assignmentPaperBeneficiaries.Count > 0)
                {
                    var firstOne = assignmentPaperBeneficiaries.FirstOrDefault();
                    int? userId = _oMCSDbContext.AssignmentPaperGroups.Where(x => x.Id == firstOne.AssignmentPaperGroupId)?.FirstOrDefault()?.UserId;
                    int? assignmentPaperId = null;
                    if (userId.HasValue)
                    {
                        assignmentPaperId = _oMCSDbContext.UserPreference.Where(x => x.UserProfileId == userId.Value).FirstOrDefault()?.AssignmentPaperId;
                        assignmentPaperBeneficiaries.ForEach(x => x.AssignmentPaperId = assignmentPaperId);
                    }
                    _oMCSDbContext.AssignmentPaperBeneficies.AddRange(assignmentPaperBeneficiaries);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateAssignmentPaper(AssignmentPaper assignmentPaper, int userId)
        {
            try
            {
                AssignmentPaper oldAssignmentPaper = GetAssignmentPaperByUserId(userId, "ar");
                if (oldAssignmentPaper != null)
                {
                    DeleteAssignmentPaper(oldAssignmentPaper);
                    AddAssignmentPaper(assignmentPaper, userId);
                }
                else
                {
                    AddAssignmentPaper(assignmentPaper, userId);
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateGroupAssignmentPaper(List<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries, int groupId)
        {
            try
            {
                
                List<AssignmentPaperBeneficiary> OldassignmentPaperBeneficiaries = GetBeneficiaryByAssignmentPaperGroupId(groupId);
                if (OldassignmentPaperBeneficiaries != null)
                {
                    DeleteAssignmentPaperBeneficiary(OldassignmentPaperBeneficiaries);
                    AddAssignmentPaperBeneficiary(assignmentPaperBeneficiaries);
                }
                else
                {
                    AddAssignmentPaperBeneficiary(assignmentPaperBeneficiaries);
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateGroupAssignmentPaper(List<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries)
        {
            try
            {
                
                List<AssignmentPaperBeneficiary> OldassignmentPaperBeneficiaries = GetBeneficiaryByAssignmentPapers();
                if (OldassignmentPaperBeneficiaries != null)
                {
                    DeleteAssignmentPaperBeneficiary(OldassignmentPaperBeneficiaries);
                    UpdateAssignmentPaperBeneficiary(assignmentPaperBeneficiaries);
                }
                else
                {
                    UpdateAssignmentPaperBeneficiary(assignmentPaperBeneficiaries);
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void DeleteAssignmentPaper(AssignmentPaper assignmentPaper)
        {
            try
            {
                if (assignmentPaper != null)
                {
                    _oMCSDbContext.UserPreference.Where(a => a.AssignmentPaperId == assignmentPaper.Id).FirstOrDefault().AssignmentPaperId = null;
                    _oMCSDbContext.AssignmentPapers.Remove(assignmentPaper);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void DeleteAssignmentPaperBeneficiary(List<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries)
        {
            try
            {
                if (assignmentPaperBeneficiaries != null)
                {

                    _oMCSDbContext.AssignmentPaperBeneficies.RemoveRange(assignmentPaperBeneficiaries);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public UserPreference GetUserPreferenceByUserId(int userId, string cultureName, int? orgUnitId = null)
        {
            try
            {
                int Disabled = DelegationStatus.Disabled.LookupIdentity(LookupCategory.DelegationStatus, cultureName);
                int Approved = DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, cultureName);
                int InProcess = DelegationStatus.InProcess.LookupIdentity(LookupCategory.DelegationStatus, cultureName);
                int Rejected = DelegationStatus.Rejected.LookupIdentity(LookupCategory.DelegationStatus, cultureName);

                UserPreference userPreference = _oMCSDbContext.UserPreference.Include(r => r.UserPreferenceFollowups).Include("UserDelegations.Status").Include("UserDelegations.UserProfile").FirstOrDefault(p => p.UserProfileId == userId);
                if (userPreference != null)
                {
                    UserPreference userPreferenceObject = new UserPreference
                    {
                        Id = userPreference.Id,
                        ThemeId = userPreference.ThemeId,
                        SMSNotifications = userPreference.SMSNotifications,
                        Signature = userPreference.Signature,
                        SignatureBehalf = userPreference.SignatureBehalf,
                        SignatureCommand = userPreference.SignatureCommand,
                        SignaturePassword = userPreference.SignaturePassword,
                        MessageSignatureDoc = userPreference.MessageSignatureDoc,
                        SealSignatureDoc = userPreference.SealSignatureDoc,
                        IsDelegationEnabled = userPreference.IsDelegationEnabled,
                        NotificationSubscriptions = userPreference.NotificationSubscriptions,
                        MarkingDoc = userPreference.MarkingDoc,
                        FollowUpOrgId = userPreference.UserPreferenceFollowups.Where(f => f.OrgUnitId == orgUnitId).Select(r => r.FollowUpOrgId).FirstOrDefault(),
                        FollowUpUserId = userPreference.UserPreferenceFollowups.Where(f => f.OrgUnitId == orgUnitId).Select(r => r.FollowUpUserId).FirstOrDefault(),
                        HasSignaturePasswordText = !string.IsNullOrEmpty(userPreference.SignaturePasswordText),
                        DefaultDisplay = userPreference.DefaultDisplay,
                        DefaultAssignmentPaper = userPreference.DefaultAssignmentPaper,
                        UserTrayPreferences = userPreference.UserTrayPreferences.Select(u => new UserTrayPreference
                        {
                            TrayId = u.TrayId,
                            Tray = new Tray { Id = u.Tray.Id, LocalName = u.Tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() }
                        }).ToList(),
                        Culture = new Culture
                        {
                            Id = userPreference.Culture.Id

                        },
                        UserProfile = new UserProfile
                        {
                            Id = userPreference.UserProfile.Id,
                            PhoneNumber = userPreference.UserProfile.PhoneNumber,
                            Email = userPreference.UserProfile.Email
                        },
                        UserDelegations = userPreference.UserDelegations.Where(a => a.StatusId == InProcess || a.StatusId == Approved || a.StatusId == Rejected).Select(u => new UserDelegation
                        {
                            Id = u.Id,
                            FromDate = u.FromDate,
                            ToDate = u.ToDate,
                            FromDateH = u.FromDateH,
                            ToDateH = u.ToDateH,
                            UserPreferenceId = u.UserPreferenceId,
                            ConfidentialityId = u.ConfidentialityId,
                            PriorityId = u.PriorityId,
                            StatusId = u.StatusId,
                            UserProfileId = u.UserProfileId,
                            TransactionTypeId = u.TransactionTypeId,
                            RejectionReason = u.RejectionReason,
                            OrgUnitId = u.OrgUnitId,
                            ReceiveCopy = u.ReceiveCopy,
                            ShowTransaction = u.ShowTransaction,
                            TransacionCategoryIds = u.TransacionCategoryIds,
                            TransacionConfidentialityIds = u.TransacionConfidentialityIds,
                            UserProfile = new UserProfile
                            {
                                Id = u.UserProfile.Id,
                                LocalName = u.UserProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            },

                            OrgUnit = new OrgUnit
                            {
                                Id = u.OrgUnit.Id,
                                LocalName = u.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            },
                            //TransactionType = new Lookup
                            //{
                            //    Id = u.TransactionType.Id,
                            //    Text = u.TransactionType.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            //},
                            //Confidentiality = new Permission
                            //{
                            //    Id = u.Confidentiality.Id,
                            //    LocalName = u.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            //},
                            //OrgUnit = new OrgUnit
                            //{
                            //    Id = u.OrgUnit.Id,
                            //    LocalName = u.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            //},
                            //TransactionType = new Lookup
                            //{
                            //    Id = u.TransactionType.Id,
                            //    Text = u.TransactionType.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            //},
                            //Confidentiality = new Permission
                            //{
                            //    Id = u.Confidentiality.Id,
                            //    LocalName = u.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            //},

                            //Priority = new Priority
                            //{
                            //    Id = u.Priority.Id,
                            //    Text = u.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            //},
                            Status = new Lookup
                            {
                                Id = u.Status.Id,
                                Text = u.Status.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            },


                        }).ToList()
                    };

                    userPreferenceObject.MyDelegations = _oMCSDbContext.UserDelegations.Where(u => u.UserProfileId == userId).ToList();

                    var UserPreferenceIdList = userPreferenceObject.MyDelegations.Select(o => o.UserPreferenceId).ToList();
                    List<UserPreference> UserPreferenceList = _oMCSDbContext.UserPreference.Where(u => UserPreferenceIdList.Contains(u.Id)).ToList();

                    foreach (var userDelegation in userPreferenceObject.MyDelegations)
                    {
                        userDelegation.UserPreference = UserPreferenceList.FirstOrDefault(y => y.Id == userDelegation.UserPreferenceId).UserProfile;
                        userDelegation.UserPreference.LocalName = UserPreferenceList.FirstOrDefault(y => y.Id == userDelegation.UserPreferenceId).UserProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText();
                    }
                    return userPreferenceObject;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public NotificationSubscriptions GetUserNotificationSubscriptions(int userId, string cultureName)
        {
            try
            {



                NotificationSubscriptions notificationSubscriptions = _oMCSDbContext.UserPreference.FirstOrDefault(p => p.UserProfileId == userId).NotificationSubscriptions;

                return notificationSubscriptions;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public byte[] GetUserSignByType(int userId, int signType)
        {
            try
            {

                byte[] content = null;
                var userPreference = _oMCSDbContext.UserPreference.Where(p => p.UserProfileId == userId);

                switch (signType)
                {
                    case (int)SigntureType.Electronic:
                        content = userPreference.Select(x => x.Signature).FirstOrDefault();
                        break;
                    case (int)SigntureType.Command:
                        content = userPreference.Select(x => x.SignatureCommand).FirstOrDefault();
                        break;
                    case (int)SigntureType.Behalf:
                        content = userPreference.Select(x => x.SignatureBehalf).FirstOrDefault();
                        break;
                    case (int)SigntureType.Message:
                        content = userPreference.Select(x => x.MessageSignatureDoc).FirstOrDefault();
                        break;
                    case (int)SigntureType.Seal:
                        content = userPreference.Select(x => x.SealSignatureDoc).FirstOrDefault();
                        break;
                    case (int)SigntureType.Marking:
                        content = userPreference.Select(x => x.MarkingDoc).FirstOrDefault();
                        break;

                }


                return content;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public UserPreference GetUserPreferenceForLogin(int userId, string cultureName)
        {
            try
            {
                UserPreference userPreference = _oMCSDbContext.UserPreference
                                                              .Where(p => p.UserProfileId == userId)
                                                              .Select(up => new
                                                              {
                                                                  up.Signature,
                                                                  up.SignatureBehalf,
                                                                  up.SignatureCommand,
                                                                  up.MarkingDoc,
                                                                  up.MessageSignatureDoc,
                                                                  up.SealSignatureDoc,
                                                                  up.CultureId,
                                                                  up.ThemeId,
                                                                  up.SMSNotifications,
                                                                  up.SignaturePasswordText,
                                                                  up.DefaultDisplay,
                                                                  up.DefaultAssignmentPaper
                                                              }).ToList().Select(p => new UserPreference
                                                              {
                                                                  Signature = p.Signature,
                                                                  MessageSignatureDoc = p.MessageSignatureDoc,
                                                                  SealSignatureDoc = p.SealSignatureDoc,
                                                                  SignatureBehalf = p.SignatureBehalf,
                                                                  SignatureCommand = p.SignatureCommand,
                                                                  MarkingDoc = p.MarkingDoc,
                                                                  CultureId = p.CultureId,
                                                                  ThemeId = p.ThemeId,
                                                                  SMSNotifications = p.SMSNotifications,
                                                                  SignaturePasswordText = p.SignaturePasswordText,
                                                                  DefaultDisplay = p.DefaultDisplay,
                                                                  DefaultAssignmentPaper = p.DefaultAssignmentPaper
                                                              }).FirstOrDefault();

                return userPreference;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<UserPreference> GetUserPreferenceByUserIds(List<int> userIds)
        {
            try
            {
                List<UserPreference> userPreference = _oMCSDbContext.UserPreference.Where(p => userIds.Contains(p.UserProfileId)).ToList().Select(p => new UserPreference
                {
                    Id = p.Id,
                    UserProfile = new UserProfile { Id = p.UserProfile.Id },
                    Culture = new Culture { Id = p.Culture.Id },
                    NotificationSubscriptions = p.NotificationSubscriptions
                }).ToList();

                return userPreference;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public UserPreference GetUserPreferenceByUserId(int userId)
        {
            try
            {
                UserPreference userPreference = _oMCSDbContext.UserPreference.Where(p => p.UserProfileId == userId).ToList().Select(p => new UserPreference
                {
                    Id = p.Id,
                    IsDelegationEnabled = p.IsDelegationEnabled,
                    UserProfileId = p.UserProfile.Id,
                    UserProfile = p.UserProfile,
                    OTP = p.OTP,
                    OTPCreatedOn = p.OTPCreatedOn,
                    AssignmentPaper = p.AssignmentPaper,
                    UserDelegations = p.UserDelegations.Select(u => new UserDelegation
                    {
                        Id = u.Id,
                        FromDate = u.FromDate,
                        ToDate = u.ToDate,
                        FromDateH = u.FromDateH,
                        ToDateH = u.ToDateH,
                        UserPreferenceId = u.UserPreferenceId,
                        UserProfileId = u.UserProfile.Id,
                        OrgUnitId = u.OrgUnit.Id,
                        TransactionTypeId = u.TransactionTypeId,//u.TransactionType.Id,
                        ConfidentialityId = u.ConfidentialityId,//u.Confidentiality.Id,
                        PriorityId = u.PriorityId,//u.Priority.Id,
                        StatusId = u.StatusId,
                        ReceiveCopy = u.ReceiveCopy,
                        ShowTransaction = u.ShowTransaction,
                        TransacionCategoryIds = u.TransacionCategoryIds,
                        TransacionConfidentialityIds = u.TransacionConfidentialityIds
                    }).ToList()
                }).FirstOrDefault();
                return userPreference;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public static UserPreference GetUserPreferenceByUserIdForWordAddIn(int userId)
        {
            try
            {
                MCSDbContext _oMCSDbContextWordAddIn = new MCSDbContext();

                UserPreference userPreference = _oMCSDbContextWordAddIn.UserPreference.Where(p => p.UserProfileId == userId).ToList().Select(p => new UserPreference
                {
                    Id = p.Id,
                    IsDelegationEnabled = p.IsDelegationEnabled,
                    UserProfileId = p.UserProfile.Id,
                    UserProfile = p.UserProfile,
                    OTP = p.OTP,
                    OTPCreatedOn = p.OTPCreatedOn,
                    AssignmentPaper = p.AssignmentPaper,
                    UserDelegations = p.UserDelegations.Select(u => new UserDelegation
                    {
                        Id = u.Id,
                        FromDate = u.FromDate,
                        ToDate = u.ToDate,
                        FromDateH = u.FromDateH,
                        ToDateH = u.ToDateH,
                        UserPreferenceId = u.UserPreferenceId,
                        UserProfileId = u.UserProfile.Id,
                        OrgUnitId = u.OrgUnit.Id,
                        TransactionTypeId = u.TransactionTypeId,//u.TransactionType.Id,
                        ConfidentialityId = u.ConfidentialityId,//u.Confidentiality.Id,
                        PriorityId = u.PriorityId,//u.Priority.Id,
                        StatusId = u.StatusId,
                        ReceiveCopy = u.ReceiveCopy,
                        ShowTransaction = u.ShowTransaction,
                        TransacionCategoryIds = u.TransacionCategoryIds,
                        TransacionConfidentialityIds = u.TransacionConfidentialityIds
                    }).ToList(),
                    Signature = p.Signature,
                    SealSignatureDoc = p.SealSignatureDoc,
                    MessageSignatureDoc = p.MessageSignatureDoc,
                    SignatureCommand = p.SignatureCommand,
                    SignatureBehalf = p.SignatureBehalf,
                    MarkingDoc = p.MarkingDoc,


                }).FirstOrDefault();
                return userPreference;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public UserPreference GetUserPreference(int userPreferenceId)
        {
            try
            {
                return FindBy(p => p.Id == userPreferenceId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public UserDelegation GetUserDelegationById(int id, string cultureName)
        {
            try
            {
                UserDelegation userDelegation = _oMCSDbContext.UserDelegations.Where(u => u.Id == id).ToList().Select(u => new UserDelegation
                {
                    Id = u.Id,
                    CreatedBy = u.CreatedBy,
                    FromDate = u.FromDate,
                    ToDate = u.ToDate,
                    FromDateH = u.FromDateH,
                    ToDateH = u.ToDateH,
                    UserPreferenceId = u.UserPreferenceId,
                    ConfidentialityId = u.ConfidentialityId,
                    PriorityId = u.PriorityId,
                    StatusId = u.StatusId,
                    UserProfileId = u.UserProfileId,
                    TransactionTypeId = u.TransactionTypeId,
                    RejectionReason = u.RejectionReason,
                    ReceiveCopy = u.ReceiveCopy,
                    ShowTransaction = u.ShowTransaction,
                    OrgUnitId = u.OrgUnitId,
                    UserProfile = new UserProfile
                    {
                        Id = u.UserProfile.Id,
                        LocalName = u.UserProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                    OrgUnit = new OrgUnit
                    {
                        Id = u.OrgUnit.Id,
                        LocalName = u.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                    //TransactionType = new Lookup
                    //{
                    //    Id = u.TransactionType.Id,
                    //    Text = u.TransactionType.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    //},
                    //Confidentiality = new Permission
                    //{
                    //    Id = u.Confidentiality.Id,
                    //    LocalName = u.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    //},

                    //Priority = new Priority
                    //{
                    //    Id = u.Priority.Id,
                    //    Text = u.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    //},
                    Status = new Lookup
                    {
                        Id = u.Status.Id,
                        Text = u.Status.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    }

                }).FirstOrDefault();


                return userDelegation;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<UserDelegation> GetUserDelegations(int preferenceId, SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<UserDelegation> userDelegations =
                    _oMCSDbContext.UserPreference.Where(p => p.Id == preferenceId).FirstOrDefault().UserDelegations.AsQueryable();

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        switch (filter.ColumnName)
                        {
                            case "DirectedTo":
                                userDelegations = FilterByDirectedTo(userDelegations, filter.Value, filter.Type, searchCriteria.CultureName);
                                break;
                            case "OrgUnit":
                                userDelegations = FilterByToOrgUnit(userDelegations, filter.Value, filter.Type, searchCriteria.CultureName);
                                break;
                            case "Priority":
                                userDelegations = FilterByPriority(userDelegations, filter.Value, filter.Type, searchCriteria.CultureName);
                                break;
                            case "SourceType":
                                userDelegations = FilterBySourceType(userDelegations, filter.Value, filter.Type, searchCriteria.CultureName);
                                break;
                            case "Confidentiality":
                                userDelegations = FilterByConfidentiality(userDelegations, filter.Value, filter.Type, searchCriteria.CultureName);
                                break;
                        }
                    }
                }

                switch (searchCriteria.OrderBy)
                {
                    case "DirectedTo":
                        userDelegations = OrderByDirectedTo(userDelegations, searchCriteria);

                        break;
                    case "OrgUnit":
                        userDelegations = OrderByToOrgUnit(userDelegations, searchCriteria);

                        break;
                    case "Priority":
                        userDelegations = OrderByPriority(userDelegations, searchCriteria);

                        break;
                    case "SourceType":
                        userDelegations = OrderBySourceType(userDelegations, searchCriteria);

                        break;
                    case "Confidentiality":
                        userDelegations = OrderByConfidentiality(userDelegations, searchCriteria);

                        break;
                }

                rowsCount = userDelegations.Count();

                userDelegations = userDelegations.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                   .Take(searchCriteria.PageSize);

                return userDelegations.ToList().Select(u => new UserDelegation
                {
                    Id = u.Id,
                    FromDate = u.FromDate,
                    ToDate = u.ToDate,
                    FromDateH = u.FromDateH,
                    ToDateH = u.ToDateH,
                    UserProfile = new UserProfile
                    {
                        Id = u.UserProfile.Id,
                        LocalName = u.UserProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    },

                    OrgUnit = new OrgUnit
                    {
                        Id = u.OrgUnit.Id,
                        LocalName = u.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    },
                    //TransactionType = new Lookup
                    //{
                    //    Id = u.TransactionType.Id,
                    //    Text = u.TransactionType.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    //},
                    //Confidentiality = new Permission
                    //{
                    //    Id = u.Confidentiality.Id,
                    //    LocalName = u.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    //},

                    //Priority = new Priority
                    //{
                    //    Id = u.Priority.Id,
                    //    Text = u.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    //},
                    Status = new Lookup
                    {
                        Id = u.Status.Id,
                        Text = u.Status.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<UserDelegation> GetUserDelegationsByUserId(int? userId, string cultureName, int? orgUnitId, SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                //IQueryable<UserDelegation> userDelegations =
                //    _oMCSDbContext.UserPreference
                //                  .Where(p => p.UserProfile.Id == userId)
                //                  .FirstOrDefault().UserDelegations.AsQueryable();
                int Rejected = DelegationStatus.Rejected.LookupIdentity(LookupCategory.DelegationStatus, cultureName);
                int Approved = DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, cultureName);
                int InProcess = DelegationStatus.InProcess.LookupIdentity(LookupCategory.DelegationStatus, cultureName);
                IQueryable<UserDelegation> userDelegations =
                                    (from usrDeleg in _oMCSDbContext.UserDelegations
                                     join UserPref in _oMCSDbContext.UserPreference on usrDeleg.UserPreferenceId equals UserPref.Id
                                     where (userId == null || UserPref.UserProfileId == userId)
                                           && (usrDeleg.StatusId == Approved || usrDeleg.StatusId == InProcess || usrDeleg.StatusId == Rejected
                                           && (orgUnitId == null || usrDeleg.OrgUnitId == orgUnitId))
                                     select usrDeleg);

                rowsCount = userDelegations.Count();

                userDelegations = userDelegations.OrderBy(d => d.Id).OrderByDescending(x => x.Id).Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                   .Take(searchCriteria.PageSize);

                var userDelegationList = userDelegations.ToList();
                return userDelegationList.Select(u => new UserDelegation
                {
                    Id = u.Id,
                    FromDate = u.FromDate,
                    ToDate = u.ToDate,
                    FromDateH = u.FromDateH,
                    ToDateH = u.ToDateH,
                    UserPreferenceId = u.UserPreferenceId,
                    UserPreference = u.UserPreference,
                    ConfidentialityId = u.ConfidentialityId,
                    PriorityId = u.PriorityId,
                    StatusId = u.StatusId,
                    UserProfileId = u.UserProfileId,
                    TransactionTypeId = u.TransactionTypeId,
                    RejectionReason = u.RejectionReason,
                    OrgUnitId = u.OrgUnitId,
                    UserProfile = new UserProfile
                    {
                        Id = u.UserProfile.Id,
                        LocalName = u.UserProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    },

                    OrgUnit = new OrgUnit
                    {
                        Id = u.OrgUnit.Id,
                        LocalName = u.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    },
                    //TransactionType = new Lookup
                    //{
                    //    Id = u.TransactionType.Id,
                    //    Text = u.TransactionType.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    //},
                    //Confidentiality = new Permission
                    //{
                    //    Id = u.Confidentiality.Id,
                    //    LocalName = u.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    //},

                    //Priority = new Priority
                    //{
                    //    Id = u.Priority.Id,
                    //    Text = u.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    //},
                    Status = new Lookup
                    {
                        Id = u.Status.Id,
                        Text = u.Status.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<UserDelegation> FilterByDirectedTo(IQueryable<UserDelegation> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(d => d.UserProfile.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(d => d.UserProfile.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(d => d.UserProfile.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(d => d.UserProfile.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<UserDelegation> FilterByToOrgUnit(IQueryable<UserDelegation> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(d => d.OrgUnit.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(d => d.OrgUnit.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(d => d.OrgUnit.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(d => d.OrgUnit.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<UserDelegation> FilterByPriority(IQueryable<UserDelegation> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(d => d.Priority.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(d => d.Priority.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(d => d.Priority.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(d => d.Priority.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<UserDelegation> FilterBySourceType(IQueryable<UserDelegation> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(d => d.TransactionType.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(d => d.TransactionType.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(d => d.TransactionType.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(d => d.TransactionType.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<UserDelegation> FilterByConfidentiality(IQueryable<UserDelegation> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(d => d.Confidentiality.Name.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(d => d.Confidentiality.Name.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(d => d.Confidentiality.Name.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(d => d.Confidentiality.Name.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<UserDelegation> OrderByDirectedTo(IQueryable<UserDelegation> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.UserProfile.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.OrderByDescending(t => t.UserProfile.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<UserDelegation> OrderByToOrgUnit(IQueryable<UserDelegation> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.OrgUnit.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.OrderByDescending(t => t.OrgUnit.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<UserDelegation> OrderByPriority(IQueryable<UserDelegation> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.Priority.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.OrderByDescending(t => t.Priority.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<UserDelegation> OrderBySourceType(IQueryable<UserDelegation> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.TransactionType.Localizations.Where(c => c.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.OrderByDescending(t => t.TransactionType.Localizations.Where(c => c.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<UserDelegation> OrderByConfidentiality(IQueryable<UserDelegation> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.Confidentiality.Name.Localizations.Where(c => c.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.OrderByDescending(t => t.Confidentiality.Name.Localizations.Where(c => c.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        public int AddDistributionList(DistributionList distributionList)
        {
            try
            {
                if (distributionList != null)
                {
                    _oMCSDbContext.distributionLists.Add(distributionList);
                    _oMCSDbContext.SaveChanges();

                    return distributionList.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int SaveDistributionListDetails(List<DistributionListDetails> distributionListDetails, int DistributionListId)
        {
            try
            {
                distributionListDetails.ForEach(dl => dl.DistributionListId = DistributionListId);
                _oMCSDbContext.DistributionListDetails.RemoveRange(_oMCSDbContext.DistributionListDetails.Where(dl => dl.DistributionListId == DistributionListId));

                _oMCSDbContext.DistributionListDetails.AddRange(distributionListDetails);
                _oMCSDbContext.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int UpdateDistributionList(DistributionList distributionList)
        {
            try
            {
                if (distributionList != null)
                {
                    var oldDistributionList = _oMCSDbContext.distributionLists.Find(distributionList.Id);
                    oldDistributionList.Name = distributionList.Name;
                    oldDistributionList.DistributionListDetails = distributionList.DistributionListDetails;
                    _oMCSDbContext.SaveChanges();
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int DeleteDistributionList(int distributionListId)
        {
            try
            {
                var deletedDistributionList = new DistributionList();

                DistributionList entityToDelete = _oMCSDbContext.distributionLists.Include(dl => dl.DistributionListDetails).Where(list => list.Id == distributionListId).FirstOrDefault();

                if (entityToDelete != null)
                {
                    if (entityToDelete.DistributionListDetails != null)
                    {

                        int DistributionListDetailsCount = entityToDelete.DistributionListDetails.Count;
                        for (int i = 0; i < DistributionListDetailsCount; i++)
                        {
                            _oMCSDbContext.Entry(entityToDelete.DistributionListDetails[0]).State = EntityState.Deleted;
                        }
                    }
                    deletedDistributionList = _oMCSDbContext.Set<DistributionList>().Remove(entityToDelete);
                    _oMCSDbContext.SaveChanges();
                }

                if (deletedDistributionList.Id > 0)
                {
                    return deletedDistributionList.Id;
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<DistributionList> GetDistributionList(int userId, int orgUnitId)
        {
            try
            {
                if (userId != 0 && orgUnitId != 0)
                {
                    List<DistributionList> distributionLists = _oMCSDbContext.distributionLists.Where(dl => (dl.UserId == userId | dl.UserId == null) && dl.OrgUnitId == orgUnitId).Include(d => d.DistributionListDetails).ToList();
                    return distributionLists;
                }
                return new List<DistributionList>(); ;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public DistributionList GetDistributionListById(int userId, int orgUnitId, int id)
        {
            try
            {
                if (userId != 0 && orgUnitId != 0)
                {
                    DistributionList distributionList = _oMCSDbContext.distributionLists.Where(dl => (dl.UserId == userId | dl.UserId == null) && dl.OrgUnitId == orgUnitId && dl.Id == id).Include(d => d.DistributionListDetails).FirstOrDefault();
                    return distributionList;
                }
                return new DistributionList(); ;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTransactionPath(TransactionPath transactionPath)
        {
            try
            {
                TransactionPath transactionPathEntity = _oMCSDbContext.TransactionPaths.Where(a => a.Id == transactionPath.Id).Include(d => d.TransactionPathDetails).FirstOrDefault();

                if (transactionPathEntity != null)
                {
                    //_oMCSDbContext.Entry(transactionPathEntity).State = EntityState.Modified;
                    var pathEntry = _oMCSDbContext.Entry(transactionPathEntity);
                    pathEntry.CurrentValues.SetValues(transactionPath);
                    _oMCSDbContext.SaveChanges();
                }
                else
                {
                    _oMCSDbContext.TransactionPaths.Add(transactionPath);
                    _oMCSDbContext.SaveChanges();
                    return;
                }

                //Add all details if there is no details added before
                if (transactionPathEntity != null && (transactionPathEntity.TransactionPathDetails == null || transactionPathEntity.TransactionPathDetails.Count == 0))
                {
                    foreach (var item in transactionPath.TransactionPathDetails)
                    {
                        item.TransactionPathId = transactionPathEntity.Id;
                        _oMCSDbContext.TransactionPathDetails.Add(item);
                    }
                    _oMCSDbContext.SaveChanges();
                    return;
                }

                //There are details added before
                if (transactionPathEntity != null && transactionPathEntity.TransactionPathDetails != null && transactionPathEntity.TransactionPathDetails.Count > 0)
                {
                    foreach (var item in transactionPath.TransactionPathDetails)
                    {
                        var originalPathDetails = transactionPathEntity.TransactionPathDetails
                                                                .Where(c => c.Id == item.Id && c.Id != 0)
                                                                .SingleOrDefault();
                        //Updated Item
                        if (originalPathDetails != null)
                        {
                            item.TransactionPathId = transactionPathEntity.Id;
                            var pathDetailsEntry = _oMCSDbContext.Entry(originalPathDetails);
                            pathDetailsEntry.CurrentValues.SetValues(item);
                        }
                        //Added item
                        else
                        {
                            item.TransactionPathId = transactionPathEntity.Id;
                            _oMCSDbContext.TransactionPathDetails.Add(item);
                        }
                    }
                    foreach (var originalPathDetails in transactionPathEntity.TransactionPathDetails.Where(c => c.Id != 0).ToList())
                    {
                        if (!transactionPath.TransactionPathDetails.Any(c => c.Id == originalPathDetails.Id))
                        {
                            _oMCSDbContext.TransactionPathDetails.Remove(originalPathDetails);
                        }
                    }
                }
                _oMCSDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<TransactionPath> GetAllPaths(int pageIndex, int pageSize, string cultureName, out int rowsCount)
        {
            try
            {
                IQueryable<TransactionPath> transactionPaths =
                    _oMCSDbContext.TransactionPaths.AsQueryable();

                rowsCount = transactionPaths.Count();

                transactionPaths = transactionPaths.OrderByDescending(r => r.CreatedOn).Skip((pageIndex - 1) * pageSize)
                                   .Take(pageSize);

                return transactionPaths.ToList().Select(u => new TransactionPath
                {
                    Id = u.Id,
                    Name = u.Name,
                    OrgUnitId = u.OrgUnitId,
                    UserId = u.UserId != null ? u.UserId : null,
                    TransactionTypeId = u.TransactionTypeId,
                    User = (u.User != null) ? new UserProfile
                    {
                        Id = u.User.Id,
                        LocalName = u.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    OrgUnit = new OrgUnit
                    {
                        Id = u.OrgUnit.Id,
                        LocalName = u.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                    TransactionType = new Lookup
                    {
                        Id = u.TransactionType.Id,
                        Text = u.TransactionType.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<TransactionPath> GetTransactionPath(int? userId, int? orgUnitId, int pageIndex, int pageSize, string cultureName, out int rowsCount)
        {
            try
            {
                IQueryable<TransactionPath> transactionPaths =
                    _oMCSDbContext.TransactionPaths
                                  .Where(p => (userId == null || p.UserId == userId) &&
                                              (orgUnitId == null || p.OrgUnitId == orgUnitId)
                                              || (p.UserId == null) && p.OrgUnitId == orgUnitId)

                                  .AsQueryable();

                rowsCount = transactionPaths.Count();

                transactionPaths = transactionPaths.OrderByDescending(r => r.CreatedOn).Skip((pageIndex - 1) * pageSize)
                                   .Take(pageSize);

                return transactionPaths.ToList().Select(u => new TransactionPath
                {
                    Id = u.Id,
                    Name = u.Name,
                    OrgUnitId = u.OrgUnitId,
                    UserId = u.UserId != null ? u.UserId : null,
                    TransactionTypeId = u.TransactionTypeId,
                    User = (u.User != null) ? new UserProfile
                    {
                        Id = u.User.Id,
                        LocalName = u.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    OrgUnit = new OrgUnit
                    {
                        Id = u.OrgUnit.Id,
                        LocalName = u.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                    TransactionType = new Lookup
                    {
                        Id = u.TransactionType.Id,
                        Text = u.TransactionType.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<TransactionPath> GetPathsName(int OrgUnitId)
        {


            return _oMCSDbContext.TransactionPaths.Where(p => p.OrgUnitId == OrgUnitId)
                .Select(p =>
                    new
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList().Select(u => new TransactionPath
                    {
                        Id = u.Id,
                        Name = u.Name,
                    }).ToList();


        }
        public List<TransactionPath> GetTransactionPathForTransaction(int? userId, int? orgUnitId, string cultureName)
        {
            try
            {
                IQueryable<TransactionPath> transactionPaths =
                    _oMCSDbContext.TransactionPaths
                                  .Where(p => (userId == null || p.CreatedBy == userId) &&
                                              (orgUnitId == null || p.OrgUnitId == orgUnitId)
                                        )
                                  .AsQueryable();

                return transactionPaths.Select(p =>
                    new
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList().Select(u => new TransactionPath
                    {
                        Id = u.Id,
                        Name = u.Name,
                    }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TransactionPath GetTransactionPathById(int pathId, string cultureName)
        {
            try
            {
                List<TransactionPath> transactionPath = _oMCSDbContext.TransactionPaths.Where(p => p.Id == pathId).Include(d => d.TransactionPathDetails).ToList();

                return transactionPath.Select(u => new TransactionPath
                {
                    Id = u.Id,
                    Name = u.Name,
                    OrgUnitId = u.OrgUnitId,
                    UserId = u.UserId != null ? u.UserId : null,
                    TransactionTypeId = u.TransactionTypeId,
                    IsReadOnly = _oMCSDbContext.TransactionAssignments.Where(a => a.TransactionPathId == pathId).Any(),
                    TransactionPathDetails = u.TransactionPathDetails.OrderBy(r => r.Sort).Select(d => new TransactionPathDetails
                    {
                        Id = d.Id,
                        ActionId = d.ActionId,
                        OrgUnitId = d.OrgUnitId,
                        TransactionPathId = d.TransactionPathId,
                        Sort = d.Sort,
                        UserId = d.UserId != null ? d.UserId : null,
                        User = (d.User != null) ? new UserProfile
                        {
                            Id = d.User.Id,
                            LocalName = d.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        OrgUnit = new OrgUnit
                        {
                            Id = d.OrgUnit.Id,
                            LocalName = d.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        },
                        Action = new Domain.Action
                        {
                            Id = d.Action.Id,
                            LocalName = d.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        }
                    }).ToList(),
                    User = (u.User != null) ? new UserProfile
                    {
                        Id = u.User.Id,
                        LocalName = u.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    OrgUnit = new OrgUnit
                    {
                        Id = u.OrgUnit.Id,
                        LocalName = u.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                    TransactionType = new Lookup
                    {
                        Id = u.TransactionType.Id,
                        Text = u.TransactionType.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int DeleteTransactionPath(int pathId)
        {
            try
            {
                TransactionPath entityToDelete = _oMCSDbContext.TransactionPaths
                                                                .Where(p => p.Id == pathId
                                                                       && !_oMCSDbContext.TransactionAssignments.Where(a => a.TransactionPathId == pathId).Any())
                                                                .FirstOrDefault();

                if (entityToDelete != null)
                {
                    _oMCSDbContext.TransactionPaths.Remove(entityToDelete);
                    _oMCSDbContext.SaveChanges();
                    return entityToDelete.Id;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTransactionPathDetailsSort(int pathId, int sort, string order)
        {
            try
            {
                List<TransactionPathDetails> transactionPathDetails = _oMCSDbContext.TransactionPathDetails
                                                                .Where(p => p.TransactionPathId == pathId)
                                                                .ToList();

                if (transactionPathDetails != null && transactionPathDetails.Count > 0 && transactionPathDetails.Count != 1)
                {
                    TransactionPathDetails pathDetails = transactionPathDetails.Where(d => d.Sort == sort).FirstOrDefault();

                    if (order == "up")
                    {
                        if (pathDetails.Sort != 1)
                        {
                            TransactionPathDetails swapDetails = transactionPathDetails.Where(d => d.Sort == sort - 1).FirstOrDefault();
                            pathDetails.Sort = pathDetails.Sort - 1;
                            swapDetails.Sort = swapDetails.Sort + 1;

                            _oMCSDbContext.Entry(pathDetails).State = EntityState.Modified;
                            _oMCSDbContext.Entry(swapDetails).State = EntityState.Modified;
                            _oMCSDbContext.SaveChanges();
                            return;
                        }
                    }
                    else if (order == "down")
                    {
                        if (pathDetails.Sort != transactionPathDetails.Count)
                        {
                            TransactionPathDetails swapDetails = transactionPathDetails.Where(d => d.Sort == sort + 1).FirstOrDefault();
                            pathDetails.Sort = pathDetails.Sort + 1;
                            swapDetails.Sort = swapDetails.Sort - 1;

                            _oMCSDbContext.Entry(pathDetails).State = EntityState.Modified;
                            _oMCSDbContext.Entry(swapDetails).State = EntityState.Modified;
                            _oMCSDbContext.SaveChanges();
                            return;
                        }
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateUserPreferenceFollowup(int userPreferenceId, int orgUnitId, int? followupOrgUnitId, int? followupUserId)
        {
            try
            {
                UserPreferenceFollowup UserPreferenceOld = _oMCSDbContext.UserPreferenceFollowups
                                                            .Where(f => f.UserPreferenceId == userPreferenceId && f.OrgUnitId == orgUnitId).FirstOrDefault();

                if (UserPreferenceOld != null)
                {
                    UserPreferenceOld.FollowUpOrgId = followupOrgUnitId;
                    UserPreferenceOld.FollowUpUserId = followupUserId;
                }
                else
                {
                    UserPreferenceFollowup userPreferenceFollowup = new UserPreferenceFollowup()
                    {
                        OrgUnitId = orgUnitId,
                        UserPreferenceId = userPreferenceId,
                        FollowUpOrgId = followupOrgUnitId,
                        FollowUpUserId = followupUserId
                    };

                    _oMCSDbContext.UserPreferenceFollowups.Add(userPreferenceFollowup);
                }

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }



        public List<UserDelegation> GetLoggedInUserDelegations(int UserId, string cultureName)
        {
            try
            {
                int approvedStatus = DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, cultureName);
                List<UserDelegation> userDelegationList = _oMCSDbContext.UserDelegations.Where(
                    u => u.UserProfileId == UserId && u.StatusId == approvedStatus &&
                    //u.StatusId == DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, cultureName) &&
                    DateTime.Now >= u.FromDate &&
                    DateTime.Now <= u.ToDate).ToList().Select(u => new UserDelegation
                    {
                        Id = u.Id,
                        FromDate = u.FromDate,
                        ToDate = u.ToDate,
                        FromDateH = u.FromDateH,
                        ToDateH = u.ToDateH,
                        Status = new Lookup
                        {
                            Id = u.Status.Id,
                            Text = u.Status.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        },
                        UserPreferenceId = u.UserPreferenceId,
                        UserProfile = new UserProfile
                        {
                            Id = u.UserProfile.Id,
                            LocalName = u.UserProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        }

                    }).ToList();

                var UserPreferenceIdList = userDelegationList.Select(o => o.UserPreferenceId).ToList();
                List<UserPreference> UserPreferenceList = _oMCSDbContext.UserPreference.Where(u => UserPreferenceIdList.Contains(u.Id)).ToList();

                foreach (var userDelegation in userDelegationList)
                {
                    userDelegation.UserPreference = UserPreferenceList.FirstOrDefault(y => y.Id == userDelegation.UserPreferenceId).UserProfile;
                    userDelegation.UserPreference.LocalName = UserPreferenceList.FirstOrDefault(y => y.Id == userDelegation.UserPreferenceId).UserProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText();
                }

                return userDelegationList;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public List<UserDelegation> GetUserDelegationsById(int UserId, string cultureName)
        {
            try
            {
                if (UserId <= 0)
                {
                    return null;
                }
                UserPreference userPreference =
                    _oMCSDbContext.UserPreference.Where(p => p.UserProfileId == UserId).FirstOrDefault();


                List<UserDelegation> userDelegations = userPreference.UserDelegations.ToList();

                var userDelegationResult = userDelegations.Where(
                    x =>
                        x.StatusId == DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, cultureName)
                        &&
                        DateTime.Now >= x.FromDate
                        &&
                        DateTime.Now <= x.ToDate
                ).ToList().Select(u => new UserDelegation
                {
                    Id = u.Id,
                    FromDate = u.FromDate,
                    ToDate = u.ToDate,
                    FromDateH = u.FromDateH,
                    ToDateH = u.ToDateH,
                    Status = new Lookup
                    {
                        Id = u.Status.Id,
                        Text = u.Status.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                    UserProfile = new UserProfile
                    {
                        Id = u.UserProfile.Id,
                        LocalName = u.UserProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                    UserPreference = new UserProfile
                    {
                        Id = u.UserPreference.Id,
                        LocalName = u.UserPreference.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },

                });
                return userDelegationResult.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion Methods

        public bool VerifySignaturePassword(string SignaturePasswordTxt, int userId)
        {
            try
            {
                bool isMatched = false;
                UserPreference userPreference = _oMCSDbContext.UserPreference.FirstOrDefault(up => up.UserProfileId == userId);
                if (userPreference != null && userPreference.SignaturePasswordText == SignaturePasswordTxt)
                {
                    isMatched = true;
                }
                return isMatched;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #region MobileAPI
        public void AddUserSignature(UserPreference userPreference, int userId, string cultureName)
        {
            try
            {
                UserPreference UserPreferenceOld = FindBy(up => up.UserProfileId == userId);
                if (UserPreferenceOld != null)
                {
                    UserPreferenceOld.Signature = userPreference.Signature;
                    UserPreferenceOld.SealSignatureDoc = userPreference.SealSignatureDoc;
                    UserPreferenceOld.MessageSignatureDoc = userPreference.MessageSignatureDoc;
                    UserPreferenceOld.SignatureBehalf = userPreference.SignatureBehalf;
                    UserPreferenceOld.SignatureCommand = userPreference.SignatureCommand;
                    UserPreferenceOld.SignaturePasswordText = userPreference.SignaturePasswordText;
                    UserPreferenceOld.FreeText = userPreference.FreeText;
                    UserPreferenceOld.SignaturePassword = userPreference.SignaturePasswordText != string.Empty;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public UserPreference GetUserSignature(int userId, string cultureName)
        {
            try
            {
                UserPreference UserPreference = _oMCSDbContext.UserPreference
                                                            .Where(p => p.UserProfileId == userId)
                                                            .Select(up => new
                                                            {
                                                                up.Signature,
                                                                up.SignatureBehalf,
                                                                up.SignatureCommand,
                                                                up.SignaturePassword,
                                                                up.SignaturePasswordText,
                                                                up.FreeText,
                                                                up.MessageSignatureDoc,
                                                                up.SealSignatureDoc,
                                                            }).ToList().Select(userPreference => new UserPreference
                                                            {
                                                                Signature = userPreference.Signature,
                                                                SignatureBehalf = userPreference.SignatureBehalf,
                                                                SignatureCommand = userPreference.SignatureCommand,
                                                                SignaturePassword = userPreference.SignaturePassword,
                                                                SignaturePasswordText = userPreference.SignaturePasswordText,
                                                                FreeText = userPreference.FreeText,
                                                                MessageSignatureDoc = userPreference.MessageSignatureDoc,
                                                                SealSignatureDoc = userPreference.SealSignatureDoc,
                                                            }).FirstOrDefault();

                return UserPreference;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool GenerateVerificationCode(int userId, string code)
        {
            try
            {
                UserPreference UserPreference = _oMCSDbContext.UserPreference.FirstOrDefault(p => p.UserProfileId == userId);
                if (UserPreference != null)
                {
                    UserPreference.OTP = code;
                    UserPreference.OTPCreatedOn = DateTime.Now;
                    _oMCSDbContext.Entry(UserPreference).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateUserPreference(int userId, string code)
        {
            try
            {
                var userPreference = _oMCSDbContext.UserPreference.FirstOrDefault(a => a.UserProfileId == userId);
                if (userPreference != null)
                {
                    userPreference.OTP = null;
                    userPreference.OTPCreatedOn = null;
                    _oMCSDbContext.Entry(userPreference).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateSignaturePassword(int userId, string signaturePassword, PasswordType passwordType)
        {
            try
            {
                var userPreference = _oMCSDbContext.UserPreference.FirstOrDefault(a => a.UserProfileId == userId);
                if (userPreference != null)
                {
                    userPreference.SignaturePasswordText = passwordType == PasswordType.Delete ? null : signaturePassword;
                    _oMCSDbContext.Entry(userPreference).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion
        public List<Theme> GetTheme()
        {
            try
            {
                return _oMCSDbContext.Themes.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public string GetThemesById(int id)
        {
            try
            {
                Theme theme = _oMCSDbContext.Themes.Where(f => f.Id == id).FirstOrDefault();
                if (theme != null)
                {
                    return theme.Path;

                }
                return null;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateSMSNotificationsConfirm(int userId, bool Confirm)
        {
            try
            {
                var userPreference = _oMCSDbContext.UserPreference.FirstOrDefault(a => a.UserProfileId == userId);
                if (userPreference != null)
                {
                    userPreference.SMSNotifications = Confirm;
                    _oMCSDbContext.Entry(userPreference).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool GetSMSNotificationsConfirmByUserId(int id)
        {
            try
            {
                var userPreference = _oMCSDbContext.UserPreference.Where(f => f.UserProfileId == id).FirstOrDefault();
                if (userPreference != null)
                {
                    return userPreference.SMSNotifications;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdatefollowUpUser(int userId, bool Confirm)
        {
            try
            {
                var userPreference = _oMCSDbContext.UserPreference.FirstOrDefault(a => a.UserProfileId == userId);
                if (userPreference != null)
                {
                    userPreference.IsFollowUpUser = Confirm;
                    _oMCSDbContext.Entry(userPreference).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateUserMobile(int userId, int orgunitId, bool AllowMobile)
        {
            try
            {
                var userMobile = _oMCSDbContext.UserMobiles.FirstOrDefault(a => a.UserId == userId);

                if (userMobile == null)
                {

                    UserMobile NewuserMobile = new UserMobile();
                    NewuserMobile.UserId = userId;
                    NewuserMobile.DefaultEntityId = orgunitId;
                    NewuserMobile.AllowMobile = AllowMobile;
                    NewuserMobile.CreatedOn = DateTime.Now;
                    NewuserMobile.LastLoginDate = DateTime.Now;
                    NewuserMobile.CreatedBy = userId;
                    NewuserMobile.IsUpdated = true;
                    NewuserMobile.EntityId = orgunitId;
                    NewuserMobile.UpdateFlags = 5;
                    NewuserMobile.ModefiedBy = null;
                    NewuserMobile.ModefiedOn = null;
                    _oMCSDbContext.UserMobiles.Add(NewuserMobile);
                    _oMCSDbContext.SaveChanges();


                }
                else
                {
                    userMobile.AllowMobile = AllowMobile;
                    userMobile.ModefiedBy = userId;
                    userMobile.ModefiedOn = DateTime.Now;
                    _oMCSDbContext.Entry(userMobile).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool GetfollowUpUserId(int id)
        {
            try
            {
                var userPreference = _oMCSDbContext.UserPreference.Where(f => f.UserProfileId == id).FirstOrDefault();
                if (userPreference != null)
                {
                    return userPreference.IsFollowUpUser;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<AssignmentPaperGroup> GetAssignmentPaperGroupsByUserId(int userId)
        {
            try
            {
                List<AssignmentPaperGroup> assignmentPaperGroupList = new List<AssignmentPaperGroup>();

                IQueryable<AssignmentPaperGroup> assignmentPaperGroups = (from assignmentPaper in _oMCSDbContext.AssignmentPaperGroups
                                                                          where assignmentPaper.UserId == userId
                                                                          select assignmentPaper);

                assignmentPaperGroupList = assignmentPaperGroups.ToList();

                return assignmentPaperGroupList;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<AssignmentPaperBeneficiary> GetBeneficiaryByAssignmentPaperGroupId(int groupId)
        {
            try
            {
                List<AssignmentPaperBeneficiary> assignmentPaperBeneficiaryList = new List<AssignmentPaperBeneficiary>();

                IQueryable<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries = (from assignmentPaperBeneficies in _oMCSDbContext.AssignmentPaperBeneficies
                                                                                       where assignmentPaperBeneficies.AssignmentPaperGroupId == groupId
                                                                                       select assignmentPaperBeneficies);

                assignmentPaperBeneficiaryList = assignmentPaperBeneficiaries.ToList();

                return assignmentPaperBeneficiaryList;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<AssignmentPaperBeneficiary> GetBeneficiaryByAssignmentPapers()
        {
            try
            {
                List<AssignmentPaperBeneficiary> assignmentPaperBeneficiaryList = new List<AssignmentPaperBeneficiary>();

                IQueryable<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries = 
                    (
                    from assignmentPaperBeneficies 
                    in _oMCSDbContext.AssignmentPaperBeneficies
                    select assignmentPaperBeneficies
                    );

                assignmentPaperBeneficiaryList = assignmentPaperBeneficiaries.ToList();

                return assignmentPaperBeneficiaryList;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void SaveAssignmentPaperGroup(AssignmentPaperGroup assignmentPaperGroup)
        {
            try
            {
                var maxOrder = _oMCSDbContext.AssignmentPaperGroups.Where(x => x.UserId == assignmentPaperGroup.UserId).OrderByDescending(x => x.OrderNo).FirstOrDefault();
                assignmentPaperGroup.OrderNo = maxOrder != null ? maxOrder.OrderNo + 1 : 1;
                _oMCSDbContext.AssignmentPaperGroups.Add(assignmentPaperGroup);
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public AssignmentPaperGroup GetAssignmentPaperGroupById(int assignmentPaperGroupId)
        {
            try
            {
                AssignmentPaperGroup assignmentPaperGroup = (from assignmentPaper in _oMCSDbContext.AssignmentPaperGroups
                                                             where assignmentPaper.Id == assignmentPaperGroupId
                                                             select assignmentPaper).First();
                return assignmentPaperGroup;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateAssignmentPaperGroup(AssignmentPaperGroup assignmentPaperGroup)
        {
            try
            {
                AssignmentPaperGroup assignmentPaperGroupOld = GetAssignmentPaperGroupById(assignmentPaperGroup.Id);
                if (assignmentPaperGroupOld != null)
                {
                    _oMCSDbContext.Entry(assignmentPaperGroupOld).CurrentValues.SetValues(assignmentPaperGroup);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public void ChangeGroupOrder(int id, bool isMoveUp)
        {
            try
            {
                var assignmentGroup = _oMCSDbContext.AssignmentPaperGroups.Where(a => a.Id == id).FirstOrDefault();
                AssignmentPaperGroup secondGroup = null;
                int oldOrder = assignmentGroup.OrderNo;
                if (isMoveUp)
                {
                    secondGroup = _oMCSDbContext.AssignmentPaperGroups.Where(a => a.UserId == assignmentGroup.UserId && a.OrderNo > oldOrder).OrderBy(x => x.OrderNo).FirstOrDefault();
                }
                else
                {
                    secondGroup = _oMCSDbContext.AssignmentPaperGroups.Where(a => a.UserId == assignmentGroup.UserId && a.OrderNo < oldOrder).OrderByDescending(x => x.OrderNo).FirstOrDefault();
                }

                if (secondGroup != null)
                {

                    assignmentGroup.OrderNo = secondGroup.OrderNo;
                    secondGroup.OrderNo = oldOrder;
                }

                _oMCSDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public int AddAllowedAssignment(AllowedAssignment allowedAssignment)
        {
            try
            {

                _oMCSDbContext.AllowedAssignments.Add(allowedAssignment);
                _oMCSDbContext.SaveChanges();
                return allowedAssignment.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<AllowedAssignment> GetAllowedAssignment(int userId, string cultureName)
        {
            try
            {

                List<AllowedAssignment> allowedAssignments = _oMCSDbContext.AllowedAssignments.Where(p => p.UserId == userId).ToList();
                allowedAssignments.ForEach(z =>
                {
                    z.User.LocalName = z.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                    z.ToUser.LocalName = z.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                    z.Entity.LocalName = z.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;

                });

                return allowedAssignments;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);

            }

        }

        public bool RemoveAllowedAssignment(int Id)
        {
            try
            {
                AllowedAssignment allowedAssignment = _oMCSDbContext.AllowedAssignments.Where(p => p.Id == Id).FirstOrDefault();

                _oMCSDbContext.AllowedAssignments.Remove(allowedAssignment);
                _oMCSDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public AllowedAssignment GetAllowedUserAssignment(int ToUserId, int FromUserId)
        {
            try
            {
                AllowedAssignment allowedAssignment = _oMCSDbContext.AllowedAssignments.Where(p => p.ToUserId == ToUserId && p.UserId == FromUserId).FirstOrDefault();
                return allowedAssignment;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteAssignmentPaperBeneficiary(int assignmentPaperGroupId)
        {
            try
            {
                IEnumerable<AssignmentPaperBeneficiary> assignmentPaperBeneficiaryList = _oMCSDbContext.AssignmentPaperBeneficies.Where(a => a.AssignmentPaperGroupId == assignmentPaperGroupId).ToList();
                _oMCSDbContext.AssignmentPaperBeneficies.RemoveRange(assignmentPaperBeneficiaryList);
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteAssignmentPaperGroup(int assignmentPaperGroupId)
        {
            try
            {
                AssignmentPaperGroup assignmentPaperGroup = _oMCSDbContext.AssignmentPaperGroups.Where(a => a.Id == assignmentPaperGroupId).FirstOrDefault();
                _oMCSDbContext.AssignmentPaperGroups.Remove(assignmentPaperGroup);
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
