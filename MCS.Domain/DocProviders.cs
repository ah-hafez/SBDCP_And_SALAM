using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;
using MCS.Common;

namespace MCS.Domain
{
    public class DocProviders: EntityBase
    {
        public string Provider_Type { get; set; }
        public int File_Id { get; set; }
        public string File_Url { get; set; }
        public int File_Doc_Id { get; set; }
        public eFileStatus File_Status { get; set; }
        public bool File_Is_Migrated { get; set; }
        public int TRANS_ID { get; set; }
    }
 
}
