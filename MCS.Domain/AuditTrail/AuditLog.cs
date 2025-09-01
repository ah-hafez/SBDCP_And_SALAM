using Audit.EntityFramework;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
  
    public class AuditLog   
    { 
        public int Id { get; set; } 
        public string TableName { get; set; } 
        public string AuditData { get; set; }
        public DateTime AuditDate { get; set; }
        public string AuditAction { get; set; }
        
        public int AuditUser { get; set; }
        public string TablePk {  get; set; }
        public string EntityType {  get; set; } 
        public string GuidId {  get; set; }



    }
}
