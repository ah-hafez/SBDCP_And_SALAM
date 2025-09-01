using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Groups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class GroupMapper
    {
        public static List<GroupDTO> Map(IList<GroupVM> groupVMs)
        {
            if (groupVMs == null || !groupVMs.Any())
            { return null; }
            List<GroupDTO> groupDTOs = groupVMs
              .Select(data => new GroupDTO
              {
                  Id = data.Id,
                  LocalName = data.LocalName,
                  Name = LookupMapper.Map(data.Name),
              }).ToList();
            return groupDTOs;
        }
        public static List<GroupVM> Map(IList<GroupDTO> groupDTOs)
        {
            if (groupDTOs == null || !groupDTOs.Any())
            {
                return new List<GroupVM>();
            }
            List<GroupVM> groupVMs = groupDTOs
              .Select(data => new GroupVM
              {
                  Id = data.Id,
                  LocalName = data.LocalName,
                  IsActive = data.IsActive,
                  Name = LookupMapper.Map(data.Name),
                  IsSelected = data.IsSelected
              }).ToList();
            return groupVMs;

        }

        public static GroupVM Map(GroupDTO groupDTO)
        {
            if (groupDTO == null)
            {
                return new GroupVM();
            }

            GroupVM groupVM = new GroupVM
            {
                Id = groupDTO.Id,
                LocalName = groupDTO.LocalName,
                IsActive = groupDTO.IsActive,
                Name = LookupMapper.Map(groupDTO.Name),
            };
            return groupVM;

        }
        public static List<AddGroupVM> Map(IList<AddGroupDTO> addGroupDTOs)
        {
            if (addGroupDTOs == null || !addGroupDTOs.Any())
            { return null; }
            List<AddGroupVM> addGroupVMs = addGroupDTOs
              .Select(addGroupDTO => new AddGroupVM
              {
                  Name = LookupMapper.Map(addGroupDTO.Name),
                  Permissions = addGroupDTO.Permissions,
              }).ToList();
            return addGroupVMs;

        }
        public static AddGroupVM Map(AddGroupDTO addGroupDTOs)
        {
            if (addGroupDTOs != null)
            {
                AddGroupVM addGroupVMs = new AddGroupVM()
                {
                    Name = LookupMapper.Map(addGroupDTOs.Name),
                    Permissions = addGroupDTOs.Permissions,
                };
                return addGroupVMs;
            }
            return null;
        }
        public static List<AddGroupDTO> Map(IList<AddGroupVM> addGroupVMs)
        {
            if (addGroupVMs == null || !addGroupVMs.Any())
            { return null; }
            List<AddGroupDTO> addGroupDTOs = addGroupVMs
              .Select(addGroupVM => new AddGroupDTO
              {
                  Name = LookupMapper.Map(addGroupVM.Name),
                  Permissions = addGroupVM.Permissions,
              }).ToList();
            return addGroupDTOs;

        }
        public static AddGroupDTO Map(AddGroupVM addGroupVM)
        {
            if (addGroupVM != null)
            {
                AddGroupDTO addGroupDTO = new AddGroupDTO()
                {
                    Name = LookupMapper.Map(addGroupVM.Name),
                    Permissions = addGroupVM.Permissions,
                };
                return addGroupDTO;
            }
            return null;
        }
        public static List<EditGroupDTO> Map(IList<EditGroupVM> editGroupVMs)
        {
            if (editGroupVMs == null || !editGroupVMs.Any())
            { return null; }
            List<EditGroupDTO> editGroupDTOs = editGroupVMs
              .Select(editGroupVM => new EditGroupDTO
              {
                  Id = editGroupVM.Id,
                  Name = LookupMapper.Map(editGroupVM.Name),
                  Permissions = editGroupVM.Permissions,
              }).ToList();
            return editGroupDTOs;

        }
        public static EditGroupDTO Map(EditGroupVM editGroupVMs)
        {
            if (editGroupVMs != null)
            {
                EditGroupDTO editGroupDTOs = new EditGroupDTO()
                {
                    Id = editGroupVMs.Id,
                    Name = LookupMapper.Map(editGroupVMs.Name),
                    Permissions = editGroupVMs.Permissions
                };
                return editGroupDTOs;
            }
            return null;

        }
        public static EditGroupVM Map(EditGroupDTO editGroupDTO)
        {
            if (editGroupDTO != null)
            {
                EditGroupVM editGroupVM = new EditGroupVM()
                {
                    Id = editGroupDTO.Id,
                    Name = LookupMapper.Map(editGroupDTO.Name),
                    Permissions = editGroupDTO.Permissions
                };
                return editGroupVM;
            }
            return null;

        }







    }
}