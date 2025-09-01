using MCS.DTO;
using MCS.IntegrationServices.Models;
using MCS.IntegrationServices.Models.IAM.Role;
using MCS.IntegrationServices.Models.IAM.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Mappers
{
    public static class UserMapper
    {
        public static UserVM Map(UserDTO b)
        {
            if (b != null)
            {
                UserVM userVM = new UserVM()
                {
                    Id = b.Id,
                    Name = b.Name,
                    AccessToken = b.AccessToken,
                    BaseOrgUnitName = b.BaseOrgUnitName,
                    Claims = b.Claims,
                    Marking = b.Marking,
                    SessionId = b.SessionId,
                    Signature = b.Signature,
                    UserName = b.UserName,
                    UserCategoryName = b.UserCategoryName,
                    Email = b.Email,
                    LoclizationName = b.LoclizationName != null ? LocalizationMapper.Map(b.LoclizationName) : null,
                    UserOrgUnits = UserOrgUnitMapper.Map(b.UserOrgUnits),
                    PhoneNumber = b.PhoneNumber

                };
                return userVM;
            }
            return new UserVM();

        }

        public static List<UserDTO> Map(IList<UserVM> userVMs)
        {
            if (userVMs == null || !userVMs.Any())
            {
                return new List<UserDTO>();
            }
            List<UserDTO> userDTOs = userVMs
                .Select(b => new UserDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    AccessToken = b.AccessToken,
                    BaseOrgUnitName = b.BaseOrgUnitName,
                    Claims = b.Claims,
                    LoclizationName = b.LoclizationName != null ? LocalizationMapper.Map(b.LoclizationName) : null,
                    Marking = b.Marking,
                    SessionId = b.SessionId,
                    Signature = b.Signature,
                    UserName = b.UserName,
                    UserCategoryName = b.UserCategoryName,
                    UserOrgUnits = UserOrgUnitMapper.Map(b.UserOrgUnits)
                }).ToList();
            return userDTOs;
        }
        public static List<UserVM> Map(IList<UserDTO> userDTOs)
        {
            if (userDTOs == null || !userDTOs.Any())
            {
                return new List<UserVM>();
            }
            List<UserVM> userVMs = userDTOs
                .Select(b => new UserVM
                {
                    Id = b.Id,
                    Name = b.Name,
                    AccessToken = b.AccessToken,
                    BaseOrgUnitName = b.BaseOrgUnitName,
                    Claims = b.Claims,
                    LoclizationName = LocalizationMapper.Map(b.LoclizationName),
                    Marking = b.Marking,
                    SessionId = b.SessionId,
                    Signature = b.Signature,
                    UserName = b.UserName,
                    UserCategoryName = b.UserCategoryName,
                    UserOrgUnits = UserOrgUnitMapper.Map(b.UserOrgUnits)
                }).ToList();
            return userVMs;

        }

        public static UserDTO Map(UserVM b)
        {
            if (b != null)
            {
                UserDTO userDTO = new UserDTO()
                {
                    Id = b.Id,
                    Name = b.Name,
                    AccessToken = b.AccessToken,
                    BaseOrgUnitName = b.BaseOrgUnitName,
                    Claims = b.Claims,
                    LoclizationName = LocalizationMapper.Map(b.LoclizationName),
                    Marking = b.Marking,
                    SessionId = b.SessionId,
                    Signature = b.Signature,
                    UserName = b.UserName,
                    UserCategoryName = b.UserCategoryName,
                    UserOrgUnits = UserOrgUnitMapper.Map(b.UserOrgUnits)
                };
                return userDTO;
            }
            return new UserDTO();

        }
        public static List<UserDetailsResponse> Map(IList<UserProfileDTO> userDTOs)
        {

            if (userDTOs == null || !userDTOs.Any())
            {
                return new List<UserDetailsResponse>();
            }
            List<UserDetailsResponse> userVMs = userDTOs
                .Select(b => new UserDetailsResponse
                {
                    Id = b.Id,
                    Category = b.Category,
                    Email = b.Email,
                    InternalNumber = b.InternalNumber,
                    IsActive = b.IsActive,
                    MainOrgUnitName = b.MainOrgUnitName,
                    PhoneNumber = b.PhoneNumber,
                    Username = b.UserName,
                    GenderId = b.GenderId,
                    CategoryId = b.CategoryId,
                    AllowMobile = b.AllowMobile,
                    IsManager = b.IsManager,
                    MainOrgUnitId = b.MainOrgUnitId,
                    Names = LocalizationMapper.Map(b.Names),
                    OrgUnits = b?.OrgUnitDTOs?.Select(org => new OrgunitResponse
                    {
                        OrgUnitId = org.Id,
                        OrgUnitName = org.Name,
                    }).ToList(),
                    TitleId = b.TitileId,
                    Title = b.Title,
                    TransactionProcessingPeriod = b.TransactionProcessingPeriod,
                    UserRoles = b.UserGroupDTOs?.Select(gdto => new UserRoleResponse
                    {
                        RoleId = gdto.GroupId,
                        RoleName = gdto.GroupName,

                    }).ToList(),
                    UserNationalId = b.UserNationalId,
                    Gender = b.Gender,



                }).ToList();
            return userVMs;

        }


        public static AddUserProfileDTO Map(CreateUserRequest createUserRequest)
        {
            if (createUserRequest == null)
                return null;

            return new AddUserProfileDTO
            {

                IsActive = createUserRequest.IsActive,
                Email = string.IsNullOrEmpty(createUserRequest.Email) ? "" : createUserRequest.Email,
                PhoneNumber = createUserRequest.PhoneNumber,
                TransactionProcessingPeriod = createUserRequest.TransactionProcessingPeriod,
                UserName = string.IsNullOrEmpty(createUserRequest.Username) ? "" : createUserRequest.Username,
                TitleId = createUserRequest.TitleId,
                CategoryId = createUserRequest.CategoryId,
                Names = Map(createUserRequest.Names),
                OrgUnits = createUserRequest.OrgUnits,
                UserNationalId = createUserRequest.UserNationalId,
                MainOrgUnitId = createUserRequest.MainOrgUnitId,
                Gender = createUserRequest.GenderId,
                //GroupId = oAddUserProfileDTO.RoleId,
                IsManager = createUserRequest.IsManager,
                Password = "p@ssw0rd",
                UserGroups = createUserRequest.UserRoles,
                AllowMobile = createUserRequest.AllowMobile,
                InternalNumber = createUserRequest.InternalNumber,
                UserMobileClassId = createUserRequest.UserMobileClassId,
                UserMobileClassName = createUserRequest.UserMobileClassName,




            };

        }

        public static List<LocalizationDTO> Map(List<LocalizationRequest> list)
        {
            if (list == null || list.Count == 0) return new List<LocalizationDTO>();

            return list.Select(name => new LocalizationDTO
            {

                CultureId = name.CultureId,
                Text = name.Text,




            }).ToList();
        }


        public static EditUserProfileDTO Update_Map(UpdateUserRequest createUserRequest)
        {
            if (createUserRequest == null)
                return null;

            return new EditUserProfileDTO
            {


                IsActive = createUserRequest.IsActive,
                Email = string.IsNullOrEmpty(createUserRequest.Email) ? "" : createUserRequest.Email,
                PhoneNumber = createUserRequest.PhoneNumber,
                TransactionProcessingPeriod = createUserRequest.TransactionProcessingPeriod,
                UserName = string.IsNullOrEmpty(createUserRequest.Username) ? "" : createUserRequest.Username,
                TitleId = createUserRequest.TitleId,
                CategoryId = createUserRequest.CategoryId,
                Names = Map(createUserRequest.Names),
                OrgUnits = createUserRequest.OrgUnits,
                UserNationalId = createUserRequest.UserNationalId,
                MainOrgUnitId = createUserRequest.MainOrgUnitId,
                Gender = createUserRequest.GenderId,
                //GroupId = oAddUserProfileDTO.RoleId,
                IsManager = createUserRequest.IsManager,
                Password = Guid.NewGuid().ToString(),
                UserGroups = Map(createUserRequest.UserRoles, createUserRequest.Id),
                AllowMobile = createUserRequest.AllowMobile,
                InternalNumber = createUserRequest.InternalNumber,
                Id = createUserRequest.Id,
                UserGroupsData = createUserRequest.UserRoles



            };

        }



        public static List<UserGroupDTO> Map(this List<int> groups, int userId)
        {
            if (groups == null || groups.Count == 0)
                return new List<UserGroupDTO>();


            return groups.Select(g => new UserGroupDTO
            {
                GroupId = g,
                UserId = userId

            }).ToList();
        }



        public static UserGroupDTO Map(AssignRoleRequest assignRoleRequest)
        {
            if (assignRoleRequest == null)
                return new UserGroupDTO();


            return new UserGroupDTO
            {

                GroupId = assignRoleRequest.RoleId,
                UserId = assignRoleRequest.UserId


            };
        }
    }
}