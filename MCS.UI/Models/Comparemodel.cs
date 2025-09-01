using System;

namespace MCS.UI.Models
{
    public class Comparemodel
    {
        // [CustomDateTimeCompare("Max", Operation.LessThan, ErrorMessage = "start date needs to be less than end date")]
        //// [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)] 
        // // [DataType(DataType.DateTime)]
        public DateTime? Min { get; set; }

        //[CustomDateTimeCompare("Min", Operation.GreaterThan, ErrorMessage = "End date needs to be greater than start date")]
        ////[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        ////   [DataType(DataType.DateTime)]
        public DateTime? Max { get; set; }

        public string Name { get; set; }
    }
}