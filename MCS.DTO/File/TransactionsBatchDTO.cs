using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MCS.DTO.File
{
    public  class TransactionsBatchDTO
    {

     

        //TODO: To Use CustomDisplayName instead of Display attribute   
      [Display(Name = "اسم الحزمة ")]
       public string BatchName { get; set; }


      //TODO: To Use CustomDisplayName instead of Display attribute   
       [Display(Name = "  هل تريد ربط المعاملات فيما بينها ")]
      public bool IsLinked { get; set; }
       public List<int> TransIds { get; set; }
      
    }
}
