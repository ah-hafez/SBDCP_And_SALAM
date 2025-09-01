using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{


    public class Entity
    {

        public int Id { get; set; }
        public bool IsCabinet { get; set; }
        public bool ExternalSend { get; set; }
        public bool ExternalReceive { get; set; }
        public int ImportantParent { get; set; }
        public int? ParentId { get; set; }
        public string Lineage { get; set; }
        public bool? Active { get; set; }
        public string Description { get; set; }
        public string UserDefinedId { get; set; }
        public int CabinetId { get; set; }
        public string Name { get; set; }
        public bool ActionTrace { get; set; }
        public int ReplaceByChild { get; set; }
        public string BarCodeSymbol { get; set; }
        public List<Person> Persons { get; set; }
        public List<Entity> Childs { get; set; }
        public bool IsVirtual { get; set; }
        public bool HasChilds { get; set; }
        
    }
}