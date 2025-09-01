using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common;

namespace MCS.UI.Areas.Admin.Models.Shared
{
    public class SettingConfigVM
    {
        public bool IsRequired { get; set; } = true;
        public string RequiredMessage { get; set; }
        public string MaxLength { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public string RangeMessage { get; set; }
        public string Regx { get; set; }
        public string RegxMessage { get; set; }
        public string StaticName { get; set; }
        public string Label { get; set; }
        public string ClassName { get; set; }
        public string LogoHeight { get; set; }
        public string LogoWidth { get; set; }
        public LookupCategory LookupCategory { get; set; }
        public ConnectionProtocolType[] ConnectionProtocolTypes { get; set; }
        public ConnectionProtocolType ConnectionProtocolType { get; set; }
        public ControlType ControlType { get; set; } = ControlType.Numeric;
    }
}