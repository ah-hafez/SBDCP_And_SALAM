namespace MCS.DTO
{
    public class GroupDTO
    {
       public int Id { get; set; }
       public LookupDTO Name { get; set; }
       public string LocalName { get; set; }
        public bool IsActive { get; set; }
       public bool IsSelected { get; set; }
    }
}
