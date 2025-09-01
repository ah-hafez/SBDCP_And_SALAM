using System;

namespace MCS.DTO
{
    public class UserTransactionsTrayDTO
    {
       public int Id { get; set; }
       public string DateH { get; set; }
       public DateTime Date { get; set; }
       public long Number { get; set; }
       public string DocumentNumber { get; set; }
       public int ConfedentialityId { get; set; }
       public int TransactionCategoryId { get; set; }
       public PriorityDTO PriorityLevel { get; set; }
       public int StatusId { get; set; }
       public UserProfileDTO ToUser  { get; set; }
       public UserProfileDTO FromUser { get; set; }
       public OrgUnitDTO FromEntity  { get; set; }
       public OrgUnitDTO ToEntity { get; set; }
       public DateTime? RemindDate { get; set; }
       public string RemindDateH { get; set; }
       public bool Islate { get; set; }
    }
}
