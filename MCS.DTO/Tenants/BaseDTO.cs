using System;

namespace MCS.DTO.Tenants
{
    public class BaseDTO
    {
        public int Id { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModefiedBy { get; set; }
        public DateTime? ModefiedOn { get; set; }
    }
}
