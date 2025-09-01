using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.Groups;

namespace MCS.UI.Areas.User.Mappers.Groups
{
    public static class GroupMapper
    {
        public static List<GroupVM> Map(IList<GroupDTO> groupDTOs)
        {
            if (groupDTOs == null || !groupDTOs.Any())
            {
                return new List<GroupVM>();
            }
            List<GroupVM> groupVMs = groupDTOs
                .Select(groupDTO => new GroupVM()
                {
                    Id = groupDTO.Id,
                    LocalName = groupDTO.LocalName,
                    Name = LookupMapper.Map(groupDTO.Name)
                }).ToList();

            return groupVMs;
        }
        public static List<GroupDTO> Map(IList<GroupVM> groupVMs)
        {
            if (groupVMs == null || !groupVMs.Any())
            {
                return new List<GroupDTO>();
            }
            List<GroupDTO> groupDTOs = groupVMs
                .Select(groupVM => new GroupDTO()
                {
                    Id = groupVM.Id,
                    LocalName = groupVM.LocalName,
                    Name = LookupMapper.Map(groupVM.Name)
                }).ToList();

            return groupDTOs;
        }
        public static List<AddGroupVM> Map(IList<AddGroupDTO> addGroupDTOs)
        {
            if (addGroupDTOs == null || !addGroupDTOs.Any())
            {
                return new List<AddGroupVM>();
            }
            List<AddGroupVM> addGroupVMs = addGroupDTOs
                .Select(addGroupDTO => new AddGroupVM()
                {
                    Name = LookupMapper.Map(addGroupDTO.Name),
                    Permissions = addGroupDTO.Permissions
                }).ToList();

            return addGroupVMs;
        }
        public static List<AddGroupDTO> Map(IList<AddGroupVM> addGroupVMs)
        {
            if (addGroupVMs == null || !addGroupVMs.Any())
            {
                return new List<AddGroupDTO>();
            }
            List<AddGroupDTO> AddGroupDTO = addGroupVMs
                .Select(addGroupVM => new AddGroupDTO()
                {
                    Name = LookupMapper.Map(addGroupVM.Name),
                    Permissions = addGroupVM.Permissions
                }).ToList();

            return AddGroupDTO;
        }
        public static List<EditGroupVM> Map(IList<EditGroupDTO> editGroupDTOs)
        {
            if (editGroupDTOs == null || !editGroupDTOs.Any())
            {
                return new List<EditGroupVM>();
            }
            List<EditGroupVM> editGroupVMs = editGroupDTOs
                .Select(editGroupDTO => new EditGroupVM()
                {
                    Id = editGroupDTO.Id,
                    Name = LookupMapper.Map(editGroupDTO.Name),
                    Permissions = editGroupDTO.Permissions
                }).ToList();

            return editGroupVMs;
        }
        public static List<EditGroupDTO> Map(IList<EditGroupVM> editGroupVMs)
        {
            if (editGroupVMs == null || !editGroupVMs.Any())
            {
                return new List<EditGroupDTO>();
            }
            List<EditGroupDTO> editGroupDTOs = editGroupVMs
                .Select(editGroupVM => new EditGroupDTO()
                {
                    Id = editGroupVM.Id,
                    Name = LookupMapper.Map(editGroupVM.Name),
                    Permissions = editGroupVM.Permissions
                }).ToList();

            return editGroupDTOs;
        }

    }
}