using MCS.IntegrationServices.Models;
using System;
using System.Collections.Generic;

namespace MCS.IntegrationServices
{
    [Serializable()]
    public class UserOrgUnitVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LocalizationVM> LoclizationName { get; set; }
        public bool IsSelected { get; set; }
    }
}