using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApi.Domain
{
    public class Lookup
    {
        public int CategoryId { get; set; }
        public int Id { get; set; }
        public string Text { get; set; } 
        public string PrivilegeName { get; set; }
        public string PermisionName { get; set; }
    }
}
