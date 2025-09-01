using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApi.Domain
{
    public class Tray
    {
        public int TrayId { get; set; }
        public string Name { get; set; }
        public int Counter { get; set; }
        public List<string> AllowedActions { get; set; }
        public string PermissionsNeeded { get; set; }
        public bool OnlyForManager
        {
            get
            {
                return false;
            }
        }
    }
    public class TrayID
    {
        public int MyTransactions { get; set; } = 1;
        public int WithAppointment { get; set; } = 99;
        public int LateTransction { get; set; } = 100;
        public int OutboundDraft { get; set; } = 3;
    }
}
